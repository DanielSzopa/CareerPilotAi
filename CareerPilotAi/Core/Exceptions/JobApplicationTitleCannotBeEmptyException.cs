namespace CareerPilotAi.Core.Exceptions;

internal class JobApplicationTitleCannotBeEmptyException : Exception
{
    public JobApplicationTitleCannotBeEmptyException() : base("Title cannot be empty.")
    {

    }
}