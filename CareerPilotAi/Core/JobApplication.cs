using CareerPilotAi.Application.Helpers;
using CareerPilotAi.Core.Exceptions;

namespace CareerPilotAi.Core;

internal class JobApplication
{
    private const int _maxWords = 5000;
    internal Guid JobApplicationId { get; private set; }
    internal string UserId { get; private set; }
    internal string JobDescription { get; private set; }
    internal string Title { get; private set; }
    internal string Company { get; private set; }
    internal string? URL { get; private set; }

    internal JobApplication(Guid jobApplicationId, string userId, string title, string company, string text, string? url)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new JobApplicationUserIdCannotBeEmptyException();

        if (Guid.Empty == jobApplicationId)
            throw new JobApplicationIdCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(title))
            throw new JobApplicationTitleCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(company))
            throw new JobApplicationCompanyCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(text))
            throw new EntryJobDetailsTextCannotBeEmptyException();

        if (!MaxTextWordsValidator.ValidateText(text, _maxWords))
            throw new EntryJobDetailsTextTooLongException(_maxWords);

        if (!string.IsNullOrWhiteSpace(url))
            UrlValidator.ValidateUrl(url);

        UserId = userId;
        JobApplicationId = jobApplicationId;
        Title = title;
        Company = company;
        JobDescription = text;
        URL = url;
    }

    internal void UpdateJobDescription(string jobDescriptionText)
    {
        if (string.IsNullOrWhiteSpace(jobDescriptionText))
            throw new EntryJobDetailsTextCannotBeEmptyException();

        if (!MaxTextWordsValidator.ValidateText(jobDescriptionText, _maxWords))
            throw new EntryJobDetailsTextTooLongException(_maxWords);

        JobDescription = jobDescriptionText;
    }
}

