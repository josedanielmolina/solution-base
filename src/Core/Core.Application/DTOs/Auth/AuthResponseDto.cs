namespace Core.Application.DTOs.Auth;

public record AuthResponseDto(
    string Token,
    AuthUserDto User,
    bool RequiresPasswordChange
);

public record AuthUserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    IEnumerable<string> Roles,
    IEnumerable<string> Permissions
);

