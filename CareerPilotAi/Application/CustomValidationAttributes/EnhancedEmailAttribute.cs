using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CareerPilotAi.Application.CustomValidationAttributes;

/// <summary>
/// Enhanced email validation attribute that uses a comprehensive regex pattern
/// to validate email addresses with better support for edge cases, international emails,
/// and RFC 5322 compliance compared to the built-in EmailAddress attribute.
/// This attribute requires a valid email address and does not allow null, empty, or whitespace values.
/// </summary>
public class EnhancedEmailAttribute : ValidationAttribute, IClientModelValidator
{
    private const string DefaultErrorMessage = "Please enter a valid email address.";
    
    // Enhanced regex pattern converted from JavaScript to C# with proper escaping
    // Handles: quoted strings, special characters, IP addresses, international domains
    private static readonly Regex EmailRegex = new(
        @"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100)
    );

    public EnhancedEmailAttribute() : base(DefaultErrorMessage)
    {
    }

    public EnhancedEmailAttribute(string errorMessage) : base(errorMessage)
    {
    }

    /// <summary>
    /// Validates the email address using the enhanced regex pattern.
    /// Unlike the built-in EmailAddress attribute, this validation fails for null, empty, or whitespace values,
    /// eliminating the need for a separate [Required] attribute.
    /// </summary>
    /// <param name="value">The email address to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>ValidationResult indicating success or failure</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Null, empty, or whitespace values are not allowed
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        var email = value.ToString()?.Trim();
        
        // Basic length and format checks - empty after trim is invalid
        if (string.IsNullOrEmpty(email))
        {
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        // Check for obvious malicious input patterns
        if (email.Length > 254 || email.Contains('\0') || email.Contains('\r') || email.Contains('\n'))
        {
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        try
        {
            // Validate using the enhanced regex pattern
            if (EmailRegex.IsMatch(email))
            {
                return ValidationResult.Success;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            // Log timeout and return invalid
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
    }

    /// <summary>
    /// Adds client-side validation support for the enhanced email attribute.
    /// </summary>
    /// <param name="context">The client model validation context</param>
    public void AddValidation(ClientModelValidationContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // Add client-side validation rule
        context.Attributes.TryAdd("data-val", "true");
        context.Attributes.TryAdd("data-val-enhancedemail", GetErrorMessage(context.ModelMetadata.DisplayName));
        
        // Add the regex pattern for client-side validation
        var clientPattern = @"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";
        context.Attributes.TryAdd("data-val-enhancedemail-pattern", clientPattern);
    }

    /// <summary>
    /// Gets the formatted error message for the validation failure.
    /// </summary>
    /// <param name="displayName">The display name of the field being validated</param>
    /// <returns>Formatted error message</returns>
    private string GetErrorMessage(string? displayName)
    {
        return string.IsNullOrEmpty(ErrorMessage) 
            ? DefaultErrorMessage 
            : string.Format(ErrorMessage, displayName ?? "Email");
    }
}
