using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface ICourtRepository
{
    Task<IEnumerable<Court>> GetByEstablishmentAsync(int establishmentId, bool activeOnly = true);
    Task<Court?> GetByIdAsync(int id);
    Task<Court?> GetByIdWithPhotosAsync(int id);
    Task<bool> ExistsByNameInEstablishmentAsync(string name, int establishmentId, int? excludeId = null);
    Task AddAsync(Court court);
    Task UpdateAsync(Court court);
    
    // Photos
    Task AddPhotoAsync(CourtPhoto photo);
    Task RemovePhotoAsync(int photoId);
}
