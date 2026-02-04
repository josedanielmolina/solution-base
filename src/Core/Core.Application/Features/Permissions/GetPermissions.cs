using Core.Application.Common.Result;
using Core.Application.DTOs.Permissions;
using Core.Application.Mappings;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Permissions;

public class GetPermissions
{
    private readonly IPermissionRepository _permissionRepository;

    public GetPermissions(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<IEnumerable<PermissionDto>>> ExecuteAsync()
    {
        var permissions = await _permissionRepository.GetAllAsync();
        var dtos = permissions.Select(p => p.ToDto());
        
        return Result.Success(dtos);
    }

    public async Task<Result<IEnumerable<PermissionDto>>> ExecuteByModuleAsync(string module)
    {
        var permissions = await _permissionRepository.GetByModuleAsync(module);
        var dtos = permissions.Select(p => p.ToDto());
        
        return Result.Success(dtos);
    }
}
