using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.SaveInterviewPreparationContent;

public record SaveInterviewPreparationContentCommand(Guid JobApplicationId, string PreparationContent) : ICommand;
