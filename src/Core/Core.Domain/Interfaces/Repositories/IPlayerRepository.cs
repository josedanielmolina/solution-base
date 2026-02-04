using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Domain.Interfaces.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(int id);
    Task<Player?> GetByIdWithCityAsync(int id);
    Task<IEnumerable<Player>> GetAllAsync();
    Task<IEnumerable<Player>> GetActiveAsync();
    Task<IEnumerable<Player>> FindAsync(Expression<Func<Player, bool>> predicate);
    Task<IEnumerable<Player>> SearchAsync(string searchTerm, int limit = 10);
    Task<IEnumerable<Player>> GetByCityAsync(int cityId);
    Task<Player?> GetByDocumentAsync(string document);
    Task<Player?> GetByUserIdAsync(int userId);
    Task<Player> AddAsync(Player entity);
    Task UpdateAsync(Player entity);
    Task SoftDeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByDocumentAsync(string document);
}

