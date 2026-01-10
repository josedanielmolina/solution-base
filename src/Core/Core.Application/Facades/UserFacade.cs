using Core.Application.Common.Result;
using Core.Application.DTOs.Users;
using Core.Application.Operations.Users;

namespace Core.Application.Facades;

public class UserFacade : IUserFacade
{
    private readonly CreateUserOperation _createUserOperation;
    private readonly GetUserOperation _getUserOperation;
    private readonly UpdateUserOperation _updateUserOperation;
    private readonly DeleteUserOperation _deleteUserOperation;

    public UserFacade(
        CreateUserOperation createUserOperation,
        GetUserOperation getUserOperation,
        UpdateUserOperation updateUserOperation,
        DeleteUserOperation deleteUserOperation)
    {
        _createUserOperation = createUserOperation;
        _getUserOperation = getUserOperation;
        _updateUserOperation = updateUserOperation;
        _deleteUserOperation = deleteUserOperation;
    }

    public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto)
    {
        return await _createUserOperation.ExecuteAsync(dto);
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(int id)
    {
        return await _getUserOperation.ExecuteAsync(id);
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        return await _getUserOperation.ExecuteAllAsync();
    }

    public async Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        return await _updateUserOperation.ExecuteAsync(id, dto);
    }

    public async Task<Result> DeleteUserAsync(int id)
    {
        return await _deleteUserOperation.ExecuteAsync(id);
    }
}
