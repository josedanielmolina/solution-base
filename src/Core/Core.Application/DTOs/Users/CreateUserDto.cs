namespace Core.Application.DTOs.Users;

public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string? Document,
    string? Phone,
    string Password,
    IEnumerable<int>? RoleIds
);

