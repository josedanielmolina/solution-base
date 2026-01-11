namespace Core.Application.DTOs.Users;

public record UpdateUserDto(
    string FirstName,
    string LastName,
    bool IsActive
);
