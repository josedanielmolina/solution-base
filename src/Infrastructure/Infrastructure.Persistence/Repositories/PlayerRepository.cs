using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Player> _dbSet;

    public PlayerRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Player>();
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<Player?> GetByIdWithCityAsync(int id)
    {
        return await _dbSet
            .Include(p => p.City)
                .ThenInclude(c => c!.Country)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.City)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Player>> GetActiveAsync()
    {
        return await _dbSet
            .Include(p => p.City)
            .Where(p => !p.IsDeleted && p.IsActive)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Player>> FindAsync(Expression<Func<Player, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<Player>> SearchAsync(string searchTerm, int limit = 10)
    {
        var term = searchTerm.ToLower();
        return await _dbSet
            .Include(p => p.City)
            .Where(p => !p.IsDeleted && p.IsActive && (
                p.Document.ToLower().Contains(term) ||
                p.FirstName.ToLower().Contains(term) ||
                p.LastName.ToLower().Contains(term) ||
                (p.Email != null && p.Email.ToLower().Contains(term))
            ))
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<Player>> GetByCityAsync(int cityId)
    {
        return await _dbSet
            .Where(p => p.CityId == cityId && !p.IsDeleted && p.IsActive)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<Player?> GetByDocumentAsync(string document)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Document.ToLower() == document.ToLower());
    }

    public async Task<Player?> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Player> AddAsync(Player entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(Player entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task SoftDeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsByDocumentAsync(string document)
    {
        return await _dbSet.AnyAsync(p => p.Document.ToLower() == document.ToLower());
    }
}

