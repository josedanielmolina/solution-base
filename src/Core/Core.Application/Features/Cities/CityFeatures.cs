using Core.Application.DTOs.Cities;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Cities;

public interface IGetCities
{
    Task<IEnumerable<CityDto>> ExecuteAsync(bool activeOnly = true);
}

public class GetCities : IGetCities
{
    private readonly ICityRepository _cityRepository;

    public GetCities(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<CityDto>> ExecuteAsync(bool activeOnly = true)
    {
        var cities = activeOnly 
            ? await _cityRepository.GetAllActiveAsync()
            : await _cityRepository.GetAllAsync();

        return cities.Select(c => c.ToDto());
    }
}

public interface IGetCitiesByCountry
{
    Task<IEnumerable<CityDto>> ExecuteAsync(int countryId);
}

public class GetCitiesByCountry : IGetCitiesByCountry
{
    private readonly ICityRepository _cityRepository;

    public GetCitiesByCountry(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<CityDto>> ExecuteAsync(int countryId)
    {
        var cities = await _cityRepository.GetByCountryAsync(countryId);
        return cities.Select(c => c.ToDto());
    }
}

public interface IGetCityById
{
    Task<CityDto?> ExecuteAsync(int id);
}

public class GetCityById : IGetCityById
{
    private readonly ICityRepository _cityRepository;

    public GetCityById(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<CityDto?> ExecuteAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        return city?.ToDto();
    }
}

public interface ICreateCity
{
    Task<CityDto> ExecuteAsync(CreateCityDto dto);
}

public class CreateCity : ICreateCity
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCity(ICityRepository cityRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CityDto> ExecuteAsync(CreateCityDto dto)
    {
        // Validate country exists
        if (!await _countryRepository.ExistsAsync(dto.CountryId))
            throw new InvalidOperationException("El país no existe");

        // Check for duplicates within country
        if (await _cityRepository.ExistsByNameInCountryAsync(dto.Name, dto.CountryId))
            throw new InvalidOperationException("Ya existe una ciudad con ese nombre en el país seleccionado");

        var city = new City
        {
            Name = dto.Name,
            CountryId = dto.CountryId,
            IsActive = true
        };

        await _cityRepository.AddAsync(city);
        await _unitOfWork.SaveChangesAsync();

        // Reload to get country name
        var created = await _cityRepository.GetByIdAsync(city.Id);
        return created!.ToDto();
    }
}

public interface IUpdateCity
{
    Task<CityDto?> ExecuteAsync(int id, UpdateCityDto dto);
}

public class UpdateCity : IUpdateCity
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCity(ICityRepository cityRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CityDto?> ExecuteAsync(int id, UpdateCityDto dto)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) return null;

        // Validate country exists
        if (!await _countryRepository.ExistsAsync(dto.CountryId))
            throw new InvalidOperationException("El país no existe");

        // Check for duplicates (excluding current)
        var existing = await _cityRepository.GetByNameInCountryAsync(dto.Name, dto.CountryId);
        if (existing != null && existing.Id != id)
            throw new InvalidOperationException("Ya existe una ciudad con ese nombre en el país seleccionado");

        city.Name = dto.Name;
        city.CountryId = dto.CountryId;
        city.IsActive = dto.IsActive;

        await _cityRepository.UpdateAsync(city);
        await _unitOfWork.SaveChangesAsync();

        // Reload to get country name
        var updated = await _cityRepository.GetByIdAsync(id);
        return updated!.ToDto();
    }
}

public interface IDeleteCity
{
    Task<bool> ExecuteAsync(int id);
}

public class DeleteCity : IDeleteCity
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCity(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) return false;

        city.IsActive = false;
        await _cityRepository.UpdateAsync(city);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

// Mapping extension
public static class CityMappingExtensions
{
    public static CityDto ToDto(this City city) => new(
        city.Id,
        city.Name,
        city.CountryId,
        city.Country?.Name ?? "",
        city.IsActive,
        city.CreatedAt,
        city.UpdatedAt
    );
}
