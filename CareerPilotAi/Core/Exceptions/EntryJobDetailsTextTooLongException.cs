namespace CareerPilotAi.Core.Exceptions;

internal class EntryJobDetailsTextTooLongException : Exception
{
    public EntryJobDetailsTextTooLongException(int maxWords) : base($"EntryJobDetails Text cannot contain more than {maxWords} words")
    {

    }
}