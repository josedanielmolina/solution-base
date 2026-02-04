using Core.Application.DTOs.Roles;
using Core.Application.DTOs.Permissions;
using Core.Domain.Entities;

namespace Core.Application.Mappings;

public static class RoleMappingExtensions
{
    public static RoleDto ToDto(this Role role)
    {
        return new RoleDto(
            role.Id,
            role.Name,
            role.Description,
            role.IsSystemRole
        );
    }

    public static RoleWithPermissionsDto ToWithPermissionsDto(this Role role)
    {
        var permissions = role.RolePermissions?
            .Select(rp => rp.Permission.ToDto()) 
            ?? Enumerable.Empty<PermissionDto>();

        return new RoleWithPermissionsDto(
            role.Id,
            role.Name,
            role.Description,
            role.IsSystemRole,
            permissions
        );
    }
}
