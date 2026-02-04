using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EstablishmentRepository : IEstablishmentRepository
{
    private readonly ApplicationDbContext _context;

    public EstablishmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Establishment>> GetAllAsync(bool activeOnly = true)
    {
        var query = _context.Establishments
            .Include(e => e.Country)
            .Include(e => e.City)
            .AsQueryable();

        if (activeOnly)
            query = query.Where(e => e.IsActive);

        return await query.OrderBy(e => e.Name).ToListAsync();
    }

    public async Task<Establishment?> GetByIdAsync(int id)
    {
        return await _context.Establishments
            .Include(e => e.Country)
            .Include(e => e.City)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Establishment?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Establishments
            .Include(e => e.Country)
            .Include(e => e.City)
            .Include(e => e.Courts.Where(c => c.IsActive))
                .ThenInclude(c => c.Photos.OrderBy(p => p.DisplayOrder))
            .Include(e => e.Photos.OrderBy(p => p.DisplayOrder))
            .Include(e => e.Schedules.OrderBy(s => s.DayOfWeek).ThenBy(s => s.BlockNumber))
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Establishment>> GetByCityAsync(int cityId)
    {
        return await _context.Establishments
            .Include(e => e.Country)
            .Include(e => e.City)
            .Where(e => e.CityId == cityId && e.IsActive)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Establishment>> SearchByNameAsync(string searchTerm)
    {
        return await _context.Establishments
            .Include(e => e.Country)
            .Include(e => e.City)
            .Where(e => e.IsActive && e.Name.Contains(searchTerm))
            .OrderBy(e => e.Name)
            .Take(20)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var query = _context.Establishments.Where(e => e.Name == name);
        if (excludeId.HasValue)
            query = query.Where(e => e.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task AddAsync(Establishment establishment)
    {
        await _context.Establishments.AddAsync(establishment);
    }

    public Task UpdateAsync(Establishment establishment)
    {
        _context.Establishments.Update(establishment);
        return Task.CompletedTask;
    }

    // Photos
    public async Task AddPhotoAsync(EstablishmentPhoto photo)
    {
        await _context.Set<EstablishmentPhoto>().AddAsync(photo);
    }

    public async Task RemovePhotoAsync(int photoId)
    {
        var photo = await _context.Set<EstablishmentPhoto>().FindAsync(photoId);
        if (photo != null)
            _context.Set<EstablishmentPhoto>().Remove(photo);
    }

    // Schedules
    public async Task SetSchedulesAsync(int establishmentId, IEnumerable<EstablishmentSchedule> schedules)
    {
        // Remove existing schedules
        var existing = await _context.Set<EstablishmentSchedule>()
            .Where(s => s.EstablishmentId == establishmentId)
            .ToListAsync();
        _context.Set<EstablishmentSchedule>().RemoveRange(existing);

        // Add new schedules
        foreach (var schedule in schedules)
        {
            schedule.EstablishmentId = establishmentId;
            await _context.Set<EstablishmentSchedule>().AddAsync(schedule);
        }
    }

    public async Task<IEnumerable<EstablishmentSchedule>> GetSchedulesAsync(int establishmentId)
    {
        return await _context.Set<EstablishmentSchedule>()
            .Where(s => s.EstablishmentId == establishmentId)
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.BlockNumber)
            .ToListAsync();
    }
}
