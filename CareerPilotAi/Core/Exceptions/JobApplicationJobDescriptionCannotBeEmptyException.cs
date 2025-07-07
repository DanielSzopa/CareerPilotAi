namespace CareerPilotAi.Core.Exceptions;

internal class JobApplicationJobDescriptionCannotBeEmptyException : Exception
{
    public JobApplicationJobDescriptionCannotBeEmptyException() : base("Job description cannot be empty.")
    {

    }
}