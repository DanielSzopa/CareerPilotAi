namespace CareerPilotAi.Models.JobApplication;

public class InterviewQuestionsSectionViewModel
{
    public Guid Id { get; set; }
    public string PreparationContent { get; set; } = string.Empty;
    public string? Status { get; set; }
    public string? FeedbackMessage { get; set; }
    public List<InterviewQuestionViewModel> InterviewQuestions { get; set; } = new();
}

public class InterviewQuestionViewModel
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string Guide { get; set; } = string.Empty;
}
