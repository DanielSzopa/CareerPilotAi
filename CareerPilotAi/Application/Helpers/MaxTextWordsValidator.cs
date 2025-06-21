namespace CareerPilotAi.Application.Helpers;

internal static class MaxTextWordsValidator
{
    internal static bool ValidateText(string validationText, int maxWords)
    {
        if(string.IsNullOrWhiteSpace(validationText))
            return false;

        var wordCount = validationText.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

        if (wordCount > maxWords)
            return false;

        return true;
    }
}