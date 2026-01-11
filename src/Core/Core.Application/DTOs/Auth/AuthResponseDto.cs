namespace Core.Application.DTOs.Auth;

public record AuthResponseDto(
    string Token,
    AuthUserDto User
);

public record AuthUserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email
);
