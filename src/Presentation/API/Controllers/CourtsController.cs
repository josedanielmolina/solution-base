using API.Authorization;
using Core.Application.DTOs.Establishments;
using Core.Application.Features.Establishments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/establishments/{establishmentId}/[controller]")]
[Authorize]
public class CourtsController : ControllerBase
{
    private readonly IGetCourtsByEstablishment _getCourtsByEstablishment;
    private readonly IGetCourtById _getCourtById;
    private readonly ICreateCourt _createCourt;
    private readonly IUpdateCourt _updateCourt;
    private readonly IDeleteCourt _deleteCourt;
    private readonly IAddCourtPhoto _addCourtPhoto;
    private readonly IRemoveCourtPhoto _removeCourtPhoto;

    public CourtsController(
        IGetCourtsByEstablishment getCourtsByEstablishment,
        IGetCourtById getCourtById,
        ICreateCourt createCourt,
        IUpdateCourt updateCourt,
        IDeleteCourt deleteCourt,
        IAddCourtPhoto addCourtPhoto,
        IRemoveCourtPhoto removeCourtPhoto)
    {
        _getCourtsByEstablishment = getCourtsByEstablishment;
        _getCourtById = getCourtById;
        _createCourt = createCourt;
        _updateCourt = updateCourt;
        _deleteCourt = deleteCourt;
        _addCourtPhoto = addCourtPhoto;
        _removeCourtPhoto = removeCourtPhoto;
    }

    [HttpGet]
    [HasPermission("establishments.view")]
    public async Task<IActionResult> GetAll(int establishmentId, [FromQuery] bool activeOnly = true)
    {
        var courts = await _getCourtsByEstablishment.ExecuteAsync(establishmentId, activeOnly);
        return Ok(courts);
    }

    [HttpGet("{id}")]
    [HasPermission("establishments.view")]
    public async Task<IActionResult> GetById(int establishmentId, int id)
    {
        var court = await _getCourtById.ExecuteAsync(id);
        if (court == null || court.EstablishmentId != establishmentId)
            return NotFound();
        return Ok(court);
    }

    [HttpPost]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> Create(int establishmentId, [FromBody] CreateCourtDto dto)
    {
        try
        {
            var court = await _createCourt.ExecuteAsync(establishmentId, dto);
            return CreatedAtAction(nameof(GetById), new { establishmentId, id = court.Id }, court);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> Update(int establishmentId, int id, [FromBody] UpdateCourtDto dto)
    {
        try
        {
            var court = await _updateCourt.ExecuteAsync(establishmentId, id, dto);
            if (court == null)
                return NotFound();
            return Ok(court);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [HasPermission("establishments.delete")]
    public async Task<IActionResult> Delete(int establishmentId, int id)
    {
        var result = await _deleteCourt.ExecuteAsync(establishmentId, id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    // === PHOTOS ===

    [HttpPost("{id}/photos")]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> AddPhoto(int establishmentId, int id, [FromBody] CreatePhotoDto dto)
    {
        try
        {
            var photo = await _addCourtPhoto.ExecuteAsync(id, dto);
            return Ok(photo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}/photos/{photoId}")]
    [HasPermission("establishments.edit")]
    public async Task<IActionResult> RemovePhoto(int establishmentId, int id, int photoId)
    {
        var result = await _removeCourtPhoto.ExecuteAsync(id, photoId);
        return NoContent();
    }
}
