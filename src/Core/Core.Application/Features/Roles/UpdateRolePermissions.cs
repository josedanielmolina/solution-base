using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Roles;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Roles;

public class UpdateRolePermissions
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRolePermissions(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> ExecuteAsync(int roleId, UpdateRolePermissionsDto dto)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null)
        {
            return Result.Failure(RoleErrors.NotFound(roleId));
        }

        // Validate all permissions exist
        foreach (var permissionId in dto.PermissionIds)
        {
            var permission = await _permissionRepository.GetByIdAsync(permissionId);
            if (permission == null)
            {
                return Result.Failure(RoleErrors.PermissionNotFound(permissionId));
            }
        }

        // Update permissions
        await _roleRepository.UpdatePermissionsAsync(roleId, dto.PermissionIds);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
