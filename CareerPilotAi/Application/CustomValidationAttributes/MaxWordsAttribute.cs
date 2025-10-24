using CareerPilotAi.Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Application.CustomValidationAttributes;

public class MaxWordsAttribute : ValidationAttribute
{
    private readonly int _maxWords;

    public MaxWordsAttribute(int maxWords)
    {
        _maxWords = maxWords;
        ErrorMessage = $"The field cannot contain more than {maxWords} words.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value?.ToString()))
        {
            return ValidationResult.Success;
        }

        if (MaxTextWordsValidator.ValidateText(value?.ToString()!, _maxWords))
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult(ErrorMessage);
        }
    }
}
