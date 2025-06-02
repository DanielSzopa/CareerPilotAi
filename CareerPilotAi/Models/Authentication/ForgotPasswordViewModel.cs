using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.Authentication
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
} 