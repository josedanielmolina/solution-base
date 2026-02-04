using Core.Application.DTOs.Permissions;
using Core.Domain.Entities;

namespace Core.Application.Mappings;

public static class PermissionMappingExtensions
{
    public static PermissionDto ToDto(this Permission permission)
    {
        return new PermissionDto(
            permission.Id,
            permission.Code,
            permission.Name,
            permission.Description,
            permission.Module
        );
    }
}
