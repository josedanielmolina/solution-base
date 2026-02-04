using Core.Application.DTOs.Roles;
using FluentValidation;

namespace Core.Application.Validators;

public class UpdateRolePermissionsValidator : AbstractValidator<UpdateRolePermissionsDto>
{
    public UpdateRolePermissionsValidator()
    {
        RuleFor(x => x.PermissionIds)
            .NotNull().WithMessage("Permission IDs are required");
    }
}
