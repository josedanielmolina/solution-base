using Core.Application.DTOs.Permissions;

namespace Core.Application.DTOs.Roles;

public record RoleWithPermissionsDto(
    int Id,
    string Name,
    string? Description,
    bool IsSystemRole,
    IEnumerable<PermissionDto> Permissions
);
