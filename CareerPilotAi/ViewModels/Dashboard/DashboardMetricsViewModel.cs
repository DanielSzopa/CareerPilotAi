namespace CareerPilotAi.ViewModels.Dashboard
{
    public class DashboardMetricsViewModel
    {
        public int TotalApplications { get; set; }

        public int DraftCount { get; set; }

        public int SubmittedCount { get; set; }

        public int InterviewScheduledCount { get; set; }

        public int WaitingForOfferCount { get; set; }

        public int ReceivedOfferCount { get; set; }

        public int RejectedCount { get; set; }

        public int NoContactCount { get; set; }
    }
}

