namespace Core.Application.DTOs.Users;

public record AssignUserRolesDto(
    IEnumerable<int> RoleIds
);
