using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourtConfiguration : IEntityTypeConfiguration<Court>
{
    public void Configure(EntityTypeBuilder<Court> builder)
    {
        builder.ToTable("Courts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.CourtType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Composite unique index
        builder.HasIndex(c => new { c.Name, c.EstablishmentId })
            .IsUnique();

        // Relationships
        builder.HasMany(c => c.Photos)
            .WithOne(p => p.Court)
            .HasForeignKey(p => p.CourtId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
