namespace CareerPilotAi.Controllers;

using CareerPilotAi.Application.Commands.Dispatcher;
using CareerPilotAi.Application.Commands.GenerateInterviewQuestions;
using CareerPilotAi.Application.Commands.DeleteInterviewQuestion;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Models.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

[Authorize]
[Route("[controller]")]
public class InterviewQuestionsController : Controller
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly ILogger<InterviewQuestionsController> _logger;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUserService _userService;

    public InterviewQuestionsController(ICommandDispatcher commandDispatcher, ILogger<InterviewQuestionsController> logger, 
        ApplicationDbContext applicationDbContext, IUserService userService)
    {
        _commandDispatcher = commandDispatcher;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
        _userService = userService;
    }

    [HttpGet("api/generate/{jobApplicationId:guid}")]
    public async Task<IActionResult> GenerateInterviewQuestions(Guid jobApplicationId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _commandDispatcher
                .DispatchAsync<GenerateInterviewQuestionsCommand, GenerateInterviewQuestionsResponse>(new GenerateInterviewQuestionsCommand(jobApplicationId), cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response.outputModel);
            }

            return Problem(
                        type: response.ProblemDetails?.Type,
                        title: response.ProblemDetails?.Title,
                        detail: response.ProblemDetails?.Detail,
                        statusCode: response.ProblemDetails?.Status,
                        instance: response.ProblemDetails?.Instance
                    ); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating interview questions for job application {JobApplicationId}", jobApplicationId);
            return Problem(
                    type: HttpStatusCode.InternalServerError.ToString(),
                    title: "Internal server error",
                    detail: "Something went wrong. Please try again later.",
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    instance: HttpContext.Request.Path.ToString()
                    );
        }
    }

    [HttpGet("api/fetch/{jobApplicationId:guid}")]
    public async Task<IActionResult> FetchInterviewQuestions(Guid jobApplicationId, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userService.GetUserIdOrThrowException();
            
            // Verify the job application belongs to the current user
            var jobApplicationExists = await _applicationDbContext.JobApplications
                .AsNoTracking()
                .AnyAsync(j => j.JobApplicationId == jobApplicationId && j.UserId == userId, cancellationToken);

            if (!jobApplicationExists)
            {
                _logger.LogError("Job application {JobApplicationId} not found for user {UserId} during interview questions fetch", jobApplicationId, userId);
                return Problem(
                    type: HttpStatusCode.NotFound.ToString(),
                    title: "Job application not found",
                    detail: "The specified job application was not found or you don't have permission to access it.",
                    statusCode: (int)HttpStatusCode.NotFound,
                    instance: HttpContext.Request.Path.ToString()
                );
            }

            var interviewQuestions = await _applicationDbContext.InterviewQuestions
                .AsNoTracking()
                .Where(iq => iq.JobApplicationId == jobApplicationId)
                .Select(iq => new InterviewQuestionViewModel
                {
                    Id = iq.Id,
                    Question = iq.Question,
                    Answer = iq.Answer,
                    Status = iq.Status,
                    FeedbackMessage = iq.FeedbackMessage
                })
                .ToListAsync(cancellationToken);

            var viewModel = new InterviewQuestionsViewModel
            {
                InterviewQuestions = interviewQuestions
            };

            _logger.LogInformation("Fetched {QuestionsCount} interview questions for JobApplicationId: {jobApplicationId} and UserId: {userId}", 
                interviewQuestions.Count, jobApplicationId, userId);

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching interview questions for job application {JobApplicationId}", jobApplicationId);
            return Problem(
                type: HttpStatusCode.InternalServerError.ToString(),
                title: "Internal server error",
                detail: "Something went wrong. Please try again later.",
                statusCode: (int)HttpStatusCode.InternalServerError,
                instance: HttpContext.Request.Path.ToString()
            );
        }
    }

    [HttpDelete("api/delete/{interviewQuestionId:guid}")]
    public async Task<IActionResult> DeleteInterviewQuestion(Guid interviewQuestionId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _commandDispatcher
                .DispatchAsync<DeleteInterviewQuestionCommand, DeleteInterviewQuestionResponse>(
                    new DeleteInterviewQuestionCommand(interviewQuestionId), cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(new { success = true, message = "Interview question deleted successfully." });
            }

            return Problem(
                type: HttpStatusCode.BadRequest.ToString(),
                title: "Delete failed",
                detail: response.ErrorMessage,
                statusCode: (int)HttpStatusCode.BadRequest,
                instance: HttpContext.Request.Path.ToString()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting interview question {InterviewQuestionId}", interviewQuestionId);
            return Problem(
                type: HttpStatusCode.InternalServerError.ToString(),
                title: "Internal server error",
                detail: "Something went wrong. Please try again later.",
                statusCode: (int)HttpStatusCode.InternalServerError,
                instance: HttpContext.Request.Path.ToString()
            );
        }
    }
}