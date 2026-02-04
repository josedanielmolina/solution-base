using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id);
    Task<IEnumerable<Permission>> GetAllAsync();
    Task<IEnumerable<Permission>> GetByModuleAsync(string module);
    Task<Permission?> GetByCodeAsync(string code);
    Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<int> ids);
    Task<IEnumerable<string>> GetCodesByUserIdAsync(int userId);
}
