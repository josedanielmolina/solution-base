using Core.Application.DTOs.Countries;
using Core.Application.Features.Countries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CountriesController : ControllerBase
{
    private readonly IGetCountries _getCountries;
    private readonly IGetCountryById _getCountryById;
    private readonly ICreateCountry _createCountry;
    private readonly IUpdateCountry _updateCountry;
    private readonly IDeleteCountry _deleteCountry;

    public CountriesController(
        IGetCountries getCountries,
        IGetCountryById getCountryById,
        ICreateCountry createCountry,
        IUpdateCountry updateCountry,
        IDeleteCountry deleteCountry)
    {
        _getCountries = getCountries;
        _getCountryById = getCountryById;
        _createCountry = createCountry;
        _updateCountry = updateCountry;
        _deleteCountry = deleteCountry;
    }

    [HttpGet]
    [HasPermission("countries.view")]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll([FromQuery] bool activeOnly = true)
    {
        var countries = await _getCountries.ExecuteAsync(activeOnly);
        return Ok(countries);
    }

    [HttpGet("{id:int}")]
    [HasPermission("countries.view")]
    public async Task<ActionResult<CountryDto>> GetById(int id)
    {
        var country = await _getCountryById.ExecuteAsync(id);
        if (country == null) return NotFound();
        return Ok(country);
    }

    [HttpPost]
    [HasPermission("countries.create")]
    public async Task<ActionResult<CountryDto>> Create([FromBody] CreateCountryDto dto)
    {
        try
        {
            var country = await _createCountry.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = country.Id }, country);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [HasPermission("countries.edit")]
    public async Task<ActionResult<CountryDto>> Update(int id, [FromBody] UpdateCountryDto dto)
    {
        try
        {
            var country = await _updateCountry.ExecuteAsync(id, dto);
            if (country == null) return NotFound();
            return Ok(country);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [HasPermission("countries.delete")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _deleteCountry.ExecuteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
