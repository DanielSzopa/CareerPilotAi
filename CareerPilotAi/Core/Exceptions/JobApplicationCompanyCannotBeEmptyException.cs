namespace CareerPilotAi.Core.Exceptions;

internal class JobApplicationCompanyCannotBeEmptyException : Exception
{
    internal JobApplicationCompanyCannotBeEmptyException() : base("Company name cannot be empty")
    {
    }
}
