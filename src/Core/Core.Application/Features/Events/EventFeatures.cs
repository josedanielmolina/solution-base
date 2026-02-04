using Core.Application.DTOs.Events;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Events;

// ============================================================================
// GET MY EVENTS (for current user)
// ============================================================================
public class GetMyEventsFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyEventsFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<EventListDto>> ExecuteAsync()
    {
        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        // PlatformAdmin can see all events
        IEnumerable<Event> events;
        if (isPlatformAdmin)
        {
            events = await _eventRepository.GetAllWithDetailsAsync();
        }
        else
        {
            events = await _eventRepository.GetByUserAsync(userId);
        }

        return events.Select(e => new EventListDto(
            e.Id,
            e.PublicId,
            e.Name,
            e.Description,
            e.OrganizerId,
            e.Organizer != null ? $"{e.Organizer.FirstName} {e.Organizer.LastName}" : "N/A",
            e.StartDate,
            e.EndDate,
            e.IsActive,
            e.Establishments?.Count ?? 0,
            e.Admins?.Count ?? 0,
            e.CreatedAt
        ));
    }
}

// ============================================================================
// GET EVENT BY PUBLIC ID
// ============================================================================
public class GetEventByPublicIdFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetEventByPublicIdFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(EventDto? Event, string? Error)> ExecuteAsync(Guid publicId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (null, "Evento no encontrado");

        // Check access (unless PlatformAdmin)
        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (null, "No tiene acceso a este evento");

        var dto = MapToDto(eventEntity);
        return (dto, null);
    }

    private EventDto MapToDto(Event e) => new EventDto(
        e.Id,
        e.PublicId,
        e.Name,
        e.Description,
        e.OrganizerId,
        e.Organizer != null ? $"{e.Organizer.FirstName} {e.Organizer.LastName}" : "N/A",
        e.ContactPhone,
        e.StartDate,
        e.EndDate,
        e.PosterVertical,
        e.PosterHorizontal,
        e.RulesPdf,
        e.WhatsApp,
        e.Facebook,
        e.Instagram,
        e.IsActive,
        e.CreatedAt,
        e.UpdatedAt,
        e.Establishments?.Select(ee => new EventEstablishmentDto(
            ee.Id,
            ee.EstablishmentId,
            ee.Establishment?.Name ?? "N/A",
            ee.Establishment?.City?.Name,
            ee.CreatedAt
        )) ?? Enumerable.Empty<EventEstablishmentDto>(),
        e.Admins?.Select(ea => new EventAdminDto(
            ea.Id,
            ea.UserId,
            ea.User != null ? $"{ea.User.FirstName} {ea.User.LastName}" : "N/A",
            ea.User?.Email ?? "N/A",
            ea.CreatedAt
        )) ?? Enumerable.Empty<EventAdminDto>()
    );
}

// ============================================================================
// CREATE EVENT (Platform Admin only)
// ============================================================================
public class CreateEventFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;

    public CreateEventFeature(IEventRepository eventRepository, IUserRepository userRepository)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
    }

    public async Task<(EventDto? Event, string? Error)> ExecuteAsync(CreateEventDto dto)
    {
        // Validate organizer exists (only if provided)
        User? organizer = null;
        if (dto.OrganizerId.HasValue)
        {
            organizer = await _userRepository.GetByIdAsync(dto.OrganizerId.Value);
            if (organizer == null)
                return (null, "El organizador no existe");
        }

        var eventEntity = new Event
        {
            PublicId = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            OrganizerId = dto.OrganizerId,
            ContactPhone = dto.ContactPhone,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            WhatsApp = dto.WhatsApp,
            Facebook = dto.Facebook,
            Instagram = dto.Instagram,
            IsActive = true
        };

        var created = await _eventRepository.CreateAsync(eventEntity);
        
        // Reload with details
        var reloaded = await _eventRepository.GetByIdWithDetailsAsync(created.Id);
        
        var organizerName = organizer != null 
            ? $"{organizer.FirstName} {organizer.LastName}" 
            : null;
        
        var result = new EventDto(
            reloaded!.Id,
            reloaded.PublicId,
            reloaded.Name,
            reloaded.Description,
            reloaded.OrganizerId,
            organizerName,
            reloaded.ContactPhone,
            reloaded.StartDate,
            reloaded.EndDate,
            reloaded.PosterVertical,
            reloaded.PosterHorizontal,
            reloaded.RulesPdf,
            reloaded.WhatsApp,
            reloaded.Facebook,
            reloaded.Instagram,
            reloaded.IsActive,
            reloaded.CreatedAt,
            reloaded.UpdatedAt,
            Enumerable.Empty<EventEstablishmentDto>(),
            Enumerable.Empty<EventAdminDto>()
        );

        return (result, null);
    }
}

