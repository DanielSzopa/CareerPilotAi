namespace CareerPilotAi.Prompts.GenerateInterviewQuestions;

public class GenerateInterviewQuestionsPromptOutputModel
{
    public string OutputStatus { get; set; }
    public string OutputFeedbackMessage { get; set; }
    public InterviewQuestion[] InterviewQuestions { get; set; }
}

public class InterviewQuestion
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Status { get; set; }
    public string FeedbackMessage { get; set; }
}