using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.OpenRouter;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Prompts.PrepareInterviewPreparationContent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CareerPilotAi.Application.Commands.PrepareInterviewPreparationContent;

public class PrepareInterviewPreparationContentCommandHandler : ICommandHandler<PrepareInterviewPreparationContentCommand, PrepareInterviewPreparationContentResponse>
{
    private readonly OpenRouterService _openRouterService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PrepareInterviewPreparationContentCommandHandler> _logger;

    public PrepareInterviewPreparationContentCommandHandler(OpenRouterService openRouterService, ApplicationDbContext dbContext,
        IUserService userService, IHttpContextAccessor httpContextAccessor, ILogger<PrepareInterviewPreparationContentCommandHandler> logger)
    {
        _openRouterService = openRouterService;
        _dbContext = dbContext;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<PrepareInterviewPreparationContentResponse> HandleAsync(PrepareInterviewPreparationContentCommand command, CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrowException();
        
        // Fetch job application with job description
        var jobApplication = await _dbContext.JobApplications
            .Include(x => x.Skills)
            .AsNoTracking()
            .Where(x => x.JobApplicationId == command.JobApplicationId && x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (jobApplication is null)
        {
            _logger.LogError("Job application {JobApplicationId} not found for user {UserId} during interview preparation content generation", command.JobApplicationId, userId);
            return new PrepareInterviewPreparationContentResponse(false, null, new ProblemDetails
            {
                Type = HttpStatusCode.NotFound.ToString(),
                Title = "Job Application Not Found",
                Detail = "The specified job application could not be found or you don't have permission to access it.",
                Status = (int)HttpStatusCode.NotFound,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        if (string.IsNullOrWhiteSpace(jobApplication.JobDescription))
        {
            _logger.LogError("Job description is empty for job application {JobApplicationId}", command.JobApplicationId);
            return new PrepareInterviewPreparationContentResponse(false, null, new ProblemDetails
            {
                Type = HttpStatusCode.BadRequest.ToString(),
                Title = "Job Description Missing",
                Detail = "The job application does not have a job description. Please add a job description before generating interview preparation content.",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        try
        {
            // Call OpenRouter service to generate preparation content

            var skills = jobApplication.Skills.Select(x => $"{x.Name} ({x.Level})").ToList();
            var preparationContentResult = await _openRouterService
                .PrepareInterviewPreparationContentAsync(new PrepareInterviewPreparationContentPromptInputModel(jobApplication.JobDescription, jobApplication.Title,skills, jobApplication.ExperienceLevel), cancellationToken);

            if (preparationContentResult is null)
            {
                _logger.LogError("No interview preparation content generated for job application {JobApplicationId}", command.JobApplicationId);
                return new PrepareInterviewPreparationContentResponse(false, null, new ProblemDetails
                {
                    Type = HttpStatusCode.BadRequest.ToString(),
                    Title = "Content Generation Failed",
                    Detail = "Failed to generate interview preparation content for the provided job description.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
                });
            }

            if (preparationContentResult.OutputStatus.ToLower() == "error")
            {
                _logger.LogError("AI service returned error for job application {JobApplicationId}: {FeedbackMessage}", command.JobApplicationId, preparationContentResult.OutputFeedbackMessage);
                return new PrepareInterviewPreparationContentResponse(false, null, new ProblemDetails
                {
                    Type = HttpStatusCode.BadRequest.ToString(),
                    Title = "Content Generation Error",
                    Detail = preparationContentResult.OutputFeedbackMessage,
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
                });
            }

            _logger.LogInformation("Successfully generated interview preparation content for job application {JobApplicationId} for user {UserId}", command.JobApplicationId, userId);
            
            return new PrepareInterviewPreparationContentResponse(true, preparationContentResult, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating interview preparation content for job application {JobApplicationId}", command.JobApplicationId);
            return new PrepareInterviewPreparationContentResponse(false, null, new ProblemDetails
            {
                Type = HttpStatusCode.InternalServerError.ToString(),
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred while generating interview preparation content.",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }
    }
}
