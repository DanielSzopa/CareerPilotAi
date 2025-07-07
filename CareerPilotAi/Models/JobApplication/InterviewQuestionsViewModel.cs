namespace CareerPilotAi.Models.JobApplication;

public class InterviewQuestionsViewModel
{
    public List<InterviewQuestionViewModel> InterviewQuestions { get; set; } = new();
}

public class InterviewQuestionViewModel
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? FeedbackMessage { get; set; }
}
