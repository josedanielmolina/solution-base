using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourtPhotoConfiguration : IEntityTypeConfiguration<CourtPhoto>
{
    public void Configure(EntityTypeBuilder<CourtPhoto> builder)
    {
        builder.ToTable("CourtPhotos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ImageData)
            .IsRequired()
            .HasColumnType("LONGTEXT");

        builder.Property(p => p.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
