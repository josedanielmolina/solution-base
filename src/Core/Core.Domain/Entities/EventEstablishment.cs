namespace Core.Domain.Entities;

public class EventEstablishment
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int EstablishmentId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Event Event { get; set; } = null!;
    public virtual Establishment Establishment { get; set; } = null!;
}
