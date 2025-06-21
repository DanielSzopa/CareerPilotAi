using CareerPilotAi.Application.Helpers;
using CareerPilotAi.Core.Exceptions;

namespace CareerPilotAi.Core;

internal class EntryJobDetails
{
    internal string Text { get; }
    internal string Title { get; }
    internal string Company { get; }
    internal string? URL { get; }

    internal EntryJobDetails(string title, string company, string text, string? url)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new JobApplicationTitleCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(company))
            throw new JobApplicationCompanyCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(text))
            throw new EntryJobDetailsTextCannotBeEmptyException();

        var maxWords = 5000;
        if (!MaxTextWordsValidator.ValidateText(text, maxWords))
            throw new EntryJobDetailsTextTooLongException(maxWords);
            
        if (!string.IsNullOrWhiteSpace(url))
            UrlValidator.ValidateUrl(url);

        Title = title;
        Company = company;
        Text = text;
        URL = url;
    }
}
