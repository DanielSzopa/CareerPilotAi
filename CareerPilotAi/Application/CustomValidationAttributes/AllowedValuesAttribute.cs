using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CareerPilotAi.Application.CustomValidationAttributes;

public class AllowedValuesAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly string[] _allowedValues;

    public AllowedValuesAttribute(params string[] allowedValues)
    {
        _allowedValues = allowedValues;
        ErrorMessage = $"The field must be one of the following values: {string.Join(", ", allowedValues)}.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success; // Allow empty values, use [Required] if needed
        }

        var stringValue = value.ToString()!;
        
        if (_allowedValues.Contains(stringValue))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-allowedvalues", ErrorMessage ?? "Please select a valid option from the list.");
        context.Attributes.Add("data-val-allowedvalues-values", string.Join(",", _allowedValues));
    }
}
