using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.SaveInterviewPreparationContent;

public record SaveInterviewPreparationContentResponse(bool IsSuccess, string? ErrorMessage, ProblemDetails? ProblemDetails);
