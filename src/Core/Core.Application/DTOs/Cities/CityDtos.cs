namespace Core.Application.DTOs.Cities;

public record CityDto(
    int Id,
    string Name,
    int CountryId,
    string CountryName,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateCityDto(
    string Name,
    int CountryId
);

public record UpdateCityDto(
    string Name,
    int CountryId,
    bool IsActive
);
