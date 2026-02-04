using Core.Application.DTOs.Establishments;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Establishments;

// === GET BY ESTABLISHMENT ===
public interface IGetCourtsByEstablishment
{
    Task<IEnumerable<CourtDto>> ExecuteAsync(int establishmentId, bool activeOnly = true);
}

public class GetCourtsByEstablishment : IGetCourtsByEstablishment
{
    private readonly ICourtRepository _repository;

    public GetCourtsByEstablishment(ICourtRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CourtDto>> ExecuteAsync(int establishmentId, bool activeOnly = true)
    {
        var courts = await _repository.GetByEstablishmentAsync(establishmentId, activeOnly);
        return courts.Select(c => c.ToDto());
    }
}

// === GET BY ID ===
public interface IGetCourtById
{
    Task<CourtDto?> ExecuteAsync(int id);
}

public class GetCourtById : IGetCourtById
{
    private readonly ICourtRepository _repository;

    public GetCourtById(ICourtRepository repository)
    {
        _repository = repository;
    }

    public async Task<CourtDto?> ExecuteAsync(int id)
    {
        var court = await _repository.GetByIdWithPhotosAsync(id);
        return court?.ToDto();
    }
}

// === CREATE ===
public interface ICreateCourt
{
    Task<CourtDto> ExecuteAsync(int establishmentId, CreateCourtDto dto);
}

public class CreateCourt : ICreateCourt
{
    private readonly ICourtRepository _courtRepository;
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourt(
        ICourtRepository courtRepository,
        IEstablishmentRepository establishmentRepository,
        IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _establishmentRepository = establishmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CourtDto> ExecuteAsync(int establishmentId, CreateCourtDto dto)
    {
        // Validate establishment exists
        var establishment = await _establishmentRepository.GetByIdAsync(establishmentId);
        if (establishment == null)
            throw new InvalidOperationException("El establecimiento no existe");

        // Validate name is unique within establishment
        if (await _courtRepository.ExistsByNameInEstablishmentAsync(dto.Name, establishmentId))
            throw new InvalidOperationException("Ya existe una cancha con ese nombre en este establecimiento");

        var court = new Court
        {
            EstablishmentId = establishmentId,
            Name = dto.Name,
            CourtType = (CourtType)dto.CourtType,
            IsActive = true
        };

        await _courtRepository.AddAsync(court);
        await _unitOfWork.SaveChangesAsync();

        // Reload with photos
        var created = await _courtRepository.GetByIdWithPhotosAsync(court.Id);
        return created!.ToDto();
    }
}

// === UPDATE ===
public interface IUpdateCourt
{
    Task<CourtDto?> ExecuteAsync(int establishmentId, int id, UpdateCourtDto dto);
}

public class UpdateCourt : IUpdateCourt
{
    private readonly ICourtRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourt(ICourtRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CourtDto?> ExecuteAsync(int establishmentId, int id, UpdateCourtDto dto)
    {
        var court = await _repository.GetByIdAsync(id);
        if (court == null || court.EstablishmentId != establishmentId) return null;

        // Validate name is unique (excluding current)
        if (await _repository.ExistsByNameInEstablishmentAsync(dto.Name, establishmentId, id))
            throw new InvalidOperationException("Ya existe otra cancha con ese nombre en este establecimiento");

        court.Name = dto.Name;
        court.CourtType = (CourtType)dto.CourtType;
        court.IsActive = dto.IsActive;

        await _repository.UpdateAsync(court);
        await _unitOfWork.SaveChangesAsync();

        // Reload with photos
        var updated = await _repository.GetByIdWithPhotosAsync(id);
        return updated!.ToDto();
    }
}

// === DELETE (Soft) ===
public interface IDeleteCourt
{
    Task<bool> ExecuteAsync(int establishmentId, int id);
}

public class DeleteCourt : IDeleteCourt
{
    private readonly ICourtRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourt(ICourtRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int establishmentId, int id)
    {
        var court = await _repository.GetByIdAsync(id);
        if (court == null || court.EstablishmentId != establishmentId) return false;

        court.IsActive = false;
        await _repository.UpdateAsync(court);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

// === ADD PHOTO ===
public interface IAddCourtPhoto
{
    Task<PhotoDto> ExecuteAsync(int courtId, CreatePhotoDto dto);
}

public class AddCourtPhoto : IAddCourtPhoto
{
    private readonly ICourtRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourtPhoto(ICourtRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PhotoDto> ExecuteAsync(int courtId, CreatePhotoDto dto)
    {
        var court = await _repository.GetByIdAsync(courtId);
        if (court == null)
            throw new InvalidOperationException("La cancha no existe");

        var photo = new CourtPhoto
        {
            CourtId = courtId,
            ImageData = dto.ImageData,
            DisplayOrder = dto.DisplayOrder
        };

        await _repository.AddPhotoAsync(photo);
        await _unitOfWork.SaveChangesAsync();

        return new PhotoDto(photo.Id, photo.ImageData, photo.DisplayOrder);
    }
}

// === REMOVE PHOTO ===
public interface IRemoveCourtPhoto
{
    Task<bool> ExecuteAsync(int courtId, int photoId);
}

public class RemoveCourtPhoto : IRemoveCourtPhoto
{
    private readonly ICourtRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourtPhoto(ICourtRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int courtId, int photoId)
    {
        await _repository.RemovePhotoAsync(photoId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
