using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface ICountryRepository
{
    Task<Country?> GetByIdAsync(int id);
    Task<IEnumerable<Country>> GetAllAsync();
    Task<IEnumerable<Country>> GetAllActiveAsync();
    Task<Country?> GetByCodeAsync(string code);
    Task<Country?> GetByNameAsync(string name);
    Task<Country> AddAsync(Country entity);
    Task UpdateAsync(Country entity);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByCodeAsync(string code);
}
