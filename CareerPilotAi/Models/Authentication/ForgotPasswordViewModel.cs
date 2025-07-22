using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.Models.Authentication
{
    public class ForgotPasswordViewModel
    {
        [EnhancedEmail]
        public required string Email { get; set; }
    }
} 