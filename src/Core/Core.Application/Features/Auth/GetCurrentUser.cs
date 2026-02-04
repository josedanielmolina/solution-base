using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Auth;

public class GetCurrentUser
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;

    public GetCurrentUser(
        IUserRepository userRepository,
        IPermissionRepository permissionRepository)
    {
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<AuthUserDto>> ExecuteAsync(int userId)
    {
        var user = await _userRepository.GetWithRolesAsync(userId);
        if (user == null)
        {
            return Result.Failure<AuthUserDto>(AuthErrors.UserNotFound);
        }

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var permissions = await _permissionRepository.GetCodesByUserIdAsync(userId);

        var response = new AuthUserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            roles,
            permissions
        );

        return Result.Success(response);
    }
}
