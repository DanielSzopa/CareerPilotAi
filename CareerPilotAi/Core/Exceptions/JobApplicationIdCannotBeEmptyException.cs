namespace CareerPilotAi.Core.Exceptions;

internal class JobApplicationIdCannotBeEmptyException : Exception
{
    public JobApplicationIdCannotBeEmptyException() : base("JobApplicationId cannot be empty.")
    {

    }
}
