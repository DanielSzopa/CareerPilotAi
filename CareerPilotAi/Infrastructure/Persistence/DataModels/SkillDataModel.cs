namespace CareerPilotAi.Infrastructure.Persistence.DataModels;

internal class SkillDataModel
{
    internal Guid SkillId { get; set; }
    internal Guid JobApplicationId { get; set; }
    internal string Name { get; set; } = string.Empty;
    internal string Level { get; set; } = string.Empty;

    // Navigation property
    internal JobApplicationDataModel JobApplication { get; set; } = null!;
}

