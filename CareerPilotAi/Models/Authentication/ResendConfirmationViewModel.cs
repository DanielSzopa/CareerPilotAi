using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.Models.Authentication;

public class ResendConfirmationViewModel
{
    [Required]
    [EnhancedEmail]
    [MaxLength(254)]
    public string Email { get; set; } = string.Empty;
}
