using CareerPilotAi.Application.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.Authentication
{
    public class LoginViewModel
    {
        [Required]
        [EnhancedEmail]
        [MaxLength(254)]
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