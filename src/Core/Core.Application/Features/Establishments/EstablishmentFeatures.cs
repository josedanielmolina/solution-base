using Core.Application.DTOs.Establishments;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Establishments;

// === GET ALL ===
public interface IGetEstablishments
{
    Task<IEnumerable<EstablishmentListDto>> ExecuteAsync(bool activeOnly = true);
}

public class GetEstablishments : IGetEstablishments
{
    private readonly IEstablishmentRepository _repository;

    public GetEstablishments(IEstablishmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EstablishmentListDto>> ExecuteAsync(bool activeOnly = true)
    {
        var establishments = await _repository.GetAllAsync(activeOnly);
        return establishments.Select(e => e.ToListDto());
    }
}

// === GET BY ID ===
public interface IGetEstablishmentById
{
    Task<EstablishmentDto?> ExecuteAsync(int id);
}

public class GetEstablishmentById : IGetEstablishmentById
{
    private readonly IEstablishmentRepository _repository;

    public GetEstablishmentById(IEstablishmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<EstablishmentDto?> ExecuteAsync(int id)
    {
        var establishment = await _repository.GetByIdWithDetailsAsync(id);
        return establishment?.ToDto();
    }
}

// === SEARCH ===
public interface ISearchEstablishments
{
    Task<IEnumerable<EstablishmentListDto>> ExecuteAsync(string searchTerm);
}

public class SearchEstablishments : ISearchEstablishments
{
    private readonly IEstablishmentRepository _repository;

