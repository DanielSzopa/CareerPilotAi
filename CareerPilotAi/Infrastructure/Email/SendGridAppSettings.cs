using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Infrastructure.Email;

public class SendGridAppSettings
{
    [Required]
    public string? ApiKey { get; set; }

    [Required]
    [EmailAddress]
    public string? FromEmail { get; set; }

    [Required]
    public string? RegistrationVerificationTemplateId { get; set; }

    [Required]
    public string? PasswordResetTemplateId { get; set; }
}

