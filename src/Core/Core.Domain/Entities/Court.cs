namespace Core.Domain.Entities;

public enum CourtType
{
    Indoor = 1,
    Outdoor = 2
}

public class Court : BaseEntity
{
    public int EstablishmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CourtType CourtType { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Establishment Establishment { get; set; } = null!;
    public virtual ICollection<CourtPhoto> Photos { get; set; } = new List<CourtPhoto>();
}
