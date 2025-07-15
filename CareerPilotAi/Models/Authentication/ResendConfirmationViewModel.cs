using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.Authentication;

public class ResendConfirmationViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}
