using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Permission> _dbSet;

    public PermissionRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Permission>();
    }

    public async Task<Permission?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync()
    {
        return await _dbSet.OrderBy(p => p.Module).ThenBy(p => p.Name).ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetByModuleAsync(string module)
    {
        return await _dbSet
            .Where(p => p.Module.ToLower() == module.ToLower())
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Permission?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Code.ToLower() == code.ToLower());
    }

    public async Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _dbSet
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetCodesByUserIdAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .Distinct()
            .ToListAsync();
    }
}
