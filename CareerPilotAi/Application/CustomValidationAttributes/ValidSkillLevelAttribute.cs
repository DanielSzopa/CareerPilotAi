using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Core;

namespace CareerPilotAi.Application.CustomValidationAttributes;

public class ValidSkillLevelAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var level = value.ToString();
        
        if (string.IsNullOrWhiteSpace(level) || !SkillLevel.ValidLevels.Contains(level))
        {
            return new ValidationResult(
                $"Invalid: {level} skill level. Please select one of: {string.Join(", ", SkillLevel.ValidLevels)}");
        }

        return ValidationResult.Success;
    }
}

