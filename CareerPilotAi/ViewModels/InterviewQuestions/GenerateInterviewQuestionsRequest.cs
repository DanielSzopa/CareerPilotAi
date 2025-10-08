using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.ViewModels.InterviewQuestions;

public class GenerateInterviewQuestionsRequest
{
    [Required(ErrorMessage = "Questions count is required.")]
    [Range(1, 10, ErrorMessage = "Questions count must be between 1 and 10.")]
    public byte QuestionsCount { get; set; }
}
