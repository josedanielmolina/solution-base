using Core.Application.Common.Result;
using Core.Application.DTOs.Users;

namespace Core.Application.Facades;

public interface IUserFacade
{
    Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto);
    Task<Result<UserDto>> GetUserByIdAsync(int id);
    Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
    Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto);
    Task<Result> DeleteUserAsync(int id);
}
