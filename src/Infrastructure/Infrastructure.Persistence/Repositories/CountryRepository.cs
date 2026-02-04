using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly ApplicationDbContext _context;

    public CountryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Country?> GetByIdAsync(int id)
    {
        return await _context.Countries.FindAsync(id);
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        return await _context.Countries.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<IEnumerable<Country>> GetAllActiveAsync()
    {
        return await _context.Countries
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Country?> GetByCodeAsync(string code)
    {
        return await _context.Countries
            .FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        return await _context.Countries
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<Country> AddAsync(Country entity)
    {
        await _context.Countries.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(Country entity)
    {
        _context.Countries.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Countries.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Countries.AnyAsync(c => c.Name == name);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Countries.AnyAsync(c => c.Code == code);
    }
}
