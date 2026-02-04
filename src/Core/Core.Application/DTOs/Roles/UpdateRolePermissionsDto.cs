namespace Core.Application.DTOs.Roles;

public record UpdateRolePermissionsDto(
    IEnumerable<int> PermissionIds
);
