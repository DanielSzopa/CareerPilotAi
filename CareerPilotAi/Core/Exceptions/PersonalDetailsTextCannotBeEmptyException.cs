namespace CareerPilotAi.Core.Exceptions;

internal class PersonalDetailsTextCannotBeEmptyException : Exception
{
    public PersonalDetailsTextCannotBeEmptyException() : base("Personal Details Text cannot be null or empty")
    {

    }
}