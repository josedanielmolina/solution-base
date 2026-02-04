using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EventEstablishmentConfiguration : IEntityTypeConfiguration<EventEstablishment>
{
    public void Configure(EntityTypeBuilder<EventEstablishment> builder)
    {
        builder.ToTable("EventEstablishments");

        builder.HasKey(ee => ee.Id);

        builder.Property(ee => ee.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Unique constraint
        builder.HasIndex(ee => new { ee.EventId, ee.EstablishmentId }).IsUnique();

        // Relationships
        builder.HasOne(ee => ee.Event)
            .WithMany(e => e.Establishments)
            .HasForeignKey(ee => ee.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ee => ee.Establishment)
            .WithMany()
            .HasForeignKey(ee => ee.EstablishmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
