using API.Authorization;
using Core.Application.DTOs.Establishments;
using Core.Application.Features.Establishments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EstablishmentsController : ControllerBase
{
    private readonly IGetEstablishments _getEstablishments;
    private readonly IGetEstablishmentById _getEstablishmentById;
    private readonly ISearchEstablishments _searchEstablishments;
    private readonly ICreateEstablishment _createEstablishment;
    private readonly IUpdateEstablishment _updateEstablishment;
    private readonly IDeleteEstablishment _deleteEstablishment;
    private readonly IAddEstablishmentPhoto _addEstablishmentPhoto;
    private readonly IRemoveEstablishmentPhoto _removeEstablishmentPhoto;
    private readonly ISetEstablishmentSchedules _setEstablishmentSchedules;

    public EstablishmentsController(
        IGetEstablishments getEstablishments,
        IGetEstablishmentById getEstablishmentById,
        ISearchEstablishments searchEstablishments,
        ICreateEstablishment createEstablishment,
        IUpdateEstablishment updateEstablishment,
        IDeleteEstablishment deleteEstablishment,
        IAddEstablishmentPhoto addEstablishmentPhoto,
        IRemoveEstablishmentPhoto removeEstablishmentPhoto,
        ISetEstablishmentSchedules setEstablishmentSchedules)
    {
        _getEstablishments = getEstablishments;
        _getEstablishmentById = getEstablishmentById;
        _searchEstablishments = searchEstablishments;
        _createEstablishment = createEstablishment;
        _updateEstablishment = updateEstablishment;
        _deleteEstablishment = deleteEstablishment;
        _addEstablishmentPhoto = addEstablishmentPhoto;
        _removeEstablishmentPhoto = removeEstablishmentPhoto;
        _setEstablishmentSchedules = setEstablishmentSchedules;
    }

    [HttpGet]
    [HasPermission("establishments.view")]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = true)
    {
        var establishments = await _getEstablishments.ExecuteAsync(activeOnly);
        return Ok(establishments);
    }

    [HttpGet("{id}")]
    [HasPermission("establishments.view")]
    public async Task<IActionResult> GetById(int id)
    {
        var establishment = await _getEstablishmentById.ExecuteAsync(id);
        if (establishment == null)
            return NotFound();
        return Ok(establishment);
    }

    [HttpGet("search")]
    [HasPermission("establishments.view")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Ok(Array.Empty<EstablishmentListDto>());

        var establishments = await _searchEstablishments.ExecuteAsync(q);
        return Ok(establishments);
    }

    [HttpPost]
    [HasPermission("establishments.create")]
    public async Task<IActionResult> Create([FromBody] CreateEstablishmentDto dto)
    {
        try
        {
            var establishment = await _createEstablishment.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = establishment.Id }, establishment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEstablishmentDto dto)
    {
        try
        {
            var establishment = await _updateEstablishment.ExecuteAsync(id, dto);
            if (establishment == null)
                return NotFound();
            return Ok(establishment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [HasPermission("establishments.delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteEstablishment.ExecuteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    // === PHOTOS ===

    [HttpPost("{id}/photos")]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> AddPhoto(int id, [FromBody] CreatePhotoDto dto)
    {
        try
        {
            var photo = await _addEstablishmentPhoto.ExecuteAsync(id, dto);
            return Ok(photo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}/photos/{photoId}")]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> RemovePhoto(int id, int photoId)
    {
        var result = await _removeEstablishmentPhoto.ExecuteAsync(id, photoId);
        return NoContent();
    }

    // === SCHEDULES ===

    [HttpPut("{id}/schedules")]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> SetSchedules(int id, [FromBody] SetSchedulesDto dto)
    {
        try
        {
            var schedules = await _setEstablishmentSchedules.ExecuteAsync(id, dto);
            return Ok(schedules);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
