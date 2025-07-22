using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.Models.Authentication
{
    public class LoginViewModel
    {
        [EnhancedEmail]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        
        public string? ReturnUrl { get; set; }
        public string? Message { get; set; }
    }
} 