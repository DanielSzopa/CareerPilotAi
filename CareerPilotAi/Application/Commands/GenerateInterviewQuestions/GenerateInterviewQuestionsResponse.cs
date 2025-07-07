using CareerPilotAi.Prompts.GenerateInterviewQuestions;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.GenerateInterviewQuestions;

public record GenerateInterviewQuestionsResponse(bool IsSuccess, GenerateInterviewQuestionsPromptOutputModel outputModel, ProblemDetails? ProblemDetails);
