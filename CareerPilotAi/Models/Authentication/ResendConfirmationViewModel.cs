using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.Models.Authentication;

public class ResendConfirmationViewModel
{
    [EnhancedEmail]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}
