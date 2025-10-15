using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Application.CustomValidationAttributes;

public class MaximumCountAttribute : ValidationAttribute
{
    private readonly int _maximumCount;

    public MaximumCountAttribute(int maximumCount)
    {
        _maximumCount = maximumCount;
        ErrorMessage = $"Maximum {maximumCount} item(s) are allowed.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Allow null values
        }

        if (value is IList list)
        {
            if (list.Count > _maximumCount)
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        else if (value is IEnumerable enumerable)
        {
            var count = enumerable.Cast<object>().Count();
            if (count > _maximumCount)
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        else
        {
            return new ValidationResult("Value must be a collection.");
        }

        return ValidationResult.Success;
    }
}

