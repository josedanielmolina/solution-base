using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByDocumentAsync(string document);
    Task<User?> GetWithRolesAsync(int id);
    Task<User?> GetWithRolesAndPermissionsAsync(int id);
    Task<IEnumerable<User>> SearchAsync(string query, int limit = 5);
}

