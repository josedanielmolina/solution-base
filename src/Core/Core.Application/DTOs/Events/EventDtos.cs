namespace Core.Application.DTOs.Events;

// List item DTO (without large fields like posters)
public record EventListDto(
    int Id,
    Guid PublicId,
    string Name,
    string? Description,
    int? OrganizerId,
    string? OrganizerName,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    int EstablishmentsCount,
    int AdminsCount,
    DateTime CreatedAt
);

// Full detail DTO
public record EventDto(
    int Id,
    Guid PublicId,
    string Name,
    string? Description,
    int? OrganizerId,
    string? OrganizerName,
    string? ContactPhone,
    DateTime StartDate,
    DateTime EndDate,
    string? PosterVertical,
    string? PosterHorizontal,
    string? RulesPdf,
    string? WhatsApp,
    string? Facebook,
    string? Instagram,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<EventEstablishmentDto> Establishments,
    IEnumerable<EventAdminDto> Admins
);

// Create DTO
public record CreateEventDto(
    string Name,
    string? Description,
    int? OrganizerId,
    string? ContactPhone,
    DateTime StartDate,
    DateTime EndDate,
    string? WhatsApp,
    string? Facebook,
    string? Instagram
);

// Update DTO
public record UpdateEventDto(
    string Name,
    string? Description,
    int? OrganizerId,
    string? ContactPhone,
    DateTime StartDate,
    DateTime EndDate,
    string? WhatsApp,
    string? Facebook,
    string? Instagram
);

// Poster upload DTO
public record UploadPosterDto(
    string ImageData  // Base64
);

// Rules PDF upload DTO
public record UploadRulesPdfDto(
    string PdfData  // Base64
);

// Event Establishment DTO
public record EventEstablishmentDto(
    int Id,
    int EstablishmentId,
    string EstablishmentName,
    string? City,
    DateTime CreatedAt
);

// Add establishment DTO
public record AddEventEstablishmentDto(
    int EstablishmentId
);

// Event Admin DTO
public record EventAdminDto(
    int Id,
    int UserId,
    string UserName,
    string UserEmail,
    DateTime CreatedAt
);

// Invite admin DTO
public record InviteAdminDto(
    string Email
);

// Event Invitation DTO
public record EventInvitationDto(
    int Id,
    string Email,
    string Token,
    DateTime ExpiresAt,
    DateTime? AcceptedAt,
    DateTime CreatedAt,
    bool IsExpired,
    bool IsAccepted
);
