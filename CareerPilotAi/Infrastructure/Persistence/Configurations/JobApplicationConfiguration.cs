using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplicationDataModel>
{
    public void Configure(EntityTypeBuilder<JobApplicationDataModel> builder)
    {
        builder.ToTable("JobApplications");

        // Primary Key
        builder.HasKey(x => x.JobApplicationId);

        // Basic Properties
        builder.Property(x => x.JobApplicationId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Company)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Url)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.JobDescription)
            .IsRequired(true);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(ApplicationStatus.DefaultStatus);

        builder.Property(x => x.CreatedAt)
            .IsRequired(true);

        builder.Property(x => x.UpdatedAt)
            .IsRequired(true);

        // Extended Properties
        builder.Property(x => x.ExperienceLevel)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Location)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.WorkMode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ContractType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.SalaryMin)
            .HasPrecision(18, 2);

        builder.Property(x => x.SalaryMax)
            .HasPrecision(18, 2);

        builder.Property(x => x.SalaryType)
            .HasMaxLength(50);

        builder.Property(x => x.SalaryPeriod)
            .HasMaxLength(50);

        // Relationships
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Skills)
            .WithOne(s => s.JobApplication)
            .HasForeignKey(s => s.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CreatedAt);
    }
} 