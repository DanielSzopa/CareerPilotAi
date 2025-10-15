using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Core;

namespace CareerPilotAi.ViewModels.JobApplication;

public class SkillViewModel
{
    [Required(ErrorMessage = "Skill name is required")]
    [MinLength(2, ErrorMessage = "Skill name must be at least 2 characters")]
    [MaxLength(50, ErrorMessage = "Skill name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Skill level is required")]
    [AllowedValues(SkillLevel.NiceToHave, SkillLevel.Regular, SkillLevel.Advanced, SkillLevel.Master)]
    public string Level { get; set; } = string.Empty;
}

