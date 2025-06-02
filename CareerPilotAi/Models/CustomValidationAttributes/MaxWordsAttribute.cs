using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.CustomValidationAttributes
{
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
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var text = value.ToString()!;
            var wordCount = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

            if (wordCount > _maxWords)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
} 