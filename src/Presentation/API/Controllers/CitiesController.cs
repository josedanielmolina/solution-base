using Core.Application.DTOs.Cities;
using Core.Application.Features.Cities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CitiesController : ControllerBase
{
    private readonly IGetCities _getCities;
    private readonly IGetCitiesByCountry _getCitiesByCountry;
    private readonly IGetCityById _getCityById;
    private readonly ICreateCity _createCity;
    private readonly IUpdateCity _updateCity;
    private readonly IDeleteCity _deleteCity;

    public CitiesController(
        IGetCities getCities,
        IGetCitiesByCountry getCitiesByCountry,
        IGetCityById getCityById,
        ICreateCity createCity,
        IUpdateCity updateCity,
        IDeleteCity deleteCity)
    {
        _getCities = getCities;
        _getCitiesByCountry = getCitiesByCountry;
        _getCityById = getCityById;
        _createCity = createCity;
        _updateCity = updateCity;
        _deleteCity = deleteCity;
    }

    [HttpGet]
    [HasPermission("cities.view")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetAll([FromQuery] bool activeOnly = true)
    {
        var cities = await _getCities.ExecuteAsync(activeOnly);
        return Ok(cities);
    }

    [HttpGet("by-country/{countryId:int}")]
    [HasPermission("cities.view")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetByCountry(int countryId)
    {
        var cities = await _getCitiesByCountry.ExecuteAsync(countryId);
        return Ok(cities);
    }

    [HttpGet("{id:int}")]
    [HasPermission("cities.view")]
    public async Task<ActionResult<CityDto>> GetById(int id)
    {
        var city = await _getCityById.ExecuteAsync(id);
        if (city == null) return NotFound();
        return Ok(city);
    }

    [HttpPost]
    [HasPermission("cities.create")]
    public async Task<ActionResult<CityDto>> Create([FromBody] CreateCityDto dto)
    {
        try
        {
            var city = await _createCity.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = city.Id }, city);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [HasPermission("cities.edit")]
    public async Task<ActionResult<CityDto>> Update(int id, [FromBody] UpdateCityDto dto)
    {
        try
        {
            var city = await _updateCity.ExecuteAsync(id, dto);
            if (city == null) return NotFound();
            return Ok(city);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [HasPermission("cities.delete")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _deleteCity.ExecuteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
