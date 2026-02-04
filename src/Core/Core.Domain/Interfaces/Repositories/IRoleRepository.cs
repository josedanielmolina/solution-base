using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByIdWithPermissionsAsync(int id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<IEnumerable<Role>> GetAllWithPermissionsAsync();
    Task<Role?> GetByNameAsync(string name);
    Task UpdatePermissionsAsync(int roleId, IEnumerable<int> permissionIds);
}
