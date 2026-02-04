namespace Core.Application.DTOs.Countries;

public record CountryDto(
    int Id,
    string Name,
    string Code,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateCountryDto(
    string Name,
    string Code
);

public record UpdateCountryDto(
    string Name,
    string Code,
    bool IsActive
);
