using CareerPilotAi.Application.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models;

public class UpdateJobDescriptionViewModel
{
    [Required(ErrorMessage = "JobApplicationId is required.")]
    public Guid JobApplicationId { get; set; }

    [Required(ErrorMessage = "Job description is required.")]
    [MaxWords(5000, ErrorMessage = "Job description cannot exceed 5,000 words.")]
    public string JobDescription { get; set; } = string.Empty;
}