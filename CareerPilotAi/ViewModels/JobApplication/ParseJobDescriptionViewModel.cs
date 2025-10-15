using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.ViewModels.JobApplication;

public class ParseJobDescriptionViewModel
{
    [Required(ErrorMessage = "Job description text is required")]
    [MaxWords(5000, ErrorMessage = "Job description cannot exceed 5000 words")]
    public string JobDescriptionText { get; set; } = string.Empty;
}

