using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EventAdminConfiguration : IEntityTypeConfiguration<EventAdmin>
{
    public void Configure(EntityTypeBuilder<EventAdmin> builder)
    {
        builder.ToTable("EventAdmins");

        builder.HasKey(ea => ea.Id);

        builder.Property(ea => ea.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Unique constraint
        builder.HasIndex(ea => new { ea.EventId, ea.UserId }).IsUnique();

        // Relationships
        builder.HasOne(ea => ea.Event)
            .WithMany(e => e.Admins)
            .HasForeignKey(ea => ea.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ea => ea.User)
            .WithMany()
            .HasForeignKey(ea => ea.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