// ============================================================================
// UPDATE EVENT
// ============================================================================
public class UpdateEventFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateEventFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId, UpdateEventDto dto)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        // Check access
        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (false, "No tiene acceso a este evento");

        eventEntity.Name = dto.Name;
        eventEntity.Description = dto.Description;
        eventEntity.OrganizerId = dto.OrganizerId;
        eventEntity.ContactPhone = dto.ContactPhone;
        eventEntity.StartDate = dto.StartDate;
        eventEntity.EndDate = dto.EndDate;
        eventEntity.WhatsApp = dto.WhatsApp;
        eventEntity.Facebook = dto.Facebook;
        eventEntity.Instagram = dto.Instagram;

        await _eventRepository.UpdateAsync(eventEntity);
        return (true, null);
    }
}

// ============================================================================
// DELETE EVENT (Soft delete)
// ============================================================================
public class DeleteEventFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteEventFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        // Only organizer or PlatformAdmin can delete
        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && eventEntity.OrganizerId != userId)
            return (false, "Solo el organizador puede eliminar el evento");

        await _eventRepository.DeleteAsync(eventEntity.Id);
        return (true, null);
    }
}

// ============================================================================
// UPLOAD POSTER
// ============================================================================
public class UploadPosterFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public UploadPosterFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId, string posterType, UploadPosterDto dto)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (false, "No tiene acceso a este evento");

        if (posterType.ToLower() == "vertical")
            eventEntity.PosterVertical = dto.ImageData;
        else if (posterType.ToLower() == "horizontal")
            eventEntity.PosterHorizontal = dto.ImageData;
        else
            return (false, "Tipo de poster inválido (use 'vertical' u 'horizontal')");

        await _eventRepository.UpdateAsync(eventEntity);
        return (true, null);
    }
}

// ============================================================================
// REMOVE POSTER
// ============================================================================
public class RemovePosterFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public RemovePosterFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId, string posterType)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (false, "No tiene acceso a este evento");

        if (posterType.ToLower() == "vertical")
            eventEntity.PosterVertical = null;
        else if (posterType.ToLower() == "horizontal")
            eventEntity.PosterHorizontal = null;
        else
            return (false, "Tipo de poster inválido");

        await _eventRepository.UpdateAsync(eventEntity);
        return (true, null);
    }
}

// ============================================================================
// UPLOAD RULES PDF
// ============================================================================
public class UploadRulesPdfFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public UploadRulesPdfFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId, UploadRulesPdfDto dto)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (false, "No tiene acceso a este evento");

        eventEntity.RulesPdf = dto.PdfData;
        await _eventRepository.UpdateAsync(eventEntity);
        return (true, null);
    }
}

// ============================================================================
// REMOVE RULES PDF
// ============================================================================
public class RemoveRulesPdfFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public RemoveRulesPdfFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (false, "No tiene acceso a este evento");

        eventEntity.RulesPdf = null;
        await _eventRepository.UpdateAsync(eventEntity);
        return (true, null);
    }
}
