using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusResponse(bool IsSuccess, ProblemDetails? ProblemDetails);
