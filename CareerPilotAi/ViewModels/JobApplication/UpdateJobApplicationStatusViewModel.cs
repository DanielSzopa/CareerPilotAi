using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.ViewModels.JobApplication;

public class UpdateJobApplicationStatusViewModel
{
    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = string.Empty;
}
