using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplicationDataModel>
{
    public void Configure(EntityTypeBuilder<JobApplicationDataModel> builder)
    {
        builder.HasKey(x => x.JobApplicationId);
            
        builder.Property(x => x.EntryJobDetails_Url)
            .IsRequired(false);
            
        builder.Property(x => x.EntryJobDetails_Text)
            .IsRequired(true);
            
        builder.Property(x => x.PersonalDetails_Text)
            .IsRequired(false);
            
        // Configure relationship with IdentityUser
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 