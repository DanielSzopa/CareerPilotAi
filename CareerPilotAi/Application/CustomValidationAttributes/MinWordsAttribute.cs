using CareerPilotAi.Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Application.CustomValidationAttributes;

public class MinWordsAttribute : ValidationAttribute
{
    private readonly int _minWords;

    public MinWordsAttribute(int minWords)
    {
        _minWords = minWords;
        ErrorMessage = $"The field must contain at least {minWords} words.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult(ErrorMessage);
        }

        var text = value.ToString() ?? string.Empty;
        var wordCount = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

        if (wordCount >= _minWords)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }
}

