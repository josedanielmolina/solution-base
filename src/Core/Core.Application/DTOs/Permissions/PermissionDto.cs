namespace Core.Application.DTOs.Permissions;

public record PermissionDto(
    int Id,
    string Code,
    string Name,
    string? Description,
    string Module
);
