namespace CareerPilotAi.Core.Exceptions;

internal class SingleInterviewQuestionCannotBeNullException : Exception
{
    public SingleInterviewQuestionCannotBeNullException() : base("Single interview question cannot be null.")
    {

    }
}