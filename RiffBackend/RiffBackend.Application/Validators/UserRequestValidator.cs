using FluentValidation;
using Microsoft.AspNetCore.Http;
using RiffBackend.Application.Requests;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Validators;

public sealed class UserRequestValidator : AbstractValidator<UserRequest>
{
    private readonly static string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"];

    public UserRequestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("email").Serialize())
            .EmailAddress().WithMessage(Errors.General.ValueIsInvalid("email").Serialize());

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("password").Serialize())
            .MinimumLength(8).WithMessage(Errors.General.ValueIsInvalidLength("password").Serialize());

        RuleFor(u => u.Name)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("name").Serialize())
            .MaximumLength(50).WithMessage(Errors.General.ValueIsInvalidLength("name").Serialize());

        RuleFor(u => u.AvatarImage)
            .Must(BeImageType!).WithMessage(Errors.FileErrors.InvalidType(allowedExtensions).Serialize())
            .Must(MaxLength!).WithMessage(Errors.FileErrors.InvalidSize(20).Serialize());
    }

    private static bool BeImageType(IFormFile file)
    {
        if (file == null)
            return true;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (allowedExtensions.Contains(extension))
            return true;

        return false;
    }

    private static bool MaxLength(IFormFile file)
    {
        if (file == null)
            return true;

        return file.Length < 20 * 1024 * 1024;
    }
}
