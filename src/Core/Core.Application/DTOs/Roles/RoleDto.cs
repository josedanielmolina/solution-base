namespace Core.Application.DTOs.Roles;

public record RoleDto(
    int Id,
    string Name,
    string? Description,
    bool IsSystemRole
);
