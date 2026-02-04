using Core.Application.DTOs.Events;
using Core.Application.Features.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly GetMyEventsFeature _getMyEvents;
    private readonly GetEventByPublicIdFeature _getEventByPublicId;
    private readonly CreateEventFeature _createEvent;
    private readonly UpdateEventFeature _updateEvent;
    private readonly DeleteEventFeature _deleteEvent;
    private readonly UploadPosterFeature _uploadPoster;
    private readonly RemovePosterFeature _removePoster;
    private readonly UploadRulesPdfFeature _uploadRulesPdf;
    private readonly RemoveRulesPdfFeature _removeRulesPdf;

    public EventsController(
        GetMyEventsFeature getMyEvents,
        GetEventByPublicIdFeature getEventByPublicId,
        CreateEventFeature createEvent,
        UpdateEventFeature updateEvent,
        DeleteEventFeature deleteEvent,
        UploadPosterFeature uploadPoster,
        RemovePosterFeature removePoster,
        UploadRulesPdfFeature uploadRulesPdf,
        RemoveRulesPdfFeature removeRulesPdf)
    {
        _getMyEvents = getMyEvents;
        _getEventByPublicId = getEventByPublicId;
        _createEvent = createEvent;
        _updateEvent = updateEvent;
        _deleteEvent = deleteEvent;
        _uploadPoster = uploadPoster;
        _removePoster = removePoster;
        _uploadRulesPdf = uploadRulesPdf;
        _removeRulesPdf = removeRulesPdf;
    }

    /// <summary>
    /// Get events for current user (as organizer or admin)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventListDto>>> GetMyEvents()
    {
        var events = await _getMyEvents.ExecuteAsync();
        return Ok(events);
    }

    /// <summary>
    /// Get event by public ID (GUID)
    /// </summary>
    [HttpGet("{publicId:guid}")]
    public async Task<ActionResult<EventDto>> GetEvent(Guid publicId)
    {
        var (eventDto, error) = await _getEventByPublicId.ExecuteAsync(publicId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        return Ok(eventDto);
    }

    /// <summary>
    /// Create a new event (Platform Admin only)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto dto)
    {
        var (eventDto, error) = await _createEvent.ExecuteAsync(dto);
        
        if (error != null)
            return BadRequest(new { message = error });
        
        return CreatedAtAction(nameof(GetEvent), new { publicId = eventDto!.PublicId }, eventDto);
    }

    /// <summary>
    /// Update event
    /// </summary>
    [HttpPut("{publicId:guid}")]
    public async Task<IActionResult> UpdateEvent(Guid publicId, [FromBody] UpdateEventDto dto)
    {
        var (success, error) = await _updateEvent.ExecuteAsync(publicId, dto);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }

    /// <summary>
    /// Delete event (soft delete)
    /// </summary>
    [HttpDelete("{publicId:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid publicId)
    {
        var (success, error) = await _deleteEvent.ExecuteAsync(publicId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error?.Contains("Solo el organizador") == true)
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }

    /// <summary>
    /// Upload poster (vertical or horizontal)
    /// </summary>
    [HttpPost("{publicId:guid}/poster-{type}")]
    public async Task<IActionResult> UploadPoster(Guid publicId, string type, [FromBody] UploadPosterDto dto)
    {
        var (success, error) = await _uploadPoster.ExecuteAsync(publicId, type, dto);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }

    /// <summary>
    /// Remove poster
    /// </summary>
    [HttpDelete("{publicId:guid}/poster/{type}")]
    public async Task<IActionResult> RemovePoster(Guid publicId, string type)
    {
        var (success, error) = await _removePoster.ExecuteAsync(publicId, type);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }

    /// <summary>
    /// Upload rules PDF
    /// </summary>
    [HttpPost("{publicId:guid}/rules-pdf")]
    public async Task<IActionResult> UploadRulesPdf(Guid publicId, [FromBody] UploadRulesPdfDto dto)
    {
        var (success, error) = await _uploadRulesPdf.ExecuteAsync(publicId, dto);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }

    /// <summary>
    /// Remove rules PDF
    /// </summary>
    [HttpDelete("{publicId:guid}/rules-pdf")]
    public async Task<IActionResult> RemoveRulesPdf(Guid publicId)
    {
        var (success, error) = await _removeRulesPdf.ExecuteAsync(publicId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }
}
