using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;
using CareerPilotAi.Core;

namespace CareerPilotAi.ViewModels.JobApplication;

[ValidSalaryRange]
[SalaryTypeRequiredIfSalaryProvided]
public class CreateJobApplicationViewModel
{
    [Required(ErrorMessage = "Company name is required")]
    [MinLength(2, ErrorMessage = "Company name must be at least 2 characters")]
    [MaxLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
    [Display(Name = "Company")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position is required")]
    [MinLength(2, ErrorMessage = "Position must be at least 2 characters")]
    [MaxLength(100, ErrorMessage = "Position cannot exceed 100 characters")]
    [Display(Name = "Position")]
    public string Position { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job description is required")]
    [MinWords(50, ErrorMessage = "Job description must contain at least 50 words")]
    [MaxWords(5000, ErrorMessage = "Job description cannot exceed 5000 words")]
    [Display(Name = "Description")]
    public string JobDescription { get; set; } = string.Empty;

    [MinimumCount(0)]
    [MaximumCount(20, ErrorMessage = "You can add maximum 20 skills")]
    public List<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();

    [Required(ErrorMessage = "Experience level is required")]
    [AllowedValues(Core.ExperienceLevel.Junior, Core.ExperienceLevel.Mid, Core.ExperienceLevel.Senior, Core.ExperienceLevel.NotSpecified)]
    [Display(Name = "Experience")]
    public string ExperienceLevel { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required")]
    [MinLength(2, ErrorMessage = "Location must be at least 2 characters")]
    [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Work mode is required")]
    [AllowedValues(Core.WorkMode.Remote, Core.WorkMode.Hybrid, Core.WorkMode.OnSite)]
    [Display(Name = "Work Mode")]
    public string WorkMode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contract type is required")]
    [AllowedValues(Core.ContractType.B2B, Core.ContractType.FTE, Core.ContractType.Other)]
    [Display(Name = "Contract Type")]
    public string ContractType { get; set; } = string.Empty;

    [Range(0, 999999, ErrorMessage = "Minimum salary must be between 0 and 999,999")]
    [Display(Name = "Min")]
    public decimal? SalaryMin { get; set; }

    [Range(0, 999999, ErrorMessage = "Maximum salary must be between 0 and 999,999")]
    [Display(Name = "Max")]
    public decimal? SalaryMax { get; set; }

    [AllowedValues(Core.SalaryType.Gross, Core.SalaryType.Net)]
    [Display(Name = "Type")]
    public string? SalaryType { get; set; }

    [AllowedValues(Core.SalaryPeriodType.Monthly, Core.SalaryPeriodType.Daily, Core.SalaryPeriodType.Hourly, Core.SalaryPeriodType.Yearly)]
    [Display(Name = "Period")]
    public string? SalaryPeriod { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL")]
    [MaxLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
    [Display(Name = "Url (optional)")]
    public string? JobUrl { get; set; }

    [Required]
    [AllowedValues(ApplicationStatus.DefaultStatus, "Draft", "Submitted", "Interview Scheduled", "Waiting for offer", "Received offer", "Rejected", "No contact")]
    public string Status { get; set; } = ApplicationStatus.DefaultStatus;
}

