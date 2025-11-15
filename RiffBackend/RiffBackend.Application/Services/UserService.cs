using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;
using System.Security.Cryptography;

namespace RiffBackend.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IFileStorageService _fileStorage;

    public UserService(IUserRepository userRepository, IFileStorageService fileStorage)
    {
        _repository = userRepository;
        _fileStorage = fileStorage;
    }

    public async Task<Result<User>> GetByIdAsync(Guid id)
    {
        User? user = await _repository.GetUserByIdAsync(id);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        if (user.AvatarUrl != string.Empty || user.AvatarUrl != null)
        {
            var result = await _fileStorage.GetURLAsync(user.AvatarUrl);
            if (result.IsError) 
            { 
                return result.Error;
            }

            user.AvatarUrl = result.Value!;
        }

        return user;
    }

    public async Task<Result<Guid>> AddAsync(User user, Stream stream, string fileName, string contentType)
    {
        if(user.Id != Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        User? clone = await _repository.GetUserByIdAsync(user.Id);

        if (clone != null)
        {
            return Errors.General.AlreadyExist();
        }

        clone = await _repository.GetByEmailAsync(user.Email);

        if (clone != null)
        {
            return Errors.UserErrors.EmailDuplicate(user.Email);
        }

        var uploadResult = await _fileStorage.UploadImageFileAsync(stream, fileName, contentType);
        if (uploadResult.IsError)
        {
            return uploadResult.Error;
        }

        user.AvatarUrl = uploadResult.Value!;

        return await _repository.AddUserAsync(user);
    }

    public async Task<Result<Guid>> UpdateAsync(User newUser, Stream stream, string fileName, string contentType)
    {
        if(newUser.Id == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        User? user = await _repository.GetByEmailAsync(newUser.Email);

        if (user != null && user.Id != newUser.Id)
        {
            return Errors.UserErrors.EmailDuplicate(newUser.Email);
        }

        user = await _repository.GetUserByIdAsync(newUser.Id);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(newUser.Id);
        }
        
        var etagResult = await _fileStorage.GetEtagAsync(user.AvatarUrl);
        if (etagResult.IsError)
        {
            return etagResult.Error;
        }

        var newHash = GetFileMd5(stream);
        var oldHash = etagResult.Value!;
        if (newHash != oldHash)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var uploadResult = await _fileStorage.UploadImageFileAsync(stream, fileName, contentType);

            if (uploadResult.IsError) 
            {
                return uploadResult.Error;
            }

            newUser.AvatarUrl = uploadResult.Value!;
        }
        else
        {
            newUser.AvatarUrl = user.AvatarUrl;
        }

        return await _repository.UpdateUserAsync(newUser);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        User? user = await _repository.GetUserByIdAsync(id);
        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        var result = await _fileStorage.DeleteFileAsync(user.AvatarUrl);
        if (result.IsError) 
        {
            return result.Error;
        }

        return await _repository.DeleteUserAsync(id); ;
    }

    private string GetFileMd5(Stream stream)
    {
        using (var md5 = MD5.Create())
        using (stream)
        {
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
