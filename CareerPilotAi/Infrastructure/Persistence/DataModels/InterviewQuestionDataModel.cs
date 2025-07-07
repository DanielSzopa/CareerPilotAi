namespace CareerPilotAi.Infrastructure.Persistence.DataModels
{
    internal class InterviewQuestionDataModel
    {
        internal Guid Id { get; set; }
        internal Guid JobApplicationId { get; set; }
        internal string Question { get; set; }
        internal string Answer { get; set; }
        internal string Status { get; set; }
        internal string? FeedbackMessage { get; set; }
        internal JobApplicationDataModel JobApplication { get; set; }
    }
}
