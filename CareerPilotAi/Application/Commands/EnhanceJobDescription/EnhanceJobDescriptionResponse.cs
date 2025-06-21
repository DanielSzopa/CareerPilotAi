using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.EnhanceJobDescription;

public record EnhanceJobDescriptionResponse(bool IsSuccess, string EnhancedJobDescription, ProblemDetails? ProblemDetails);
