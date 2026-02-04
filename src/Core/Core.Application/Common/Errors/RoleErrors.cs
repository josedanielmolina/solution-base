using Core.Application.Common.Result;

namespace Core.Application.Common.Errors;

public static class RoleErrors
{
    public static Error NotFound(int id) => Error.NotFound(
        "Role.NotFound",
        $"Role with ID {id} was not found");

    public static Error CannotModifySystemRole => Error.Validation(
        "Role.CannotModifySystemRole",
        "System roles cannot be modified");

    public static Error PermissionNotFound(int id) => Error.NotFound(
        "Permission.NotFound",
        $"Permission with ID {id} was not found");
}
