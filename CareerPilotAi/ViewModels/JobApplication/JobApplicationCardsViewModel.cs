namespace CareerPilotAi.ViewModels.JobApplication;

public class JobApplicationCardsViewModel
{
    public List<JobApplicationCardViewModel> Cards { get; set; } = new();
    
    public int ResultCount { get; set; }
    
    public JobApplicationFiltersViewModel Filters { get; set; } = new();
}