    public SearchEstablishments(IEstablishmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EstablishmentListDto>> ExecuteAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Enumerable.Empty<EstablishmentListDto>();

        var establishments = await _repository.SearchByNameAsync(searchTerm);
        return establishments.Select(e => e.ToListDto());
    }
}

// === CREATE ===
public interface ICreateEstablishment
{
    Task<EstablishmentDto> ExecuteAsync(CreateEstablishmentDto dto);
}

public class CreateEstablishment : ICreateEstablishment
{
    private readonly IEstablishmentRepository _repository;
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEstablishment(
        IEstablishmentRepository repository,
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EstablishmentDto> ExecuteAsync(CreateEstablishmentDto dto)
    {
        // Validate city exists
        var city = await _cityRepository.GetByIdAsync(dto.CityId);
        if (city == null)
            throw new InvalidOperationException("La ciudad no existe");

        // Validate name is unique
        if (await _repository.ExistsByNameAsync(dto.Name))
            throw new InvalidOperationException("Ya existe un establecimiento con ese nombre");

        var establishment = new Establishment
        {
            Name = dto.Name,
            CountryId = dto.CountryId,
            CityId = dto.CityId,
            Address = dto.Address,
            GoogleMapsUrl = dto.GoogleMapsUrl,
            PhoneLandline = dto.PhoneLandline,
            PhoneMobile = dto.PhoneMobile,
            Logo = dto.Logo,
            ScheduleType = (ScheduleType)dto.ScheduleType,
            IsActive = true
        };

        await _repository.AddAsync(establishment);
        await _unitOfWork.SaveChangesAsync();

        // Reload with details
        var created = await _repository.GetByIdWithDetailsAsync(establishment.Id);
        return created!.ToDto();
    }
}

// === UPDATE ===
public interface IUpdateEstablishment
{
    Task<EstablishmentDto?> ExecuteAsync(int id, UpdateEstablishmentDto dto);
}

public class UpdateEstablishment : IUpdateEstablishment
{
    private readonly IEstablishmentRepository _repository;
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEstablishment(
        IEstablishmentRepository repository,
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EstablishmentDto?> ExecuteAsync(int id, UpdateEstablishmentDto dto)
    {
        var establishment = await _repository.GetByIdAsync(id);
        if (establishment == null) return null;

        // Validate city exists
        var city = await _cityRepository.GetByIdAsync(dto.CityId);
        if (city == null)
            throw new InvalidOperationException("La ciudad no existe");

        // Validate name is unique (excluding current)
        if (await _repository.ExistsByNameAsync(dto.Name, id))
            throw new InvalidOperationException("Ya existe otro establecimiento con ese nombre");

        establishment.Name = dto.Name;
        establishment.CountryId = dto.CountryId;
        establishment.CityId = dto.CityId;
        establishment.Address = dto.Address;
        establishment.GoogleMapsUrl = dto.GoogleMapsUrl;
        establishment.PhoneLandline = dto.PhoneLandline;
        establishment.PhoneMobile = dto.PhoneMobile;
        establishment.Logo = dto.Logo;
        establishment.ScheduleType = (ScheduleType)dto.ScheduleType;
        establishment.IsActive = dto.IsActive;

        await _repository.UpdateAsync(establishment);
        await _unitOfWork.SaveChangesAsync();

        // Reload with details
        var updated = await _repository.GetByIdWithDetailsAsync(id);
        return updated!.ToDto();
    }
}

// === DELETE (Soft) ===
public interface IDeleteEstablishment
{
    Task<bool> ExecuteAsync(int id);
}

public class DeleteEstablishment : IDeleteEstablishment
{
    private readonly IEstablishmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEstablishment(IEstablishmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var establishment = await _repository.GetByIdAsync(id);
        if (establishment == null) return false;

        establishment.IsActive = false;
        await _repository.UpdateAsync(establishment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

// === ADD PHOTO ===
public interface IAddEstablishmentPhoto
{
    Task<PhotoDto> ExecuteAsync(int establishmentId, CreatePhotoDto dto);
}

public class AddEstablishmentPhoto : IAddEstablishmentPhoto
{
    private readonly IEstablishmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddEstablishmentPhoto(IEstablishmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PhotoDto> ExecuteAsync(int establishmentId, CreatePhotoDto dto)
    {
        var establishment = await _repository.GetByIdAsync(establishmentId);
        if (establishment == null)
            throw new InvalidOperationException("El establecimiento no existe");

        var photo = new EstablishmentPhoto
        {
            EstablishmentId = establishmentId,
            ImageData = dto.ImageData,
            DisplayOrder = dto.DisplayOrder
        };

        await _repository.AddPhotoAsync(photo);
        await _unitOfWork.SaveChangesAsync();

        return new PhotoDto(photo.Id, photo.ImageData, photo.DisplayOrder);
    }
}

// === REMOVE PHOTO ===
public interface IRemoveEstablishmentPhoto
{
    Task<bool> ExecuteAsync(int establishmentId, int photoId);
}

public class RemoveEstablishmentPhoto : IRemoveEstablishmentPhoto
{
    private readonly IEstablishmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveEstablishmentPhoto(IEstablishmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int establishmentId, int photoId)
    {
        await _repository.RemovePhotoAsync(photoId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

// === SET SCHEDULES ===
public interface ISetEstablishmentSchedules
{
    Task<IEnumerable<ScheduleDto>> ExecuteAsync(int establishmentId, SetSchedulesDto dto);
}

public class SetEstablishmentSchedules : ISetEstablishmentSchedules
{
    private readonly IEstablishmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SetEstablishmentSchedules(IEstablishmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ScheduleDto>> ExecuteAsync(int establishmentId, SetSchedulesDto dto)
    {
        var establishment = await _repository.GetByIdAsync(establishmentId);
        if (establishment == null)
            throw new InvalidOperationException("El establecimiento no existe");

        // Update schedule type
        establishment.ScheduleType = (ScheduleType)dto.ScheduleType;
        await _repository.UpdateAsync(establishment);

        // Set new schedules
        var schedules = dto.Schedules.Select(s => new EstablishmentSchedule
        {
            EstablishmentId = establishmentId,
            DayOfWeek = s.DayOfWeek,
            OpenTime = TimeSpan.Parse(s.OpenTime),
            CloseTime = TimeSpan.Parse(s.CloseTime),
            BlockNumber = s.BlockNumber
        });

        await _repository.SetSchedulesAsync(establishmentId, schedules);
        await _unitOfWork.SaveChangesAsync();

        var savedSchedules = await _repository.GetSchedulesAsync(establishmentId);
        return savedSchedules.Select(s => s.ToDto());
    }
}

// === MAPPING EXTENSIONS ===
public static class EstablishmentMappingExtensions
{
    private static readonly string[] DayNames = { "", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

    public static EstablishmentListDto ToListDto(this Establishment e) => new(
        e.Id,
        e.Name,
        e.CountryId,
        e.Country?.Name ?? "",
        e.CityId,
        e.City?.Name ?? "",
        e.Address,
        e.Courts?.Count(c => c.IsActive) ?? 0,
        e.IsActive
    );

    public static EstablishmentDto ToDto(this Establishment e) => new(
        e.Id,
        e.Name,
        e.CountryId,
        e.Country?.Name ?? "",
        e.CityId,
        e.City?.Name ?? "",
        e.Address,
        e.GoogleMapsUrl,
        e.PhoneLandline,
        e.PhoneMobile,
        e.Logo,
        (int)e.ScheduleType,
        e.IsActive,
        e.CreatedAt,
        e.UpdatedAt,
        e.Courts?.Where(c => c.IsActive).Select(c => c.ToDto()) ?? Enumerable.Empty<CourtDto>(),
        e.Photos?.Select(p => new PhotoDto(p.Id, p.ImageData, p.DisplayOrder)) ?? Enumerable.Empty<PhotoDto>(),
        e.Schedules?.Select(s => s.ToDto()) ?? Enumerable.Empty<ScheduleDto>()
    );

    public static CourtDto ToDto(this Court c) => new(
        c.Id,
        c.EstablishmentId,
        c.Name,
        (int)c.CourtType,
        c.CourtType == CourtType.Indoor ? "Indoor" : "Outdoor",
        c.IsActive,
        c.Photos?.Select(p => new PhotoDto(p.Id, p.ImageData, p.DisplayOrder)) ?? Enumerable.Empty<PhotoDto>()
    );

    public static ScheduleDto ToDto(this EstablishmentSchedule s) => new(
        s.Id,
        s.DayOfWeek,
        s.DayOfWeek >= 1 && s.DayOfWeek <= 7 ? DayNames[s.DayOfWeek] : "N/A",
        s.OpenTime.ToString(@"hh\:mm"),
        s.CloseTime.ToString(@"hh\:mm"),
        s.BlockNumber
    );
}
