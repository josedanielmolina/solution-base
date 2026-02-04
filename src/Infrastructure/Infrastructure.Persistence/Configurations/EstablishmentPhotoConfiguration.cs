using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EstablishmentPhotoConfiguration : IEntityTypeConfiguration<EstablishmentPhoto>
{
    public void Configure(EntityTypeBuilder<EstablishmentPhoto> builder)
    {
        builder.ToTable("EstablishmentPhotos");

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
