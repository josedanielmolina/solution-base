namespace Core.Domain.Entities;

public enum ScheduleType
{
    Continuous = 1,
    Blocks = 2
}

public class Establishment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? GoogleMapsUrl { get; set; }
    public string? PhoneLandline { get; set; }
    public string? PhoneMobile { get; set; }
    public string? Logo { get; set; }
    public ScheduleType ScheduleType { get; set; } = ScheduleType.Continuous;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Country Country { get; set; } = null!;
    public virtual City City { get; set; } = null!;
    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
    public virtual ICollection<EstablishmentPhoto> Photos { get; set; } = new List<EstablishmentPhoto>();
    public virtual ICollection<EstablishmentSchedule> Schedules { get; set; } = new List<EstablishmentSchedule>();
}
