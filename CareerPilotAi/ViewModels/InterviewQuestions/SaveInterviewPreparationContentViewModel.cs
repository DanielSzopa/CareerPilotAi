using CareerPilotAi.Application.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.ViewModels.InterviewQuestions;

public class SaveInterviewPreparationContentViewModel
{
    [MaxWords(5000)]
    public string PreparationContent { get; set; } = string.Empty;
}