namespace Core.Domain.Entities;

public class EstablishmentSchedule
{
    public int Id { get; set; }
    public int EstablishmentId { get; set; }
    public int DayOfWeek { get; set; } // 1=Monday, 7=Sunday
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public int BlockNumber { get; set; } = 1;

    // Navigation properties
    public virtual Establishment Establishment { get; set; } = null!;
}
