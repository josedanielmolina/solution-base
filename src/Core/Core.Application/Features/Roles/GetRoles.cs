using Core.Application.Common.Result;
using Core.Application.DTOs.Roles;
using Core.Application.Mappings;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Roles;

public class GetRoles
{
    private readonly IRoleRepository _roleRepository;

    public GetRoles(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<IEnumerable<RoleDto>>> ExecuteAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        var dtos = roles.Select(r => r.ToDto());
        
        return Result.Success(dtos);
    }
}
