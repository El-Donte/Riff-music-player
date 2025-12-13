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

    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        if(id == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        User? user = await _repository.GetUserByIdAsync(id, ct);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        var result = await _storage.GetURLAsync(user.AvatarPath, ct);
        if (result.IsFailure) 
        { 
            return result.Error;
        }

        user.AvatarPath = result.Value!;

        return user;
    }

    public async Task<Result<User>> GetUserFromJwtAsync(string jwt, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(jwt))
        {
            return Errors.General.ValueIsRequired("JWT");
        }

        var id = _jwtProvider.GetGuidFromJwt(jwt);

        User? user = await _repository.GetUserByIdAsync(id, ct);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        var result = await _storage.GetURLAsync(user.AvatarPath, ct);
        if (result.IsFailure)
        {
            return result.Error;
        }

        user.AvatarPath = result.Value!;

        return user;
    }

    public async Task<Result<Guid>> RegisterAsync(Guid id,string name, string email, 
        string password, IFormFile avatar, CancellationToken ct = default)
    {
        User? clone = await _repository.GetUserByIdAsync(id);

        if (clone != null)
        {
            return Errors.General.AlreadyExist();
        }

        clone = await _repository.GetByEmailAsync(email, ct);

        if (clone != null)
        {
            return Errors.UserErrors.EmailDuplicate(email);
        }

        var avatarResult = await _fileProcessor.UploadNewOrKeepOldAsync(avatar, 
                                    User.DEFAULT_AVATAR_PATH, ct, _storage.UploadImageFileAsync);

        if (avatarResult.IsFailure)
        {
            return avatarResult.Error;
        }

        return await _repository.AddUserAsync(
            User.Create(id, name, email, _hasher.Hash(password), avatarResult.Value!));
    }

    public async Task<Result<Guid>> UpdateAsync(Guid id,string name, string email, 
        string password, IFormFile avatar, CancellationToken ct = default)
    {
        if(id == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        User? user = await _repository.GetByEmailAsync(email, ct);

        if (user != null && user.Id != id)
        {
            return Errors.UserErrors.EmailDuplicate(email);
        }

        user = await _repository.GetUserByIdAsync(id, ct);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        var avatarResult = await _fileProcessor.UploadNewOrKeepOldAsync(avatar,
                                    user.AvatarPath, ct, _storage.UploadImageFileAsync);

        if (avatarResult.IsFailure)
        {
            return avatarResult.Error;
        }

        return await _repository.UpdateUserAsync(
            User.Create(id, name, email, _hasher.Hash(password), avatarResult.Value!));
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        User? user = await _repository.GetUserByIdAsync(id, ct);
        if (user is null)
        {
            return Errors.UserErrors.NotFound(id);
        }

        if (user.AvatarPath != User.DEFAULT_AVATAR_PATH)
        {
            var result = await _storage.DeleteFileAsync(user.AvatarPath, ct);
            if (result.IsFailure)
            {
                return result.Error;
            }
        }

        return await _repository.DeleteUserAsync(id, ct);
    }

    public async Task<Result<string>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Errors.General.ValueIsRequired("почта");
        }

        if (string.IsNullOrEmpty(password))
        {
            return Errors.General.ValueIsRequired("пароль");
        }

        User? user = await _repository.GetByEmailAsync(email, ct);

        if (user is null)
        {
            return Errors.UserErrors.NotFound(null,email);
        }

        if(!_hasher.Verify(password, user.PasswordHash))
        {
            return Errors.UserErrors.IncorrectPassword();
        }

        return _jwtProvider.GenerateToken(user);
    }
}
