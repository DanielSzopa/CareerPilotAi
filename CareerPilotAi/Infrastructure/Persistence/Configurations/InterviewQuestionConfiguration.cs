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

        builder.Property(x => x.Guide)
            .IsRequired();

        // Configure relationship with InterviewQuestionsSectionDataModel
        builder.HasOne(x => x.InterviewQuestionsSection)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.InterviewQuestionsSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
