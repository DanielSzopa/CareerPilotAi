using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.OpenRouter;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CareerPilotAi.Application.Commands.EnhanceJobDescription;

public class EnhanceJobDescriptionCommandHandler : ICommandHandler<EnhanceJobDescriptionCommand, EnhanceJobDescriptionResponse>
{
    private readonly IUserService _userService;
    private readonly ILogger<EnhanceJobDescriptionCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly OpenRouterService _openRouterService;

    public EnhanceJobDescriptionCommandHandler(IUserService userService, ILogger<EnhanceJobDescriptionCommandHandler> logger, IHttpContextAccessor httpContextAccessor,
        OpenRouterService openRouterService)
    {
        _userService = userService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _openRouterService = openRouterService;
    }
    public async Task<EnhanceJobDescriptionResponse> HandleAsync(EnhanceJobDescriptionCommand command, CancellationToken cancellationToken)
    {
        //Ensure the user is authenticated
        _userService.GetUserIdOrThrowException();

        if (string.IsNullOrWhiteSpace(command.JobDescriptionText))
        {
            _logger.LogError("Job description text is required but was not provided.");
            return new EnhanceJobDescriptionResponse(false, string.Empty, new ProblemDetails()
            {
                Type = HttpStatusCode.BadRequest.ToString(),
                Title = "Invalid Job Description Text",
                Detail = "Job description text is required.",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        var enhancedText = await _openRouterService.EnhanceJobDescriptionAsync(command.JobDescriptionText, cancellationToken);

        if (enhancedText.IsProvidedDataValid == false || string.IsNullOrWhiteSpace(enhancedText.Content))
        {
            _logger.LogError("It's unpossible to enhance the jobDescription. Invalid job description text provided: {jobDescriptionText}", command.JobDescriptionText);
            return new EnhanceJobDescriptionResponse(false, string.Empty, new ProblemDetails()
            {
                Type = HttpStatusCode.BadRequest.ToString(),
                Title = "Invalid Job Description Text. It's unpossible to enhance it.",
                Detail = "The text entereed for the job description is either too short or lacks of job description informations. " +
                "It's unpossible to enhance it. Please provide more informations about job offer.",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        return new EnhanceJobDescriptionResponse(true, enhancedText.Content, null);
    }
}