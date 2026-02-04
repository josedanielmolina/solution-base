using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EstablishmentScheduleConfiguration : IEntityTypeConfiguration<EstablishmentSchedule>
{
    public void Configure(EntityTypeBuilder<EstablishmentSchedule> builder)
    {
        builder.ToTable("EstablishmentSchedules");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.DayOfWeek)
            .IsRequired();

        builder.Property(s => s.OpenTime)
            .IsRequired();

        builder.Property(s => s.CloseTime)
            .IsRequired();

        builder.Property(s => s.BlockNumber)
            .HasDefaultValue(1);

        builder.HasIndex(s => s.DayOfWeek);
    }
}
