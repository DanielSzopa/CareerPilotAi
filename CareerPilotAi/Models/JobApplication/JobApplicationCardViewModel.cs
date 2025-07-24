namespace CareerPilotAi.Models.JobApplication;

public class JobApplicationCardViewModel
{
    public Guid JobApplicationId { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public CardDate CardDate { get; set; }

    public string Status { get; set; }
}

public class CardDate
{
    public string Value { get; }

    public CardDate(DateTime dateTime)
    {
        if(dateTime == default)
        {
            throw new ArgumentException("DateTime cannot be default value.", nameof(dateTime));
        }

        Value = dateTime.ToString("dd.MM.yyyy HH:mm");
    }
}