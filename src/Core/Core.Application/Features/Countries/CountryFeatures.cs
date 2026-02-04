using Core.Application.DTOs.Countries;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Countries;

public interface IGetCountries
{
    Task<IEnumerable<CountryDto>> ExecuteAsync(bool activeOnly = true);
}

public class GetCountries : IGetCountries
{
    private readonly ICountryRepository _countryRepository;

    public GetCountries(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CountryDto>> ExecuteAsync(bool activeOnly = true)
    {
        var countries = activeOnly 
            ? await _countryRepository.GetAllActiveAsync()
            : await _countryRepository.GetAllAsync();

        return countries.Select(c => c.ToDto());
    }
}

public interface IGetCountryById
{
    Task<CountryDto?> ExecuteAsync(int id);
}

public class GetCountryById : IGetCountryById
{
    private readonly ICountryRepository _countryRepository;

    public GetCountryById(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<CountryDto?> ExecuteAsync(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id);
        return country?.ToDto();
    }
}

public interface ICreateCountry
{
    Task<CountryDto> ExecuteAsync(CreateCountryDto dto);
}

public class CreateCountry : ICreateCountry
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCountry(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CountryDto> ExecuteAsync(CreateCountryDto dto)
    {
        // Check for duplicates
        if (await _countryRepository.ExistsByNameAsync(dto.Name))
            throw new InvalidOperationException("Ya existe un país con ese nombre");

        if (await _countryRepository.ExistsByCodeAsync(dto.Code))
            throw new InvalidOperationException("Ya existe un país con ese código");

        var country = new Country
        {
            Name = dto.Name,
            Code = dto.Code.ToUpper(),
            IsActive = true
        };

        await _countryRepository.AddAsync(country);
        await _unitOfWork.SaveChangesAsync();

        return country.ToDto();
    }
}

public interface IUpdateCountry
{
    Task<CountryDto?> ExecuteAsync(int id, UpdateCountryDto dto);
}

public class UpdateCountry : IUpdateCountry
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCountry(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CountryDto?> ExecuteAsync(int id, UpdateCountryDto dto)
    {
        var country = await _countryRepository.GetByIdAsync(id);
        if (country == null) return null;

        // Check for duplicates (excluding current)
        var existingByName = await _countryRepository.GetByNameAsync(dto.Name);
        if (existingByName != null && existingByName.Id != id)
            throw new InvalidOperationException("Ya existe un país con ese nombre");

        var existingByCode = await _countryRepository.GetByCodeAsync(dto.Code);
        if (existingByCode != null && existingByCode.Id != id)
            throw new InvalidOperationException("Ya existe un país con ese código");

        country.Name = dto.Name;
        country.Code = dto.Code.ToUpper();
        country.IsActive = dto.IsActive;

        await _countryRepository.UpdateAsync(country);
        await _unitOfWork.SaveChangesAsync();

        return country.ToDto();
    }
}

public interface IDeleteCountry
{
    Task<bool> ExecuteAsync(int id);
}

public class DeleteCountry : IDeleteCountry
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCountry(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id);
        if (country == null) return false;

        // Soft delete - just deactivate
        country.IsActive = false;
        await _countryRepository.UpdateAsync(country);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

// Mapping extension
public static class CountryMappingExtensions
{
    public static CountryDto ToDto(this Country country) => new(
        country.Id,
        country.Name,
        country.Code,
        country.IsActive,
        country.CreatedAt,
        country.UpdatedAt
    );
}
