namespace CareerPilotAi.Controllers;

using CareerPilotAi.Application.Commands.Dispatcher;
using CareerPilotAi.Application.Commands.GenerateInterviewQuestions;
using CareerPilotAi.Application.Commands.DeleteInterviewQuestion;
using CareerPilotAi.Application.Commands.PrepareInterviewPreparationContent;
using CareerPilotAi.Application.Commands.SaveInterviewPreparationContent;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Models.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SaveInterviewPreparationContentRequest = CareerPilotAi.Models.InterviewQuestions.SaveInterviewPreparationContentRequest;

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

            var interviewQuestions = await _applicationDbContext.InterviewQuestionsSections
                .Include(iqs => iqs.Questions)
                .AsNoTracking()
                .Where(iq => iq.JobApplicationId == jobApplicationId)
                .SingleOrDefaultAsync();
                
            if (interviewQuestions == null)
                return Ok(new InterviewQuestionsSectionViewModel());

            var viewModel = new InterviewQuestionsSectionViewModel
            {
                Id = interviewQuestions.Id,
                PreparationContent = interviewQuestions.PreparationContent,
                FeedbackMessage = interviewQuestions.InterviewQuestionsFeedbackMessage,
                Status = interviewQuestions.Status,
                InterviewQuestions = interviewQuestions.Questions.Any() ?
                interviewQuestions.Questions.Select(q => new InterviewQuestionViewModel
                {
                    Id = q.Id,
                    Question = q.Question,
                    Answer = q.Answer,
                    Guide = q.Guide
                }).ToList() 
                : new List<InterviewQuestionViewModel>()
            };

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

    [HttpPost("api/prepare-content/{jobApplicationId:guid}")]
    public async Task<IActionResult> PrepareInterviewPreparationContent(Guid jobApplicationId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _commandDispatcher
                .DispatchAsync<PrepareInterviewPreparationContentCommand, PrepareInterviewPreparationContentResponse>(
                    new PrepareInterviewPreparationContentCommand(jobApplicationId), cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(new 
                { 
                    success = true, 
                    preparationContent = response.OutputModel?.PreparedContentDescriptionOutput,
                    feedbackMessage = response.OutputModel?.OutputFeedbackMessage,
                    status = response.OutputModel?.OutputStatus
                });
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
            _logger.LogError(ex, "An error occurred while preparing interview preparation content for job application {JobApplicationId}", jobApplicationId);
            return Problem(
                type: HttpStatusCode.InternalServerError.ToString(),
                title: "Internal server error",
                detail: "Something went wrong while preparing interview content. Please try again later.",
                statusCode: (int)HttpStatusCode.InternalServerError,
                instance: HttpContext.Request.Path.ToString()
            );
        }
    }

    [HttpPost("api/save-preparation-content/{jobApplicationId:guid}")]
    public async Task<IActionResult> SaveInterviewPreparationContent(Guid jobApplicationId, [FromBody] SaveInterviewPreparationContentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Problem(
                    type: HttpStatusCode.BadRequest.ToString(),
                    title: "Validation Failed",
                    detail: string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                    statusCode: (int)HttpStatusCode.BadRequest,
                    instance: HttpContext.Request.Path.ToString()
                );
            }

            var response = await _commandDispatcher
                .DispatchAsync<SaveInterviewPreparationContentCommand, SaveInterviewPreparationContentResponse>(
                    new SaveInterviewPreparationContentCommand(jobApplicationId, request.PreparationContent), cancellationToken);

            if (response.IsSuccess)
            {
                return Ok(new { success = true, message = "Interview preparation content saved successfully." });
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
            _logger.LogError(ex, "An error occurred while saving interview preparation content for job application {JobApplicationId}", jobApplicationId);
            return Problem(
                type: HttpStatusCode.InternalServerError.ToString(),
                title: "Internal server error",
                detail: "Something went wrong while saving interview preparation content. Please try again later.",
                statusCode: (int)HttpStatusCode.InternalServerError,
                instance: HttpContext.Request.Path.ToString()
            );
        }
    }
}