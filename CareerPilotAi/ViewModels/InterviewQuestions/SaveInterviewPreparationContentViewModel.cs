using CareerPilotAi.Application.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.ViewModels.InterviewQuestions;

public class SaveInterviewPreparationContentViewModel
{
    [Required(ErrorMessage = "Preparation content is required.")]
    [MaxWords(5000)]
    public string PreparationContent { get; set; } = string.Empty;
}