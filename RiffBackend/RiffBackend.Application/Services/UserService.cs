using FluentEmail.Core;
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
                         IFileProcessor fileProcessor,
                         IEmailVerificationLinkFactory linkFactory,
                         IFluentEmail fluentEmail) : IUserService
{
    private readonly IUserRepository _repository = userRepository;
    private readonly IFileStorageService _storage = fileStorage;
    private readonly IFileProcessor _fileProcessor = fileProcessor;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IEmailVerificationLinkFactory _linkFactory = linkFactory;
    private readonly IFluentEmail _fluentEmail = fluentEmail;

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
        User? clone = await _repository.GetUserByIdAsync(id, ct);

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
        
        var token = await _repository.AddUserAsync(
            User.Create(id, name, email, _hasher.Hash(password), avatarResult.Value!, false), ct);
        
        var verificationLinkResult = CreateLink(token);
        
        if (verificationLinkResult.IsFailure)
        {
            return verificationLinkResult.Error;
        }
        
        await _fluentEmail
            .To(email)
            .Subject("Подтверждение email для riff")
            .Body($"Чтобы подтвердить email перейдите по <a href='{verificationLinkResult.Value!}'>ссылке</a>")
            .SendAsync();
        
        return id;
    }

    private Result<string> CreateLink(EmailVerificationToken token)
    {
        var link = _linkFactory.Create(token);

        if (string.IsNullOrEmpty(link))
        {
            return Errors.General.ValueIsRequired();
        }

        return link;
    }

    public async Task<Result<bool>> VerifyEmailAsync(Guid tokenId, CancellationToken ct = default)
    {
        var token = await _repository.GetEmailVerificationTokenAsync(tokenId, ct);

        if (token is null || token.ExpiresAtUtc < DateTime.UtcNow || token.User.EmailVerified)
        {
            return Errors.UserErrors.NotFound();
        }

        await _repository.DeleteEmailVerificationTokenAsync(tokenId, ct);

        return true;
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
            User.Create(id, name, email, _hasher.Hash(password), avatarResult.Value!, user.EmailVerified), ct);
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

        if (!user.EmailVerified)
        {
            return Errors.UserErrors.EmailNotVerified();
        }

        return _jwtProvider.GenerateToken(user);
    }
}
