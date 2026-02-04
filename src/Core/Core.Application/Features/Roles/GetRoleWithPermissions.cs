using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Roles;
using Core.Application.Mappings;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Roles;

public class GetRoleWithPermissions
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleWithPermissions(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<RoleWithPermissionsDto>> ExecuteAsync(int roleId)
    {
        var role = await _roleRepository.GetByIdWithPermissionsAsync(roleId);
        if (role == null)
        {
            return Result.Failure<RoleWithPermissionsDto>(RoleErrors.NotFound(roleId));
        }

        return Result.Success(role.ToWithPermissionsDto());
    }
}
