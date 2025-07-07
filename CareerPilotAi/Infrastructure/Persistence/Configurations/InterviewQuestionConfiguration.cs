using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations;

internal class InterviewQuestionConfiguration : IEntityTypeConfiguration<InterviewQuestionDataModel>
{
    public void Configure(EntityTypeBuilder<InterviewQuestionDataModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Question)
            .IsRequired();

        builder.Property(x => x.Answer)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.FeedbackMessage)
            .IsRequired(false);

        // Configure relationship with JobApplicationDataModel
        builder.HasOne<JobApplicationDataModel>()
            .WithMany(x => x.InterviewQuestions)
            .HasForeignKey(x => x.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
