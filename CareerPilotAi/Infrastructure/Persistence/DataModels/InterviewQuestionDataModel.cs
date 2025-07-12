namespace CareerPilotAi.Infrastructure.Persistence.DataModels;

internal class InterviewQuestionDataModel
{
    internal Guid Id { get; set; }
    internal string Question { get; set; }
    internal string Answer { get; set; }
    internal string Guide { get; set; }
    internal bool IsActive { get; set; }
    internal Guid InterviewQuestionsSectionId { get; set; }
    internal InterviewQuestionsSectionDataModel InterviewQuestionsSection { get; set; }
}

internal class InterviewQuestionsSectionDataModel
{
    internal Guid Id { get; set; }
    internal Guid JobApplicationId { get; set; }
    internal string PreparationContent { get; set; }
    internal string? InterviewQuestionsFeedbackMessage { get; set; }
    internal string Status { get; set; }
    internal ICollection<InterviewQuestionDataModel> Questions { get; set; } = new List<InterviewQuestionDataModel>();
    internal JobApplicationDataModel JobApplication { get; set; }
}
