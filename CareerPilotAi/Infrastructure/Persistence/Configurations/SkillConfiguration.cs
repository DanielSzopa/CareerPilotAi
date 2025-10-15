using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class SkillConfiguration : IEntityTypeConfiguration<SkillDataModel>
{
    public void Configure(EntityTypeBuilder<SkillDataModel> builder)
    {
        builder.ToTable("Skills");

        // Primary Key
        builder.HasKey(s => s.SkillId);

        // Properties
        builder.Property(s => s.SkillId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(s => s.JobApplicationId)
            .IsRequired();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Level)
            .IsRequired()
            .HasMaxLength(50);

        // Indexes
        builder.HasIndex(s => s.JobApplicationId);
    }
}

