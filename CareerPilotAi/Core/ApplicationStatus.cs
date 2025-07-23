namespace CareerPilotAi.Core
{
    public record ApplicationStatus
    {
        public const string DefaultStatus = "Draft";
        public static readonly ApplicationStatus Draft = new ApplicationStatus(DefaultStatus);
        public static readonly ApplicationStatus Rejected = new ApplicationStatus("Rejected");
        public static readonly ApplicationStatus Submitted = new ApplicationStatus("Submitted");
        public static readonly ApplicationStatus InterviewScheduled = new ApplicationStatus("Interview Scheduled");
        public static readonly ApplicationStatus WaitingForOffer = new ApplicationStatus("Waiting for offer");
        public static readonly ApplicationStatus ReceivedOffer = new ApplicationStatus("Received offer");
        public static readonly ApplicationStatus NoContact = new ApplicationStatus("No contact");
        public string Status { get; }

        public ApplicationStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Status cannot be null or empty.", nameof(status));
            }

            Status = status;
        }
    }
}