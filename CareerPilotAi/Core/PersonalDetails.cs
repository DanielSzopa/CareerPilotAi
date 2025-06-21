using CareerPilotAi.Core.Exceptions;

namespace CareerPilotAi.Core;

internal class PersonalDetails
{
    internal PersonalDetails(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new PersonalDetailsTextCannotBeEmptyException();
        }

        Text = text;
    }

    internal string Text { get; }
}
