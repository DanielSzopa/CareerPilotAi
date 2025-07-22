using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.Infrastructure.Email;

public class SendGridAppSettings
{
    [Required]
    public string? ApiKey { get; set; }

    [EnhancedEmail]
    public string? FromEmail { get; set; }

    [Required]
    public string? RegistrationVerificationTemplateId { get; set; }

    [Required]
    public string? PasswordResetTemplateId { get; set; }
}

