using FluentValidation;
using RiffBackend.Application.Requests;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Validators;

public sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Email").Serialize())
            .EmailAddress().WithMessage(Errors.General.ValueIsInvalid("Email").Serialize());

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Password").Serialize());
    }
}
