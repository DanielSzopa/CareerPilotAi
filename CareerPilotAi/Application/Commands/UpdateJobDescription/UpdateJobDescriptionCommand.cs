using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.UpdateJobDescription;

public record UpdateJobDescriptionCommand(Guid JobApplicationId, string JobDescription) : ICommand;

