using Core.Application.Common.Result;
using Core.Application.DTOs.Users;
using Core.Application.Features.Users;

namespace Core.Application.Facades;

public class UserFacade : IUserFacade
{
    private readonly CreateUser _createUser;
    private readonly GetUser _getUser;
    private readonly UpdateUser _updateUser;
    private readonly DeleteUser _deleteUser;

    public UserFacade(
        CreateUser createUser,
        GetUser getUser,
        UpdateUser updateUser,
        DeleteUser deleteUser)
    {
        _createUser = createUser;
        _getUser = getUser;
        _updateUser = updateUser;
        _deleteUser = deleteUser;
    }

    public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto)
    {
        return await _createUser.ExecuteAsync(dto);
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(int id)
    {
        return await _getUser.ExecuteAsync(id);
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        return await _getUser.ExecuteAllAsync();
    }

    public async Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        return await _updateUser.ExecuteAsync(id, dto);
    }

    public async Task<Result> DeleteUserAsync(int id)
    {
        return await _deleteUser.ExecuteAsync(id);
    }
}
