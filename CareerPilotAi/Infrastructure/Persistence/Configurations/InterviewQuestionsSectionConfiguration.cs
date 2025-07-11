using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class InterviewQuestionsSectionConfiguration : IEntityTypeConfiguration<InterviewQuestionsSectionDataModel>
{
    public void Configure(EntityTypeBuilder<InterviewQuestionsSectionDataModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PreparationContent)
            .IsRequired();

        builder.Property(x => x.InterviewQuestionsFeedbackMessage)
            .IsRequired(false);

        builder.Property(x => x.Status)
            .IsRequired(false);

        // Configure relationship with JobApplicationDataModel
        builder.HasOne(x => x.JobApplication)
            .WithMany()
            .HasForeignKey(x => x.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure relationship with InterviewQuestionDataModel
        builder.HasMany(x => x.Questions)
            .WithOne(x => x.InterviewQuestionsSection)
            .HasForeignKey(x => x.InterviewQuestionsSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
