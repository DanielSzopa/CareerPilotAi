using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.ViewModels.Authentication
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EnhancedEmail]
        [MaxLength(254)]
        public required string Email { get; set; }
    }
} 