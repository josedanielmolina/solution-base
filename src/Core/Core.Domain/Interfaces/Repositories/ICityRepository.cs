using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface ICityRepository
{
    Task<City?> GetByIdAsync(int id);
    Task<IEnumerable<City>> GetAllAsync();
    Task<IEnumerable<City>> GetAllActiveAsync();
    Task<IEnumerable<City>> GetByCountryAsync(int countryId);
    Task<City?> GetByNameInCountryAsync(string name, int countryId);
    Task<City> AddAsync(City entity);
    Task UpdateAsync(City entity);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameInCountryAsync(string name, int countryId);
}
