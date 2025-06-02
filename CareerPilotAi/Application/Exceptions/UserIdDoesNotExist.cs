namespace CareerPilotAi.Application.Exceptions;

internal class UserIdDoesNotExist : Exception
{
    public UserIdDoesNotExist() : base("UserId doesn't exist. It's null or empty.")
    {
        
    }
}
