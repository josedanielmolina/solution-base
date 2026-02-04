namespace Core.Domain.Entities;

public class EstablishmentPhoto
{
    public int Id { get; set; }
    public int EstablishmentId { get; set; }
    public string ImageData { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Establishment Establishment { get; set; } = null!;
}
