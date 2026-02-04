namespace Core.Domain.Entities;

public class Event : BaseEntity
{
    public Guid PublicId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? OrganizerId { get; set; }
    public string? ContactPhone { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? PosterVertical { get; set; }
    public string? PosterHorizontal { get; set; }
    public string? RulesPdf { get; set; }
    public string? WhatsApp { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual User? Organizer { get; set; }
    public virtual ICollection<EventEstablishment> Establishments { get; set; } = new List<EventEstablishment>();
    public virtual ICollection<EventAdmin> Admins { get; set; } = new List<EventAdmin>();
    public virtual ICollection<EventInvitation> Invitations { get; set; } = new List<EventInvitation>();

    /// <summary>
    /// Verifica si un usuario tiene acceso al evento (es Organizador o Admin)
    /// </summary>
    public bool HasAccess(int userId) =>
        OrganizerId == userId || Admins?.Any(a => a.UserId == userId) == true;
}
