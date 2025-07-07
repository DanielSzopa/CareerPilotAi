namespace CareerPilotAi.Models.JobApplication;

internal class JobApplicationCardViewModel
{
    public Guid JobApplicationId { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public CardDate CardDate { get; set; }
}

internal class CardDate
{
    internal string Value { get; }

    internal CardDate(DateTime dateTime)
    {
        if(dateTime == default)
        {
            throw new ArgumentException("DateTime cannot be default value.", nameof(dateTime));
        }

        Value = dateTime.ToString("dd.MM.yyyy HH:mm");
    }
}