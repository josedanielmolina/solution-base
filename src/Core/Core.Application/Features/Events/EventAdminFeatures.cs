using Core.Application.DTOs.Events;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Events;

// ============================================================================
// GET EVENT ADMINS
// ============================================================================
public class GetEventAdminsFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetEventAdminsFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(IEnumerable<EventAdminDto>? Admins, string? Error)> ExecuteAsync(Guid publicId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (null, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && !eventEntity.HasAccess(userId))
            return (null, "No tiene acceso a este evento");

        var admins = await _eventRepository.GetAdminsByEventAsync(eventEntity.Id);
        
        var dtos = admins.Select(a => new EventAdminDto(
            a.Id,
            a.UserId,
            a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : "N/A",
            a.User?.Email ?? "N/A",
            a.CreatedAt
        ));

        return (dtos, null);
    }
}

// ============================================================================
// INVITE ADMIN
// ============================================================================
public class InviteAdminFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public InviteAdminFeature(
        IEventRepository eventRepository, 
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(EventInvitationDto? Invitation, string? Error)> ExecuteAsync(Guid publicId, InviteAdminDto dto)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (null, "Evento no encontrado");

        // Only organizer can invite admins
        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && eventEntity.OrganizerId != userId)
            return (null, "Solo el organizador puede invitar administradores");

        // Check if user with this email already exists and is already admin
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            var isAlreadyAdmin = await _eventRepository.IsUserAdminOfEventAsync(eventEntity.Id, existingUser.Id);
            if (isAlreadyAdmin)
                return (null, "El usuario ya es administrador de este evento");
            
            // Check if is organizer
            if (eventEntity.OrganizerId == existingUser.Id)
                return (null, "El organizador no puede ser agregado como administrador");
        }

        // Check for pending invitations
        var pendingInvitations = await _eventRepository.GetPendingInvitationsAsync(eventEntity.Id);
        if (pendingInvitations.Any(i => i.Email.ToLower() == dto.Email.ToLower()))
            return (null, "Ya existe una invitación pendiente para este email");

        // Create invitation
        var invitation = new EventInvitation
        {
            EventId = eventEntity.Id,
            Email = dto.Email,
            Token = Guid.NewGuid().ToString("N"), // 32 char token
            ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _eventRepository.CreateInvitationAsync(invitation);

        // TODO: Send email with invitation link

        return (new EventInvitationDto(
            created.Id,
            created.Email,
            created.Token,
            created.ExpiresAt,
            created.AcceptedAt,
            created.CreatedAt,
            created.IsExpired,
            created.IsAccepted
        ), null);
    }
}

// ============================================================================
// REMOVE ADMIN
// ============================================================================
public class RemoveAdminFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public RemoveAdminFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(Guid publicId, int targetUserId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (false, "Evento no encontrado");

        // Only organizer can remove admins
        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        if (!isPlatformAdmin && eventEntity.OrganizerId != userId)
            return (false, "Solo el organizador puede remover administradores");

        // Check if user is actually admin
        var isAdmin = await _eventRepository.IsUserAdminOfEventAsync(eventEntity.Id, targetUserId);
        if (!isAdmin)
            return (false, "El usuario no es administrador de este evento");

        await _eventRepository.RemoveAdminAsync(eventEntity.Id, targetUserId);
        
        // TODO: Send notification to removed admin

        return (true, null);
    }
}

// ============================================================================
// ACCEPT INVITATION
// ============================================================================
public class AcceptInvitationFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public AcceptInvitationFeature(
        IEventRepository eventRepository, 
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(bool Success, string? Error)> ExecuteAsync(string token)
    {
        var invitation = await _eventRepository.GetInvitationByTokenAsync(token);
        
        if (invitation == null)
            return (false, "Invitación no encontrada");

        if (invitation.IsExpired)
            return (false, "La invitación ha expirado");

        if (invitation.IsAccepted)
            return (false, "La invitación ya fue aceptada");

        // Get current user
        var currentUserId = _currentUserService.UserId;
        var currentUser = await _userRepository.GetByIdAsync(currentUserId);
        
        if (currentUser == null)
            return (false, "Usuario no encontrado");

        // Verify email matches
        if (currentUser.Email.ToLower() != invitation.Email.ToLower())
            return (false, "Esta invitación no es para su cuenta");

        // Add user as admin
        var admin = new EventAdmin
        {
            EventId = invitation.EventId,
            UserId = currentUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _eventRepository.AddAdminAsync(admin);

        // Mark invitation as accepted
        invitation.AcceptedAt = DateTime.UtcNow;
        await _eventRepository.UpdateInvitationAsync(invitation);

        return (true, null);
    }
}

// ============================================================================
// GET PENDING INVITATIONS
// ============================================================================
public class GetPendingInvitationsFeature
{
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetPendingInvitationsFeature(IEventRepository eventRepository, ICurrentUserService currentUserService)
    {
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<(IEnumerable<EventInvitationDto>? Invitations, string? Error)> ExecuteAsync(Guid publicId)
    {
        var eventEntity = await _eventRepository.GetByPublicIdWithDetailsAsync(publicId);
        
        if (eventEntity == null)
            return (null, "Evento no encontrado");

        var userId = _currentUserService.UserId;
        var isPlatformAdmin = _currentUserService.Roles?.Contains("PlatformAdmin") ?? false;
        
        // Only organizer can see pending invitations
        if (!isPlatformAdmin && eventEntity.OrganizerId != userId)
            return (null, "Solo el organizador puede ver las invitaciones pendientes");

        var invitations = await _eventRepository.GetPendingInvitationsAsync(eventEntity.Id);
        
        var dtos = invitations.Select(i => new EventInvitationDto(
            i.Id,
            i.Email,
            i.Token,
            i.ExpiresAt,
            i.AcceptedAt,
            i.CreatedAt,
            i.IsExpired,
            i.IsAccepted
        ));

        return (dtos, null);
    }
}
