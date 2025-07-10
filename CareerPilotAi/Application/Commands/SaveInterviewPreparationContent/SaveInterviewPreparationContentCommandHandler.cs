using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CareerPilotAi.Application.Commands.SaveInterviewPreparationContent;

public class SaveInterviewPreparationContentCommandHandler : ICommandHandler<SaveInterviewPreparationContentCommand, SaveInterviewPreparationContentResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SaveInterviewPreparationContentCommandHandler> _logger;

    public SaveInterviewPreparationContentCommandHandler(ApplicationDbContext dbContext,
        IUserService userService, IHttpContextAccessor httpContextAccessor, ILogger<SaveInterviewPreparationContentCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<SaveInterviewPreparationContentResponse> HandleAsync(SaveInterviewPreparationContentCommand command, CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrowException();
        
        // Verify the job application belongs to the current user
        var jobApplicationExists = await _dbContext.JobApplications
            .AsNoTracking()
            .AnyAsync(j => j.JobApplicationId == command.JobApplicationId && j.UserId == userId, cancellationToken);

        if (!jobApplicationExists)
        {
            _logger.LogError("Job application {JobApplicationId} not found for user {UserId} during interview preparation content save", command.JobApplicationId, userId);
            return new SaveInterviewPreparationContentResponse(false, "Job application not found", new ProblemDetails
            {
                Type = HttpStatusCode.NotFound.ToString(),
                Title = "Job Application Not Found",
                Detail = "The specified job application could not be found or you don't have permission to access it.",
                Status = (int)HttpStatusCode.NotFound,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        try
        {
            // Update or create InterviewQuestionsSection with preparation content
            var interviewSection = await _dbContext.InterviewQuestionsSections
                .Where(iqs => iqs.JobApplicationId == command.JobApplicationId)
                .FirstOrDefaultAsync(cancellationToken);

            if (interviewSection is null)
            {
                // Create new interview questions section
                interviewSection = new InterviewQuestionsSectionDataModel
                {
                    Id = Guid.NewGuid(),
                    JobApplicationId = command.JobApplicationId,
                    PreparationContent = command.PreparationContent,
                    InterviewQuestionsFeedbackMessage = "Content saved by user",
                    Status = "Success"
                };
                _dbContext.InterviewQuestionsSections.Add(interviewSection);
            }
            else
            {
                // Update existing interview questions section
                interviewSection.PreparationContent = command.PreparationContent;
                interviewSection.InterviewQuestionsFeedbackMessage = "Content updated by user";
                interviewSection.Status = "Success";
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully saved interview preparation content for job application {JobApplicationId} for user {UserId}", command.JobApplicationId, userId);
            
            return new SaveInterviewPreparationContentResponse(true, null, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving interview preparation content for job application {JobApplicationId}", command.JobApplicationId);
            return new SaveInterviewPreparationContentResponse(false, "An unexpected error occurred", new ProblemDetails
            {
                Type = HttpStatusCode.InternalServerError.ToString(),
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred while saving interview preparation content.",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }
    }
}
