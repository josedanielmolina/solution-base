using Core.Application.DTOs.Events;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Events;

// ============================================================================
// GET EVENT ESTABLISHMENTS
// ============================================================================
public class GetEventEstablishmentsFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetEventEstablishmentsFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(IEnumerable<EventEstablishmentDto>? Establishments, string? Error)> ExecuteAsync(Guid publicId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (null, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (null, "No tiene acceso a este evento");

        var establishments = await _eventRepository.GetEstablishmentsByEventAsync(eventEntity.Id);
        
        var dtos = establishments.Select(ee => new EventEstablishmentDto(
            ee.Id,
            ee.EstablishmentId,
            ee.Establishment?.Name ?? "N/A",
            ee.Establishment?.City?.Name,
            ee.CreatedAt
        ));

        return (dtos, null);
    }
}

// ============================================================================
// ADD ESTABLISHMENT TO EVENT
// ============================================================================
public class AddEstablishmentFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddEstablishmentFeature(
        IEventRepository eventRepository, 
        IEstablishmentRepository establishmentRepository,
        ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _establishmentRepository = establishmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(EventEstablishmentDto? Establishment, string? Error)> ExecuteAsync(Guid publicId, AddEventEstablishmentDto dto)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (null, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (null, "No tiene acceso a este evento");

        // Verify establishment exists
        var establishment = await _establishmentRepository.GetByIdAsync(dto.EstablishmentId);
        if (establishment == null)
            return (null, "El establecimiento no existe");

        // Check if already associated
        var existing = eventEntity.Establishments?.Any(ee => ee.EstablishmentId == dto.EstablishmentId);
        if (existing == true)
            return (null, "El establecimiento ya está asociado a este evento");

        var eventEstablishment = new EventEstablishment
        {
            EventId = eventEntity.Id,
            EstablishmentId = dto.EstablishmentId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _eventRepository.AddEstablishmentAsync(eventEstablishment);

        return (new EventEstablishmentDto(
            created.Id,
            created.EstablishmentId,
            establishment.Name,
            establishment.City?.Name,
            created.CreatedAt
        ), null);
    }
}

// ============================================================================
// REMOVE ESTABLISHMENT FROM EVENT
// ============================================================================
public class RemoveEstablishmentFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public RemoveEstablishmentFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId, int establishmentId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (false, "No tiene acceso a este evento");

        // Check if associated
        var existing = eventEntity.Establishments?.Any(ee => ee.EstablishmentId == establishmentId);
        if (existing != true)
            return (false, "El establecimiento no está asociado a este evento");

        await _eventRepository.RemoveEstablishmentAsync(eventEntity.Id, establishmentId);
        return (true, null);
    }
}

// ============================================================================
// SEARCH AVAILABLE ESTABLISHMENTS
// ============================================================================
public class SearchAvailableEstablishmentsFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public SearchAvailableEstablishmentsFeature(
        IEventRepository eventRepository, 
        IEstablishmentRepository establishmentRepository,
        ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _establishmentRepository = establishmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(IEnumerable<object>? Establishments, string? Error)> ExecuteAsync(Guid publicId, string? searchTerm)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (null, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (null, "No tiene acceso a este evento");

        // Get all establishments and filter out already associated ones
        var allEstablishments = await _establishmentRepository.GetAllAsync();
        var associatedIds = eventEntity.Establishments?.Select(ee => ee.EstablishmentId).ToHashSet() ?? new HashSet<int>();

        var available = allEstablishments
            .Where(e => !associatedIds.Contains(e.Id))
            .Where(e => string.IsNullOrEmpty(searchTerm) || 
                        e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .Select(e => new 
            {
                e.Id,
                e.Name,
                City = e.City?.Name,
                e.Address
            })
            .Take(20); // Limit results

        return (available, null);
    }
}
