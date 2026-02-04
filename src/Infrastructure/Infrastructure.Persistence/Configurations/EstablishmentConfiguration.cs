using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
{
    public void Configure(EntityTypeBuilder<Establishment> builder)
    {
        builder.ToTable("Establishments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.GoogleMapsUrl)
            .HasMaxLength(500);

        builder.Property(e => e.PhoneLandline)
            .HasMaxLength(20);

        builder.Property(e => e.PhoneMobile)
            .HasMaxLength(20);

        builder.Property(e => e.Logo)
            .HasColumnType("LONGTEXT");

        builder.Property(e => e.ScheduleType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Unique constraint
        builder.HasIndex(e => e.Name)
            .IsUnique();

        // Relationships
        builder.HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Courts)
            .WithOne(c => c.Establishment)
            .HasForeignKey(c => c.EstablishmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Photos)
            .WithOne(p => p.Establishment)
            .HasForeignKey(p => p.EstablishmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Schedules)
            .WithOne(s => s.Establishment)
            .HasForeignKey(s => s.EstablishmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
