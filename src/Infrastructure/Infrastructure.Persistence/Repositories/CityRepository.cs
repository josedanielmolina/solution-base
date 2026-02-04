using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<City?> GetByIdAsync(int id)
    {
        return await _context.Cities
            .Include(c => c.Country)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        return await _context.Cities
            .Include(c => c.Country)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetAllActiveAsync()
    {
        return await _context.Cities
            .Include(c => c.Country)
            .Where(c => c.IsActive)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetByCountryAsync(int countryId)
    {
        return await _context.Cities
            .Where(c => c.CountryId == countryId && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<City?> GetByNameInCountryAsync(string name, int countryId)
    {
        return await _context.Cities
            .FirstOrDefaultAsync(c => c.Name == name && c.CountryId == countryId);
    }

    public async Task<City> AddAsync(City entity)
    {
        await _context.Cities.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(City entity)
    {
        _context.Cities.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Cities.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByNameInCountryAsync(string name, int countryId)
    {
        return await _context.Cities.AnyAsync(c => c.Name == name && c.CountryId == countryId);
    }
}
