using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.PublicId)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasColumnType("TEXT");

        builder.Property(e => e.ContactPhone)
            .HasMaxLength(20);

        builder.Property(e => e.PosterVertical)
            .HasColumnType("LONGTEXT");

        builder.Property(e => e.PosterHorizontal)
            .HasColumnType("LONGTEXT");

        builder.Property(e => e.RulesPdf)
            .HasColumnType("LONGTEXT");

        builder.Property(e => e.WhatsApp)
            .HasMaxLength(50);

        builder.Property(e => e.Facebook)
            .HasMaxLength(200);

        builder.Property(e => e.Instagram)
            .HasMaxLength(200);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(e => e.PublicId).IsUnique();
        builder.HasIndex(e => e.OrganizerId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.StartDate);

        // Relationships
        builder.HasOne(e => e.Organizer)
            .WithMany()
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Establishments)
            .WithOne(ee => ee.Event)
            .HasForeignKey(ee => ee.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Admins)
            .WithOne(ea => ea.Event)
            .HasForeignKey(ea => ea.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Invitations)
            .WithOne(ei => ei.Event)
            .HasForeignKey(ei => ei.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
