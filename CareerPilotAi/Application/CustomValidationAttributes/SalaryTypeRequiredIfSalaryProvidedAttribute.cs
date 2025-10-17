using CareerPilotAi.ViewModels.JobApplication;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Application.CustomValidationAttributes;

/// <summary>
/// Validates that SalaryType is required if either SalaryMin or SalaryMax is provided.
/// This attribute should be applied to the class level.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SalaryTypeRequiredIfSalaryProvidedAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "Salary type is required when salary is specified.";

    public SalaryTypeRequiredIfSalaryProvidedAttribute() : base(DefaultErrorMessage)
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var model = validationContext.ObjectInstance;
        var salaryMinProperty = model?.GetType().GetProperty(nameof(CreateJobApplicationViewModel.SalaryMin));
        var salaryMaxProperty = model?.GetType().GetProperty(nameof(CreateJobApplicationViewModel.SalaryMax));
        var salaryTypeProperty = model?.GetType().GetProperty(nameof(CreateJobApplicationViewModel.SalaryType));

        if (salaryMinProperty is null || salaryMaxProperty is null || salaryTypeProperty is null)
        {
            return ValidationResult.Success;
        }

        var salaryMin = salaryMinProperty.GetValue(model) as decimal?;
        var salaryMax = salaryMaxProperty.GetValue(model) as decimal?;
        var salaryType = salaryTypeProperty.GetValue(model) as string;

        // Check if any salary value is provided
        var hasSalary = salaryMin.HasValue || salaryMax.HasValue;

        // If salary is provided, SalaryType must not be empty
        if (hasSalary && string.IsNullOrEmpty(salaryType))
        {
            return new ValidationResult(
                ErrorMessage,
                new[] { nameof(CreateJobApplicationViewModel.SalaryType) });
        }

        return ValidationResult.Success;
    }
}
