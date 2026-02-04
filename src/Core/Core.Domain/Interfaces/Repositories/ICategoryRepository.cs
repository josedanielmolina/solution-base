using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<IEnumerable<Category>> GetAllActiveAsync();
    Task<IEnumerable<Category>> GetByCountryAsync(int countryId);
    Task<IEnumerable<Category>> GetByGenderAsync(Gender gender);
    Task<IEnumerable<Category>> GetByCountryAndGenderAsync(int countryId, Gender gender);
    Task<Category> AddAsync(Category entity);
    Task UpdateAsync(Category entity);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameInCountryAndGenderAsync(string name, int countryId, Gender gender);
}
