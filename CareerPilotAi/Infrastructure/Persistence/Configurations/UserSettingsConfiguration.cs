using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettingsDataModel>
{
    public void Configure(EntityTypeBuilder<UserSettingsDataModel> builder)
    {
        builder.ToTable("UserSettings");

        builder.HasKey(us => us.UserSettingsId);

        builder.Property(us => us.UserSettingsId)
            .IsRequired();

        builder.Property(us => us.UserId)
            .IsRequired();

        builder.Property(us => us.TimeZoneId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(us => us.UserId)
            .IsUnique();

        builder.HasOne(us => us.User)
            .WithOne()
            .HasForeignKey<UserSettingsDataModel>(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(us => us.TimeZone)
            .WithMany(tz => tz.UserSettings)
            .HasForeignKey(us => us.TimeZoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

