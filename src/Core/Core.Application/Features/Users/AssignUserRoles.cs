using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Users;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Users;

public class AssignUserRoles
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignUserRoles(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> ExecuteAsync(int userId, AssignUserRolesDto dto)
    {
        var user = await _userRepository.GetWithRolesAsync(userId);
        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound(userId));
        }

        // Validate all roles exist
        foreach (var roleId in dto.RoleIds)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return Result.Failure(RoleErrors.NotFound(roleId));
            }
        }

        // Clear existing roles
        user.UserRoles.Clear();

        // Add new roles
        foreach (var roleId in dto.RoleIds)
        {
            user.UserRoles.Add(new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow
            });
        }

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
