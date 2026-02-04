using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface IEstablishmentRepository
{
    Task<IEnumerable<Establishment>> GetAllAsync(bool activeOnly = true);
    Task<Establishment?> GetByIdAsync(int id);
    Task<Establishment?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Establishment>> GetByCityAsync(int cityId);
    Task<IEnumerable<Establishment>> SearchByNameAsync(string searchTerm);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task AddAsync(Establishment establishment);
    Task UpdateAsync(Establishment establishment);
    
    // Photos
    Task AddPhotoAsync(EstablishmentPhoto photo);
    Task RemovePhotoAsync(int photoId);
    
    // Schedules
    Task SetSchedulesAsync(int establishmentId, IEnumerable<EstablishmentSchedule> schedules);
    Task<IEnumerable<EstablishmentSchedule>> GetSchedulesAsync(int establishmentId);
}
