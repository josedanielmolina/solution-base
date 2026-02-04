using Core.Application.DTOs.Events;
using Core.Application.Features.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/events/{publicId:guid}/admins")]
[Authorize]
public class EventAdminsController : ControllerBase
{
    private readonly GetEventAdminsFeature _getEventAdmins;
    private readonly InviteAdminFeature _inviteAdmin;
    private readonly RemoveAdminFeature _removeAdmin;
    private readonly GetPendingInvitationsFeature _getPendingInvitations;

    public EventAdminsController(
        GetEventAdminsFeature getEventAdmins,
        InviteAdminFeature inviteAdmin,
        RemoveAdminFeature removeAdmin,
        GetPendingInvitationsFeature getPendingInvitations)
    {
        _getEventAdmins = getEventAdmins;
        _inviteAdmin = inviteAdmin;
        _removeAdmin = removeAdmin;
        _getPendingInvitations = getPendingInvitations;
    }

    /// <summary>
    /// Get all admins for an event
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventAdminDto>>> GetAdmins(Guid publicId)
    {
        var (admins, error) = await _getEventAdmins.ExecuteAsync(publicId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        return Ok(admins);
    }

    /// <summary>
    /// Invite a new admin by email
    /// </summary>
    [HttpPost("invite")]
    public async Task<ActionResult<EventInvitationDto>> InviteAdmin(Guid publicId, [FromBody] InviteAdminDto dto)
    {
        var (invitation, error) = await _inviteAdmin.ExecuteAsync(publicId, dto);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error?.Contains("Solo el organizador") == true)
            return Forbid();
        
        if (error != null)
            return BadRequest(new { message = error });
        
        return Ok(invitation);
    }

    /// <summary>
    /// Remove an admin from event
    /// </summary>
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> RemoveAdmin(Guid publicId, int userId)
    {
        var (success, error) = await _removeAdmin.ExecuteAsync(publicId, userId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error?.Contains("Solo el organizador") == true)
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }

    /// <summary>
    /// Get pending invitations for an event
    /// </summary>
    [HttpGet("~/api/events/{publicId:guid}/invitations")]
    public async Task<ActionResult<IEnumerable<EventInvitationDto>>> GetPendingInvitations(Guid publicId)
    {
        var (invitations, error) = await _getPendingInvitations.ExecuteAsync(publicId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error?.Contains("Solo el organizador") == true)
            return Forbid();
        
        if (error != null)
            return BadRequest(new { message = error });
        
        return Ok(invitations);
    }
}

/// <summary>
/// Separate controller for invitation acceptance (token-based, no event ID in URL)
/// </summary>
[ApiController]
[Route("api/invitations")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly AcceptInvitationFeature _acceptInvitation;

    public InvitationsController(AcceptInvitationFeature acceptInvitation)
    {
        _acceptInvitation = acceptInvitation;
    }

    /// <summary>
    /// Accept an invitation by token
    /// </summary>
    [HttpPost("{token}/accept")]
    public async Task<IActionResult> AcceptInvitation(string token)
    {
        var (success, error) = await _acceptInvitation.ExecuteAsync(token);
        
        if (error == "Invitación no encontrada")
            return NotFound(new { message = error });
        
        if (!success)
            return BadRequest(new { message = error });
        
        return Ok(new { message = "Invitación aceptada exitosamente" });
    }
}
