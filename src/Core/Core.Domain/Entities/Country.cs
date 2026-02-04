namespace Core.Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // ISO 2-3 chars
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
