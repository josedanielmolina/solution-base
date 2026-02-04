using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events
            .Where(e => e.IsActive)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetAllWithDetailsAsync()
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Establishments)
            .Include(e => e.Admins)
            .Where(e => e.IsActive)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetByUserAsync(int userId)
    {
        return await _context.Events
            .Where(e => e.IsActive && 
                (e.OrganizerId == userId || e.Admins.Any(a => a.UserId == userId)))
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
    }

    public async Task<Event?> GetByPublicIdAsync(Guid publicId)
    {
        return await _context.Events
            .FirstOrDefaultAsync(e => e.PublicId == publicId && e.IsActive);
    }

    public async Task<Event?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Establishments).ThenInclude(ee => ee.Establishment)
            .Include(e => e.Admins).ThenInclude(ea => ea.User)
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
    }

    public async Task<Event?> GetByPublicIdWithDetailsAsync(Guid publicId)
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Establishments).ThenInclude(ee => ee.Establishment)
            .Include(e => e.Admins).ThenInclude(ea => ea.User)
            .FirstOrDefaultAsync(e => e.PublicId == publicId && e.IsActive);
    }

    public async Task<Event> CreateAsync(Event entity)
    {
        entity.PublicId = Guid.NewGuid();
        _context.Events.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Event entity)
    {
        _context.Events.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Events.FindAsync(id);
        if (entity != null)
        {
            entity.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id && e.IsActive);
    }

    public async Task<bool> ExistsByPublicIdAsync(Guid publicId)
    {
        return await _context.Events.AnyAsync(e => e.PublicId == publicId && e.IsActive);
    }

    // Event Establishments
    public async Task<IEnumerable<EventEstablishment>> GetEstablishmentsByEventAsync(int eventId)
    {
        return await _context.EventEstablishments
            .Include(ee => ee.Establishment)
            .Where(ee => ee.EventId == eventId)
            .ToListAsync();
    }

    public async Task<EventEstablishment> AddEstablishmentAsync(EventEstablishment entity)
    {
        _context.EventEstablishments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task RemoveEstablishmentAsync(int eventId, int establishmentId)
    {
        var entity = await _context.EventEstablishments
            .FirstOrDefaultAsync(ee => ee.EventId == eventId && ee.EstablishmentId == establishmentId);
        if (entity != null)
        {
            _context.EventEstablishments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    // Event Admins
    public async Task<IEnumerable<EventAdmin>> GetAdminsByEventAsync(int eventId)
    {
        return await _context.EventAdmins
            .Include(ea => ea.User)
            .Where(ea => ea.EventId == eventId)
            .ToListAsync();
    }

    public async Task<EventAdmin> AddAdminAsync(EventAdmin entity)
    {
        _context.EventAdmins.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task RemoveAdminAsync(int eventId, int userId)
    {
        var entity = await _context.EventAdmins
            .FirstOrDefaultAsync(ea => ea.EventId == eventId && ea.UserId == userId);
        if (entity != null)
        {
            _context.EventAdmins.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsUserAdminOfEventAsync(int eventId, int userId)
    {
        return await _context.EventAdmins
            .AnyAsync(ea => ea.EventId == eventId && ea.UserId == userId);
    }

    // Event Invitations
    public async Task<IEnumerable<EventInvitation>> GetPendingInvitationsAsync(int eventId)
    {
        return await _context.EventInvitations
            .Where(ei => ei.EventId == eventId && ei.AcceptedAt == null && ei.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<EventInvitation?> GetInvitationByTokenAsync(string token)
    {
        return await _context.EventInvitations
            .Include(ei => ei.Event)
            .FirstOrDefaultAsync(ei => ei.Token == token);
    }

    public async Task<EventInvitation> CreateInvitationAsync(EventInvitation entity)
    {
        _context.EventInvitations.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateInvitationAsync(EventInvitation entity)
    {
        _context.EventInvitations.Update(entity);
        await _context.SaveChangesAsync();
    }
}
