namespace CareerPilotAi.Core.Exceptions;

internal class JobApplicationUserIdCannotBeEmptyException : Exception
{
    public JobApplicationUserIdCannotBeEmptyException() : base("UserId cannot be empty.")
    {

    }
}