using Microsoft.AspNetCore.Http;
using RiffBackend.Application.Common;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Services;

public class UserService(IUserRepository userRepository,
                         IFileStorageService fileStorage,
                         IPasswordHasher hasher,
                         IJwtProvider jwtProvider,
                         IFileProcessor fileProcessor) : IUserService
{
    private readonly IUserRepository _repository = userRepository;
    private readonly IFileStorageService _storage = fileStorage;
    private readonly IFileProcessor _fileProcessor = fileProcessor;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<User>> GetByIdAsync(Guid id)
    {
        User? user = await _repository.GetUserByIdAsync(id);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        var result = await _storage.GetURLAsync(user.AvatarPath);
        if (result.IsFailure) 
        { 
            return result.Error;
        }

        user.AvatarPath = result.Value!;

        return user;
    }

    public async Task<Result<User>> GetUserAsync(string jwt)
    {
        if (string.IsNullOrEmpty(jwt))
        {
            return Errors.General.ValueIsRequired("JWT");
        }

        var id = _jwtProvider.GetGuidFromJwt(jwt);

        User? user = await _repository.GetUserByIdAsync(id);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        var result = await _storage.GetURLAsync(user.AvatarPath);
        if (result.IsFailure)
        {
            return result.Error;
        }

        user.AvatarPath = result.Value!;

        return user;
    }

    public async Task<Result<Guid>> RegisterAsync(string name, string email, string password, IFormFile avatar)
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

        if (avatar != null)
        {
            var avatarPathResult = await _fileProcessor.UploadNewOrKeepOldAsync(avatar, "", _storage.UploadImageFileAsync);
            if (avatarPathResult.IsFailure)
            {
                return avatarPathResult.Error;
            }
            avatarPath = avatarPathResult.Value!;
        }

        return await _repository.AddUserAsync(User.Create(id, name, email, _hasher.Hash(password), avatarPath));
    }

    public async Task<Result<Guid>> UpdateAsync(Guid id,string name, string email, string password, IFormFile avatar)
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

        var avatarPath = User.DEFAULT_AVATAR_PATH;
        if (avatar != null)
        {
            var avatarPathResult = await _fileProcessor.UploadNewOrKeepOldAsync(avatar, user.AvatarPath, _storage.UploadImageFileAsync);
            if (avatarPathResult.IsFailure)
            {
                return avatarPathResult.Error;
            }
            avatarPath = avatarPathResult.Value!;
        }

        return await _repository.UpdateUserAsync(User.Create(id, name, email, _hasher.Hash(password), avatarPath));
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

        if (user.AvatarPath != User.DEFAULT_AVATAR_PATH)
        {
            var result = await _storage.DeleteFileAsync(user.AvatarPath);
            if (result.IsFailure)
            {
                return result.Error;
            }
        }

        return await _repository.DeleteUserAsync(id); ;
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
