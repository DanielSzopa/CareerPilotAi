namespace CareerPilotAi.ViewModels.JobApplication;

public class JobApplicationCardViewModel
{
    public Guid JobApplicationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string WorkMode { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string? SalaryType { get; set; }
    public string? SalaryPeriod { get; set; }
    public List<string> SkillsTop { get; set; } = new();
    public int SkillsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}