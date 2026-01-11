namespace Core.Application.DTOs.Users;

public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Password
);
