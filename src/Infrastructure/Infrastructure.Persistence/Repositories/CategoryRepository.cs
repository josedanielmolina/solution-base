using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.Country)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.Country)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Gender)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllActiveAsync()
    {
        return await _context.Categories
            .Include(c => c.Country)
            .Where(c => c.IsActive)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Gender)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetByCountryAsync(int countryId)
    {
        return await _context.Categories
            .Where(c => c.CountryId == countryId && c.IsActive)
            .OrderBy(c => c.Gender)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetByGenderAsync(Gender gender)
    {
        return await _context.Categories
            .Include(c => c.Country)
            .Where(c => c.Gender == gender && c.IsActive)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetByCountryAndGenderAsync(int countryId, Gender gender)
    {
        return await _context.Categories
            .Where(c => c.CountryId == countryId && c.Gender == gender && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category> AddAsync(Category entity)
    {
        await _context.Categories.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(Category entity)
    {
        _context.Categories.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByNameInCountryAndGenderAsync(string name, int countryId, Gender gender)
    {
        return await _context.Categories.AnyAsync(c => 
            c.Name == name && c.CountryId == countryId && c.Gender == gender);
    }
}
