namespace Core.Domain.Entities;

public class CourtPhoto
{
    public int Id { get; set; }
    public int CourtId { get; set; }
    public string ImageData { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Court Court { get; set; } = null!;
}
