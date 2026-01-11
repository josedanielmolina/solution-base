namespace Core.Application.DTOs.Users;

public record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool IsEmailVerified,
    DateTime? LastLoginAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
