namespace CareerPilotAi.Application.Commands.Abstractions;

public interface ICommand
{
}

public interface ICommand<TResponse> : ICommand
{
}
