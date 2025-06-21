using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.UpdateJobDescription;

public record UpdateJobDescriptionResponse(bool IsSuccess, ProblemDetails? ProblemDetails);

