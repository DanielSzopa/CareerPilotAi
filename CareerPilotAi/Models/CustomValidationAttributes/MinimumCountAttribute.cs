using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.CustomValidationAttributes
{
    public class MinimumCountAttribute : ValidationAttribute
    {
        private readonly int _minimumCount;

        public MinimumCountAttribute(int minimumCount)
        {
            _minimumCount = minimumCount;
            ErrorMessage = $"At least {minimumCount} item(s) are required.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            if (value is IList list)
            {
                if (list.Count < _minimumCount)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else if (value is IEnumerable enumerable)
            {
                var count = enumerable.Cast<object>().Count();
                if (count < _minimumCount)
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
} 