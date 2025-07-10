using CareerPilotAi.Application.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.InterviewQuestions;

public class SaveInterviewPreparationContentRequest
{
    [Required(ErrorMessage = "Preparation content is required.")]
    [MaxWords(5000)]
    public string PreparationContent { get; set; } = string.Empty;
}