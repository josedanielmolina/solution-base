namespace Core.Application.DTOs.Users;

public record UserSearchDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? Document
);
