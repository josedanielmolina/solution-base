namespace Core.Domain.Entities;

public enum Gender
{
    Male = 1,
    Female = 2,
    Mixed = 3
}

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int CountryId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Country Country { get; set; } = null!;
}
