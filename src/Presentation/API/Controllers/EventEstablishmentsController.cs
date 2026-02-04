using Core.Application.DTOs.Events;
using Core.Application.Features.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/events/{publicId:guid}/establishments")]
[Authorize]
public class EventEstablishmentsController : ControllerBase
{
    private readonly GetEventEstablishmentsFeature _getEventEstablishments;
    private readonly AddEstablishmentFeature _addEstablishment;
    private readonly RemoveEstablishmentFeature _removeEstablishment;
    private readonly SearchAvailableEstablishmentsFeature _searchAvailable;

    public EventEstablishmentsController(
        GetEventEstablishmentsFeature getEventEstablishments,
        AddEstablishmentFeature addEstablishment,
        RemoveEstablishmentFeature removeEstablishment,
        SearchAvailableEstablishmentsFeature searchAvailable)
    {
        _getEventEstablishments = getEventEstablishments;
        _addEstablishment = addEstablishment;
        _removeEstablishment = removeEstablishment;
        _searchAvailable = searchAvailable;
    }

    /// <summary>
    /// Get all establishments associated with an event
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventEstablishmentDto>>> GetEstablishments(Guid publicId)
    {
        var (establishments, error) = await _getEventEstablishments.ExecuteAsync(publicId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        return Ok(establishments);
    }

    /// <summary>
    /// Add an establishment to an event
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EventEstablishmentDto>> AddEstablishment(Guid publicId, [FromBody] AddEventEstablishmentDto dto)
    {
        var (establishment, error) = await _addEstablishment.ExecuteAsync(publicId, dto);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        if (error != null)
            return BadRequest(new { message = error });
        
        return Ok(establishment);
    }

    /// <summary>
    /// Remove an establishment from an event
    /// </summary>
    [HttpDelete("{establishmentId:int}")]
    public async Task<IActionResult> RemoveEstablishment(Guid publicId, int establishmentId)
    {
        var (success, error) = await _removeEstablishment.ExecuteAsync(publicId, establishmentId);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        if (!success)
            return BadRequest(new { message = error });
        
        return NoContent();
    }

    /// <summary>
    /// Search available establishments (not yet associated with event)
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> SearchAvailable(Guid publicId, [FromQuery] string? q)
    {
        var (establishments, error) = await _searchAvailable.ExecuteAsync(publicId, q);
        
        if (error == "Evento no encontrado")
            return NotFound(new { message = error });
        
        if (error == "No tiene acceso a este evento")
            return Forbid();
        
        return Ok(establishments);
    }
}
