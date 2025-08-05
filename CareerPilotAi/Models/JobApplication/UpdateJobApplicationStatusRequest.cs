using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Models.JobApplication;

public class UpdateJobApplicationStatusRequest
{
    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = string.Empty;
}
