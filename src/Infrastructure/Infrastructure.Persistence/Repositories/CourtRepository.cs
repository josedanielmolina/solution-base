using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourtRepository : ICourtRepository
{
    private readonly ApplicationDbContext _context;

    public CourtRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Court>> GetByEstablishmentAsync(int establishmentId, bool activeOnly = true)
    {
        var query = _context.Courts
            .Include(c => c.Photos.OrderBy(p => p.DisplayOrder))
            .Where(c => c.EstablishmentId == establishmentId);

        if (activeOnly)
            query = query.Where(c => c.IsActive);

        return await query.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Court?> GetByIdAsync(int id)
    {
        return await _context.Courts
            .Include(c => c.Establishment)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Court?> GetByIdWithPhotosAsync(int id)
    {
        return await _context.Courts
            .Include(c => c.Photos.OrderBy(p => p.DisplayOrder))
            .Include(c => c.Establishment)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByNameInEstablishmentAsync(string name, int establishmentId, int? excludeId = null)
    {
        var query = _context.Courts.Where(c => c.Name == name && c.EstablishmentId == establishmentId);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task AddAsync(Court court)
    {
        await _context.Courts.AddAsync(court);
    }

    public Task UpdateAsync(Court court)
    {
        _context.Courts.Update(court);
        return Task.CompletedTask;
    }

    // Photos
    public async Task AddPhotoAsync(CourtPhoto photo)
    {
        await _context.Set<CourtPhoto>().AddAsync(photo);
    }

    public async Task RemovePhotoAsync(int photoId)
    {
        var photo = await _context.Set<CourtPhoto>().FindAsync(photoId);
        if (photo != null)
            _context.Set<CourtPhoto>().Remove(photo);
    }
}
