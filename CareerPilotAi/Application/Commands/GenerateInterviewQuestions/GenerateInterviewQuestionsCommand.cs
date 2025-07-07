using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.GenerateInterviewQuestions;

public record GenerateInterviewQuestionsCommand(Guid JobApplicationId) : ICommand;
