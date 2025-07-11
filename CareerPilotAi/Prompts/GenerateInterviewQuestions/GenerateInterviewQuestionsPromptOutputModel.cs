namespace CareerPilotAi.Prompts.GenerateInterviewQuestions;

public class GenerateInterviewQuestionsPromptOutputModel
{
    public string OutputStatus { get; set; }
    public string OutputFeedbackMessage { get; set; }
    public PromptOutputInterviewQuestion[] InterviewQuestions { get; set; }
}

public class PromptOutputInterviewQuestion
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Guide { get; set; }
}