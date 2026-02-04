namespace Core.Application.DTOs.Users;

public record UpdateProfileDto(
    string FirstName,
    string LastName,
    string? Phone
);
