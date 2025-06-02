using CareerPilotAi.Application.Helpers;

namespace CareerPilotAi.Core;

internal class JobApplication
{
    internal Guid JobApplicationId { get; private set; }
    internal string UserId { get; private set; }
    internal EntryJobDetails EntryJobDetails { get; private set; }
    internal PersonalDetails? PersonalDetails { get; private set; }

    internal JobApplication(Guid jobApplicationId, string userId, EntryJobDetails entryJobDetails, PersonalDetails? personalDetails)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("UserId is required", nameof(userId));
        }

        if (Guid.Empty == jobApplicationId)
        {
            throw new ArgumentException("JobApplicationId is required", nameof(jobApplicationId));
        }

        if (entryJobDetails == null)
        {
            throw new ArgumentException("EntryJobDetails is required", nameof(entryJobDetails));
        }

        UserId = userId;
        JobApplicationId = jobApplicationId;
        EntryJobDetails = entryJobDetails;
        PersonalDetails = personalDetails;
    }
}

internal class EntryJobDetails
{
    internal string? Url { get; }
    internal string Text { get; }

    internal EntryJobDetails(string? url, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException($"{nameof(EntryJobDetails)} Text cannot be null or empty", nameof(text));

        if(!string.IsNullOrWhiteSpace(url))
            UrlValidator.ValidateUrl(url);

        var validationText = text.ToString()!;
        var wordCount = validationText.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

        if (wordCount > 10000)
            throw new ArgumentException($"{nameof(EntryJobDetails)} Text cannot contain more than 10,000 words", nameof(text));

        Url = url;
        Text = text;
    }
}

internal class PersonalDetails
{
    internal PersonalDetails(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Text cannot be null or empty", nameof(text));
        }

        Text = text;
    }

    internal string Text { get; }
}
