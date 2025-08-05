namespace CareerPilotAi.Core
{
    public record ApplicationStatus
    {
        private const string _draft = "Draft";
        private const string _rejected = "Rejected";
        private const string _submitted = "Submitted";
        private const string _interviewScheduled = "Interview Scheduled";
        private const string _waitingForOffer = "Waiting for offer";
        private const string _receivedOffer = "Received offer";
        private const string _noContact = "No contact";
        public static readonly IReadOnlyList<string> ValidStatuses = new List<string>
        {
            _draft,
            _rejected,
            _submitted,
            _interviewScheduled,
            _waitingForOffer,
            _receivedOffer,
            _noContact
        }.AsReadOnly();

        public const string DefaultStatus = _draft;
        public static readonly ApplicationStatus Draft = new ApplicationStatus(_draft);
        public static readonly ApplicationStatus Rejected = new ApplicationStatus(_rejected);
        public static readonly ApplicationStatus Submitted = new ApplicationStatus(_submitted);
        public static readonly ApplicationStatus InterviewScheduled = new ApplicationStatus(_interviewScheduled);
        public static readonly ApplicationStatus WaitingForOffer = new ApplicationStatus(_waitingForOffer);
        public static readonly ApplicationStatus ReceivedOffer = new ApplicationStatus(_receivedOffer);
        public static readonly ApplicationStatus NoContact = new ApplicationStatus(_noContact);
        public string Status { get; }

        public ApplicationStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Status cannot be null or empty.", nameof(status));
            }

            if (!ValidStatuses.Contains(status))
                throw new ArgumentException("Invalid status.", nameof(status));

            Status = status;
        }
    }
}