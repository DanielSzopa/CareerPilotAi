using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CareerPilotAi.ViewModels.Authentication;

public class UserSettingsViewModel
{
    [Display(Name = "Email")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Time zone is required.")]
    [Display(Name = "Time Zone")]
    public string TimeZoneId { get; set; }

    public IEnumerable<SelectListItem> TimeZones { get; set; } = Enumerable.Empty<SelectListItem>();
}

