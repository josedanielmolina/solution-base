using Core.Application.DTOs.Users;
using Core.Domain.Entities;

namespace Core.Application.Mappings;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.IsActive,
            user.IsEmailVerified,
            user.LastLoginAt,
            user.CreatedAt,
            user.UpdatedAt
        );
    }

    public static User ToEntity(this CreateUserDto dto, string passwordHash)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = passwordHash,
            IsActive = true,
            IsEmailVerified = false
        };
    }

    public static void UpdateEntity(this UpdateUserDto dto, User user)
    {
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.IsActive = dto.IsActive;
    }
}
