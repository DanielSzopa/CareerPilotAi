using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class TimeZoneConfiguration : IEntityTypeConfiguration<TimeZoneDataModel>
{
    public void Configure(EntityTypeBuilder<TimeZoneDataModel> builder)
    {
        builder.ToTable("TimeZones");

        builder.HasKey(tz => tz.TimeZoneId);

        builder.Property(tz => tz.TimeZoneId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tz => tz.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany(tz => tz.UserSettings)
            .WithOne(us => us.TimeZone)
            .HasForeignKey(us => us.TimeZoneId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new TimeZoneDataModel { TimeZoneId = "UTC", Name = "UTC" },
            new TimeZoneDataModel { TimeZoneId = "Europe/Warsaw", Name = "Europe/Warsaw" },
            new TimeZoneDataModel { TimeZoneId = "Europe/London", Name = "Europe/London" },
            new TimeZoneDataModel { TimeZoneId = "America/New_York", Name = "America/New_York" },
            new TimeZoneDataModel { TimeZoneId = "Asia/Tokyo", Name = "Asia/Tokyo" }
        );
    }
}

