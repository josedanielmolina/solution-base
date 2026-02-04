using Core.Application.DTOs.Auth;
using FluentValidation;

namespace Core.Application.Validators;

public class RequestPasswordResetValidator : AbstractValidator<RequestPasswordResetDto>
{
    public RequestPasswordResetValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
    }
}
