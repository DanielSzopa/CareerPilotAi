using System.Text.Json.Serialization;

namespace CareerPilotAi.Prompts.ParseJobDescription;

public class ParseJobDescriptionOutputModel
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("parsingResult")]
    public string FeedbackMessage { get; set; } // "FullSuccess", "PartialSuccess", "Failed"

    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; }

    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("jobDescription")]
    public string JobDescription { get; set; }

    [JsonPropertyName("skills")]
    public List<Skill> Skills { get; set; }

    [JsonPropertyName("experienceLevel")]
    public string ExperienceLevel { get; set; } // "Junior", "Mid", "Senior", "NotSpecified"

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("workMode")]
    public string WorkMode { get; set; } // "Remote", "Hybrid", "OnSite"

    [JsonPropertyName("contractType")]
    public string ContractType { get; set; } // "B2B", "FTE", "Other"
    
    [JsonPropertyName("salaryMin")]
    public decimal? SalaryMin { get; set; }

    [JsonPropertyName("salaryMax")]
    public decimal? SalaryMax { get; set; }

    [JsonPropertyName("salaryType")]
    public string? SalaryType { get; set; } // "Gross", "Net"

    [JsonPropertyName("salaryPeriod")]
    public string? SalaryPeriod { get; set; } // "Monthly", "Daily", "Hourly", "Yearly"
}

public class Skill
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("level")]
    public string Level { get; set; } // "Nice to have", "Junior", "Regular", "Advanced", "Master"
}

