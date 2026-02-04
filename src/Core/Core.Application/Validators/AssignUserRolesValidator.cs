using Core.Application.DTOs.Users;
using FluentValidation;

namespace Core.Application.Validators;

public class AssignUserRolesValidator : AbstractValidator<AssignUserRolesDto>
{
    public AssignUserRolesValidator()
    {
        RuleFor(x => x.RoleIds)
            .NotNull().WithMessage("Role IDs are required")
            .NotEmpty().WithMessage("At least one role must be assigned");
    }
}
