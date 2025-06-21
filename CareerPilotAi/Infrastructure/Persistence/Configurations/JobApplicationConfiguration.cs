using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplicationDataModel>
{
    public void Configure(EntityTypeBuilder<JobApplicationDataModel> builder)
    {
        builder.HasKey(x => x.JobApplicationId);

        builder.Property(x => x.Title)
            .IsRequired();

        builder.Property(x => x.Company)
            .IsRequired();

        builder.Property(x => x.Url)
            .IsRequired(false);

        builder.Property(x => x.JobDescription)
            .IsRequired(true);
            
        // Configure relationship with IdentityUser
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 