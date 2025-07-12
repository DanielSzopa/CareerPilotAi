using CareerPilotAi.Core;

namespace CareerPilotAi.Prompts.GenerateInterviewQuestions;

public class GenerateInterviewQuestionsPromptOutputModel
{
    public string OutputStatus { get; set; }
    public string OutputFeedbackMessage { get; set; }
    public List<InterviewQuestionPromptOutputModel> InterviewQuestions { get; set; }
}

public class InterviewQuestionPromptOutputModel
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Guide { get; set; }
    public bool IsActive { get; set; }
}