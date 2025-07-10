using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.PrepareInterviewPreparationContent;

public record PrepareInterviewPreparationContentCommand(Guid JobApplicationId) : ICommand;
