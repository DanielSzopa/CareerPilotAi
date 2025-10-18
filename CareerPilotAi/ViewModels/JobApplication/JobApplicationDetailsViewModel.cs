using CareerPilotAi.Application.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.ViewModels.JobApplication
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

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;

        // New properties
        [Required(ErrorMessage = "Location is required")]
        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; } = string.Empty;

        public string WorkMode { get; set; } = string.Empty;

        public string ContractType { get; set; } = string.Empty;

        public string ExperienceLevel { get; set; } = string.Empty;

        [Range(0, 999999, ErrorMessage = "Salary must be between 0 and 999,999")]
        public decimal? SalaryMin { get; set; }

        [Range(0, 999999, ErrorMessage = "Salary must be between 0 and 999,999")]
        public decimal? SalaryMax { get; set; }

        public string? SalaryType { get; set; }

        public string? SalaryPeriod { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [MaxLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        public string? JobUrl { get; set; }

        public List<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
