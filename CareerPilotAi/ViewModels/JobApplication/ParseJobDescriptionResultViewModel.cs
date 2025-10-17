namespace CareerPilotAi.ViewModels.JobApplication;

public class ParseJobDescriptionResultViewModel
{
    public bool Success { get; set; }
    public string FeedbackMessage { get; set; } = string.Empty;
    public CreateJobApplicationViewModel? Data { get; set; }
}