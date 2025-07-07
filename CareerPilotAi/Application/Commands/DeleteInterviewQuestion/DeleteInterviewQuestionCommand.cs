using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.DeleteInterviewQuestion;

public record DeleteInterviewQuestionCommand(Guid InterviewQuestionId) : ICommand;
