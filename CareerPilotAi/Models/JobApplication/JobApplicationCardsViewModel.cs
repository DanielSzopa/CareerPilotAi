namespace CareerPilotAi.Models.JobApplication;

public class JobApplicationCardsViewModel
{
    public List<JobApplicationCardViewModel> Cards { get; set; }

    public int TotalApplications { get; set; }
    public int DraftStatusQuantity { get; set; }
    public int RejectedStatusQuantity { get; set; }
    public int SubmittedStatusQuantity { get; set; }
    public int InterviewScheduledStatusQuantity { get; set; }
    public int WaitingForOfferStatusQuantity { get; set; }
    public int ReceivedOfferStatusQuantity { get; set; }
    public int NoContactStatusQuantity { get; set; }
}