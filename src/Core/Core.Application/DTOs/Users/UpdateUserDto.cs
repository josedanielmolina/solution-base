namespace Core.Application.DTOs.Users;

public record UpdateUserDto(
    string FirstName,
    string LastName,
    string? Document,
    string? Phone,
    bool IsActive
);

