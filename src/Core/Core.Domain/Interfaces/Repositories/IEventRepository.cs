using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetAllWithDetailsAsync();
    Task<IEnumerable<Event>> GetByUserAsync(int userId);
    Task<Event?> GetByIdAsync(int id);
    Task<Event?> GetByPublicIdAsync(Guid publicId);
    Task<Event?> GetByIdWithDetailsAsync(int id);
    Task<Event?> GetByPublicIdWithDetailsAsync(Guid publicId);
    Task<Event> CreateAsync(Event entity);
    Task UpdateAsync(Event entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByPublicIdAsync(Guid publicId);

    // Event Establishments
    Task<IEnumerable<EventEstablishment>> GetEstablishmentsByEventAsync(int eventId);
    Task<EventEstablishment> AddEstablishmentAsync(EventEstablishment entity);
    Task RemoveEstablishmentAsync(int eventId, int establishmentId);

    // Event Admins
    Task<IEnumerable<EventAdmin>> GetAdminsByEventAsync(int eventId);
    Task<EventAdmin> AddAdminAsync(EventAdmin entity);
    Task RemoveAdminAsync(int eventId, int userId);
    Task<bool> IsUserAdminOfEventAsync(int eventId, int userId);

    // Event Invitations
    Task<IEnumerable<EventInvitation>> GetPendingInvitationsAsync(int eventId);
    Task<EventInvitation?> GetInvitationByTokenAsync(string token);
    Task<EventInvitation> CreateInvitationAsync(EventInvitation entity);
    Task UpdateInvitationAsync(EventInvitation entity);
}
