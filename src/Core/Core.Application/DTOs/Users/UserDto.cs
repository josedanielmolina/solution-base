namespace Core.Application.DTOs.Users;

public record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? Document,
    string? Phone,
    bool IsActive,
    bool RequiresPasswordChange,
    IEnumerable<string> Roles,
    DateTime? LastLoginAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

