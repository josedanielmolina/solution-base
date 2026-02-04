namespace Core.Application.DTOs.Categories;

public record CategoryDto(
    int Id,
    string Name,
    string Gender,
    int CountryId,
    string CountryName,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateCategoryDto(
    string Name,
    int Gender,
    int CountryId
);

public record UpdateCategoryDto(
    string Name,
    int Gender,
    int CountryId,
    bool IsActive
);

