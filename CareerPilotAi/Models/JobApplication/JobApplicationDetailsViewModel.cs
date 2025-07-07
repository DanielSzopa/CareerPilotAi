using CareerPilotAi.Application.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.JobApplication
{
    public class JobApplicationDetailsViewModel
    {
        public Guid? JobApplicationId { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job title is required")]
        [MaxLength(200, ErrorMessage = "Job title cannot exceed 200 characters")]
        public string JobTitle { get; set; } = string.Empty;

        [MaxWords(5000, ErrorMessage = "Job description cannot exceed 5000 words")]
        public string JobDescription { get; set; } = string.Empty;
    }
}
