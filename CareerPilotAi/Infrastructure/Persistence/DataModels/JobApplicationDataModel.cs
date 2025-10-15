using Microsoft.AspNetCore.Identity;

namespace CareerPilotAi.Infrastructure.Persistence.DataModels;

internal class JobApplicationDataModel
{
    internal Guid JobApplicationId { get; set; }
    internal IdentityUser User { get; set; }
    internal string UserId { get; set; }
    internal string Title { get; set; }
    internal string Company { get; set; }
    internal string? Url { get; set; }
    internal string JobDescription { get; set; }
    internal string Status { get; set; }
    internal DateTime CreatedAt { get; set; }
    internal DateTime UpdatedAt { get; set; }
    
    // New properties for extended job application details
    internal string ExperienceLevel { get; set; } = string.Empty;
    internal string Location { get; set; } = string.Empty;
    internal string WorkMode { get; set; } = string.Empty;
    internal string ContractType { get; set; } = string.Empty;
    internal decimal? SalaryMin { get; set; }
    internal decimal? SalaryMax { get; set; }
    internal string? SalaryType { get; set; }
    internal string? SalaryPeriod { get; set; }
    
    // Navigation properties
    internal ICollection<InterviewQuestionDataModel> InterviewQuestions { get; set; } = new List<InterviewQuestionDataModel>();
    internal ICollection<SkillDataModel> Skills { get; set; } = new List<SkillDataModel>();
}
