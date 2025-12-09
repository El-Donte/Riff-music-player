using FluentValidation;
using RiffBackend.Application.Requests;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Validators;

public sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("email").Serialize())
            .EmailAddress().WithMessage(Errors.General.ValueIsInvalid("email").Serialize());

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("password").Serialize());
    }
}
