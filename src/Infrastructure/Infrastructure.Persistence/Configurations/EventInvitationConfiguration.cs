using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EventInvitationConfiguration : IEntityTypeConfiguration<EventInvitation>
{
    public void Configure(EntityTypeBuilder<EventInvitation> builder)
    {
        builder.ToTable("EventInvitations");

        builder.HasKey(ei => ei.Id);

        builder.Property(ei => ei.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(ei => ei.Token)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ei => ei.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Unique constraint for token
        builder.HasIndex(ei => ei.Token).IsUnique();
        builder.HasIndex(ei => ei.Email);
        builder.HasIndex(ei => ei.EventId);

        // Ignore computed properties
        builder.Ignore(ei => ei.IsExpired);
        builder.Ignore(ei => ei.IsAccepted);
        builder.Ignore(ei => ei.IsValid);

        // Relationships
        builder.HasOne(ei => ei.Event)
            .WithMany(e => e.Invitations)
            .HasForeignKey(ei => ei.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
