using CareerPilotAi.ViewModels.JobApplication;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Application.CustomValidationAttributes;

/// <summary>
/// Validates that the maximum salary is greater than or equal to the minimum salary.
/// This attribute should be applied to the class level, not on individual properties.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ValidSalaryRangeAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "Maximum salary must be greater than or equal to minimum salary.";

    public ValidSalaryRangeAttribute() : base(DefaultErrorMessage)
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var model = validationContext.ObjectInstance;
        var salaryMinProperty = model?.GetType().GetProperty(nameof(CreateJobApplicationViewModel.SalaryMin));
        var salaryMaxProperty = model?.GetType().GetProperty(nameof(CreateJobApplicationViewModel.SalaryMax));

        if (salaryMinProperty is null || salaryMaxProperty is null)
        {
            return ValidationResult.Success;
        }

        var salaryMin = salaryMinProperty.GetValue(model) as decimal?;
        var salaryMax = salaryMaxProperty.GetValue(model) as decimal?;

        // Only validate if both values are provided
        if (!salaryMin.HasValue || !salaryMax.HasValue)
        {
            return ValidationResult.Success;
        }

        if (salaryMax < salaryMin)
        {
            return new ValidationResult(
                ErrorMessage,
                new[] { nameof(CreateJobApplicationViewModel.SalaryMax) });
        }

        return ValidationResult.Success;
    }
}
