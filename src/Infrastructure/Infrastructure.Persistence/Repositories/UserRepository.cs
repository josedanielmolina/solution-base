using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByDocumentAsync(string document)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Document != null && u.Document.ToLower() == document.ToLower());
    }

    public async Task<User?> GetWithRolesAsync(int id)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetWithRolesAndPermissionsAsync(int id)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> SearchAsync(string query, int limit = 5)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<User>();

        var lowerQuery = query.ToLower();

        return await _dbSet
            .Where(u => u.IsActive &&
                (u.Email.ToLower().Contains(lowerQuery) ||
                 (u.Document != null && u.Document.ToLower().Contains(lowerQuery)) ||
                 u.FirstName.ToLower().Contains(lowerQuery) ||
                 u.LastName.ToLower().Contains(lowerQuery) ||
                 (u.FirstName + " " + u.LastName).ToLower().Contains(lowerQuery)))
            .Take(limit)
            .ToListAsync();
    }
}

