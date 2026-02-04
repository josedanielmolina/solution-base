using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Document)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Document)
            .IsUnique();

        builder.Property(p => p.Email)
            .HasMaxLength(255);

        builder.Property(p => p.Phone)
            .HasMaxLength(20);

        builder.Property(p => p.BirthDate)
            .IsRequired(false);

        builder.Property(p => p.Photo)
            .HasColumnType("VARBINARY(MAX)");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(p => p.IsDeleted);
        builder.HasIndex(p => p.IsActive);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        // Relationships
        builder.HasOne(p => p.City)
            .WithMany(c => c.Players)
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.User)
            .WithOne(u => u.Player)
            .HasForeignKey<Player>(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Query filter for soft delete
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}

