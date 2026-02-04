namespace Core.Application.DTOs.Establishments;

// === List/Detail DTOs ===
public record EstablishmentListDto(
    int Id,
    string Name,
    int CountryId,
    string CountryName,
    int CityId,
    string CityName,
    string Address,
    int CourtsCount,
    bool IsActive
);

public record EstablishmentDto(
    int Id,
    string Name,
    int CountryId,
    string CountryName,
    int CityId,
    string CityName,
    string Address,
    string? GoogleMapsUrl,
    string? PhoneLandline,
    string? PhoneMobile,
    string? Logo,
    int ScheduleType,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<CourtDto> Courts,
    IEnumerable<PhotoDto> Photos,
    IEnumerable<ScheduleDto> Schedules
);

// === Create/Update DTOs ===
public record CreateEstablishmentDto(
    string Name,
    int CountryId,
    int CityId,
    string Address,
    string? GoogleMapsUrl,
    string? PhoneLandline,
    string? PhoneMobile,
    string? Logo,
    int ScheduleType
);

public record UpdateEstablishmentDto(
    string Name,
    int CountryId,
    int CityId,
    string Address,
    string? GoogleMapsUrl,
    string? PhoneLandline,
    string? PhoneMobile,
    string? Logo,
    int ScheduleType,
    bool IsActive
);

// === Court DTOs ===
public record CourtDto(
    int Id,
    int EstablishmentId,
    string Name,
    int CourtType,
    string CourtTypeName,
    bool IsActive,
    IEnumerable<PhotoDto> Photos
);

public record CreateCourtDto(
    string Name,
    int CourtType
);

public record UpdateCourtDto(
    string Name,
    int CourtType,
    bool IsActive
);

// === Photo DTOs ===
public record PhotoDto(
    int Id,
    string ImageData,
    int DisplayOrder
);

public record CreatePhotoDto(
    string ImageData,
    int DisplayOrder
);

// === Schedule DTOs ===
public record ScheduleDto(
    int Id,
    int DayOfWeek,
    string DayName,
    string OpenTime,
    string CloseTime,
    int BlockNumber
);

public record SetScheduleDto(
    int DayOfWeek,
    string OpenTime,
    string CloseTime,
    int BlockNumber
);

public record SetSchedulesDto(
    int ScheduleType,
    IEnumerable<SetScheduleDto> Schedules
);
