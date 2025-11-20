using Microsoft.AspNetCore.Http;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;
using System.Security.Cryptography;

namespace RiffBackend.Application.Services;

public class UserService(IUserRepository userRepository,
                         IFileStorageService fileStorage,
                         IPasswordHasher hasher,
                         IJwtProvider jwtProvider) : IUserService
{
    private readonly IUserRepository _repository = userRepository;
    private readonly IFileStorageService _fileStorage = fileStorage;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<User>> GetByIdAsync(Guid id)
    {
        User? user = await _repository.GetUserByIdAsync(id);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var result = await _fileStorage.GetURLAsync(user.AvatarUrl);
            if (result.IsFailure) 
            { 
                return result.Error;
            }

            user.AvatarUrl = result.Value!;
        }

        return user;
    }

    public async Task<Result<Guid>> RegisterAsync(string name, string email, string password, IFormFile image)
    {
        Guid id = Guid.NewGuid();
        User? clone = await _repository.GetUserByIdAsync(id);

        if (clone != null)
        {
            return Errors.General.AlreadyExist();
        }

        clone = await _repository.GetByEmailAsync(email);

        if (clone != null)
        {
            return Errors.UserErrors.EmailDuplicate(email);
        }

        string avatarPath = User.DEFAULT_AVATAR_PATH;

        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var uploadResult = await _fileStorage.UploadImageFileAsync(stream, image.FileName, image.ContentType);
            if (uploadResult.IsFailure)
            {
                return uploadResult.Error;
            }

            avatarPath = uploadResult.Value!;
        }

        User user = User.Create(id, name, email, _hasher.Hash(password), avatarPath);
        
        return await _repository.AddUserAsync(user);
    }

    public async Task<Result<Guid>> UpdateAsync(Guid id,string name, string email, string password, IFormFile file)
    {
        if(id == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        User? user = await _repository.GetByEmailAsync(email);

        if (user != null && user.Id != id)
        {
            return Errors.UserErrors.EmailDuplicate(email);
        }

        user = await _repository.GetUserByIdAsync(id);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }
        
        var etagResult = await _fileStorage.GetEtagAsync(user.AvatarUrl);
        if (etagResult.IsFailure)
        {
            return etagResult.Error;
        }

        if (file == null)
        {
            return Errors.FileErrors.MissingFile("Image");
        }
        
        var newUser = User.Create(id, name, email, _hasher.Hash(password), user.AvatarUrl);

        using var stream = file.OpenReadStream();

        var newHash = GetFileMd5(stream);
        var oldHash = etagResult.Value!;

        if (newHash != oldHash)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var uploadResult = await _fileStorage.UploadImageFileAsync(stream, file.FileName, file.ContentType);

            if (uploadResult.IsFailure)
            {
                return uploadResult.Error;
            }

            newUser.AvatarUrl = uploadResult.Value!;
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

        if (user.AvatarUrl != User.DEFAULT_AVATAR_PATH)
        {
            var result = await _fileStorage.DeleteFileAsync(user.AvatarUrl);
            if (result.IsFailure)
            {
                return result.Error;
            }
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

    public async Task<Result<string>> LoginAsync(string email, string password)
    {
        User? user = await _repository.GetByEmailAsync(email);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(null,email);
        }

        var verify = _hasher.Verify(password, user.PasswordHash);
        if(verify == false)
        {
            return Errors.UserErrors.IncorrectPassword();
        }

        return _jwtProvider.GenerateToken(user);
    }
}
