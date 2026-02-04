namespace Core.Domain.Entities;

public class City : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Country Country { get; set; } = null!;
    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
