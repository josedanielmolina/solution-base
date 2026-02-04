namespace Core.Domain.Entities;

public class EventInvitation
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Event Event { get; set; } = null!;

    /// <summary>
    /// Verifica si la invitación ha expirado
    /// </summary>
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    /// <summary>
    /// Verifica si la invitación ya fue aceptada
    /// </summary>
    public bool IsAccepted => AcceptedAt.HasValue;

    /// <summary>
    /// Verifica si la invitación es válida (no expirada y no aceptada)
    /// </summary>
    public bool IsValid => !IsExpired && !IsAccepted;
}
