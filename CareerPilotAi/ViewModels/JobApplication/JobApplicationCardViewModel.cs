using CareerPilotAi.Application.Extensions;

namespace CareerPilotAi.ViewModels.JobApplication;

public class JobApplicationCardViewModel
{
    public Guid JobApplicationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = string.Empty;
}