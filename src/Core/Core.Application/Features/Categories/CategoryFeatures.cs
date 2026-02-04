using Core.Application.DTOs.Categories;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Categories;

public interface IGetCategories
{
    Task<IEnumerable<CategoryDto>> ExecuteAsync(bool activeOnly = true);
}

public class GetCategories : IGetCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> ExecuteAsync(bool activeOnly = true)
    {
        var categories = activeOnly 
            ? await _categoryRepository.GetAllActiveAsync()
            : await _categoryRepository.GetAllAsync();

        return categories.Select(c => c.ToDto());
    }
}

public interface IGetCategoriesByCountry
{
    Task<IEnumerable<CategoryDto>> ExecuteAsync(int countryId, Gender? gender = null);
}

public class GetCategoriesByCountry : IGetCategoriesByCountry
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesByCountry(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> ExecuteAsync(int countryId, Gender? gender = null)
    {
        var categories = gender.HasValue
            ? await _categoryRepository.GetByCountryAndGenderAsync(countryId, gender.Value)
            : await _categoryRepository.GetByCountryAsync(countryId);

        return categories.Select(c => c.ToDto());
    }
}

public interface IGetCategoryById
{
    Task<CategoryDto?> ExecuteAsync(int id);
}

public class GetCategoryById : IGetCategoryById
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryById(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto?> ExecuteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category?.ToDto();
    }
}

public interface ICreateCategory
{
    Task<CategoryDto> ExecuteAsync(CreateCategoryDto dto);
}

public class CreateCategory : ICreateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategory(ICategoryRepository categoryRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto> ExecuteAsync(CreateCategoryDto dto)
    {
        // Validate country exists
        if (!await _countryRepository.ExistsAsync(dto.CountryId))
            throw new InvalidOperationException("El país no existe");

        // Check for duplicates within country and gender
        if (await _categoryRepository.ExistsByNameInCountryAndGenderAsync(dto.Name, dto.CountryId, (Gender)dto.Gender))
            throw new InvalidOperationException("Ya existe una categoría con ese nombre para el país y género seleccionados");

        var category = new Category
        {
            Name = dto.Name,
            Gender = (Gender)dto.Gender,
            CountryId = dto.CountryId,
            IsActive = true
        };

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        // Reload to get country name
        var created = await _categoryRepository.GetByIdAsync(category.Id);
        return created!.ToDto();
    }
}

public interface IUpdateCategory
{
    Task<CategoryDto?> ExecuteAsync(int id, UpdateCategoryDto dto);
}

public class UpdateCategory : IUpdateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategory(ICategoryRepository categoryRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto?> ExecuteAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return null;

        // Validate country exists
        if (!await _countryRepository.ExistsAsync(dto.CountryId))
            throw new InvalidOperationException("El país no existe");

        // Check for duplicates (excluding current)
        var existing = await _categoryRepository.GetByCountryAndGenderAsync(dto.CountryId, (Gender)dto.Gender);
        if (existing.Any(c => c.Name == dto.Name && c.Id != id))
            throw new InvalidOperationException("Ya existe una categoría con ese nombre para el país y género seleccionados");

        category.Name = dto.Name;
        category.Gender = (Gender)dto.Gender;
        category.CountryId = dto.CountryId;
        category.IsActive = dto.IsActive;

        await _categoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        // Reload to get country name
        var updated = await _categoryRepository.GetByIdAsync(id);
        return updated!.ToDto();
    }
}

public interface IDeleteCategory
{
    Task<bool> ExecuteAsync(int id);
}

public class DeleteCategory : IDeleteCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return false;

        category.IsActive = false;
        await _categoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

// Mapping extension
public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category category) => new(
        category.Id,
        category.Name,
        category.Gender.ToString(),
        category.CountryId,
        category.Country?.Name ?? "",
        category.IsActive,
        category.CreatedAt,
        category.UpdatedAt
    );
}
