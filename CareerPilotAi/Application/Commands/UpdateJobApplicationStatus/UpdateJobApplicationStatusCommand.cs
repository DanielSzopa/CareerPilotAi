using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusCommand(Guid JobApplicationId, string Status) : ICommand;
