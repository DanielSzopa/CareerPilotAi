using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.DeleteJobApplication;

public record DeleteJobApplicationCommand(Guid JobApplicationId) : ICommand;