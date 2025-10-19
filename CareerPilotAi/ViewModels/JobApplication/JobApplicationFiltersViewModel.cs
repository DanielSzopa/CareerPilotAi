using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.ViewModels.JobApplication;

public class JobApplicationFiltersViewModel
{
    [MaxLength(100)]
    public string? SearchTerm { get; set; }

    public List<string>? Statuses { get; set; }

    [Range(0, 999999)]
    public decimal? MinSalary { get; set; }

    [Range(0, 999999)]
    public decimal? MaxSalary { get; set; }

    public string? SalaryType { get; set; }

    public string? SalaryPeriod { get; set; }

    [MaxLength(100)]
    public string? Location { get; set; }

    public List<string>? WorkModes { get; set; }

    public List<string>? ExperienceLevels { get; set; }

    public string? SortBy { get; set; }
}

