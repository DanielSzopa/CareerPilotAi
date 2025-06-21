namespace CareerPilotAi.Core.Exceptions;

internal class EntryJobDetailsTextCannotBeEmptyException : Exception
{
    public EntryJobDetailsTextCannotBeEmptyException() : base("EntryJobDetails Text cannot be null or empty")
    {

    }
}