using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.InterviewQuestions;

public class GenerateInterviewQuestionsRequest
{
    [Required(ErrorMessage = "Questions count is required.")]
    [Range(1, 10, ErrorMessage = "Questions count must be between 1 and 10.")]
    public int QuestionsCount { get; set; } = 5;
}
