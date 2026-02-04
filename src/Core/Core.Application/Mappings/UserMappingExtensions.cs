using Core.Application.DTOs.Users;
using Core.Domain.Entities;

namespace Core.Application.Mappings;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        var roles = user.UserRoles?.Select(ur => ur.Role.Name) ?? Enumerable.Empty<string>();
        
        return new UserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Document,
            user.Phone,
            user.IsActive,
            user.RequiresPasswordChange,
            roles,
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
            Document = dto.Document,
            Phone = dto.Phone,
            PasswordHash = passwordHash,
            IsActive = true,
            RequiresPasswordChange = true // New users must change password
        };
    }

    public static void UpdateEntity(this UpdateUserDto dto, User user)
    {
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Document = dto.Document;
        user.Phone = dto.Phone;
        user.IsActive = dto.IsActive;
    }

    public static void UpdateProfileEntity(this UpdateProfileDto dto, User user)
    {
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Phone = dto.Phone;
    }
}

