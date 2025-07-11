using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.OpenRouter;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using CareerPilotAi.Prompts.GenerateInterviewQuestions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CareerPilotAi.Application.Commands.GenerateInterviewQuestions;

public class GenerateInterviewQuestionsCommandHandler : ICommandHandler<GenerateInterviewQuestionsCommand, GenerateInterviewQuestionsResponse>
{
    private readonly OpenRouterService _openRouterService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<GenerateInterviewQuestionsCommandHandler> _logger;

    public GenerateInterviewQuestionsCommandHandler(OpenRouterService openRouterService, ApplicationDbContext dbContext,
    IUserService userService, IHttpContextAccessor httpContextAccessor, ILogger<GenerateInterviewQuestionsCommandHandler> logger)
    {
        _openRouterService = openRouterService;
        _dbContext = dbContext;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<GenerateInterviewQuestionsResponse> HandleAsync(GenerateInterviewQuestionsCommand command, CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrowException();
        var interviewQuestionsSection = await _dbContext.InterviewQuestionsSections
            .Include(x => x.JobApplication)
            .Include(x => x.Questions)
            .Where(x => x.JobApplicationId == command.JobApplicationId && x.JobApplication.UserId == userId)
            .SingleOrDefaultAsync();

        if (interviewQuestionsSection is null || interviewQuestionsSection.JobApplication is null)
        {
            _logger.LogError("Interview questions section for job application ID {JobApplicationId} not found for user {UserId}.", command.JobApplicationId, userId);
            return new GenerateInterviewQuestionsResponse(false, null, new ProblemDetails
            {
                Type = HttpStatusCode.NotFound.ToString(),
                Title = "Interview Questions Section Not Found",
                Detail = "The specified interview questions section could not be found.",
                Status = (int)HttpStatusCode.NotFound,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        var interviewQuestions = new InterviewQuestions(interviewQuestionsSection.JobApplicationId,
            interviewQuestionsSection.JobApplication.Title, interviewQuestionsSection.JobApplication.Company, interviewQuestionsSection.PreparationContent);

        foreach (var question in interviewQuestionsSection.Questions)
        {
            interviewQuestions.AddQuestion(new SingleInterviewQuestion(question.Id, question.Question, question.Answer, question.Guide));
        }

        if(interviewQuestions.ShouldGenerateFirstInterviewQuestions())
        {
            var firstQuestionsResult = await _openRouterService
                .GenerateInterviewQuestionsAsync(new GenerateInterviewQuestionsPromptInputModel(interviewQuestions.CompanyName, interviewQuestions.JobRole, interviewQuestions.InterviewPreparationContent), cancellationToken);

            if (firstQuestionsResult is null)
            {
                _logger.LogError("No initial interview questions generated for job application ID {JobApplicationId}.", command.JobApplicationId);
                return new GenerateInterviewQuestionsResponse(false, null, new ProblemDetails
                {
                    Type = HttpStatusCode.BadRequest.ToString(),
                    Title = "No Questions Generated",
                    Detail = "No initial interview questions could be generated for the provided job application.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
                });
            }

            if (firstQuestionsResult.OutputStatus.ToLower() != "success")
            {
                _logger.LogError("LLM wasn't able to generate initial interview questions for job application ID {JobApplicationId}: {FeedbackMessage}", command.JobApplicationId, firstQuestionsResult.OutputFeedbackMessage);
            }

            interviewQuestionsSection.InterviewQuestionsFeedbackMessage = firstQuestionsResult.OutputFeedbackMessage;
            interviewQuestionsSection.Status = firstQuestionsResult.OutputStatus;

            if (firstQuestionsResult.InterviewQuestions is not null && firstQuestionsResult.InterviewQuestions.Any())
            {
                foreach (var firstQuestion in firstQuestionsResult.InterviewQuestions)
                {
                    interviewQuestionsSection.Questions.Add(new InterviewQuestionDataModel
                    {
                        InterviewQuestionsSectionId = interviewQuestionsSection.Id,
                        Question = firstQuestion.Question,
                        Answer = firstQuestion.Answer,
                        Guide = firstQuestion.Guide
                    });
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new GenerateInterviewQuestionsResponse(true, firstQuestionsResult, null);
        }

        if (!interviewQuestions.CanGenerateMoreInterviewQuestions())
        {
            _logger.LogError("Cannot generate more interview questions for job application ID {JobApplicationId}. Maximum limit {limit} reached.", interviewQuestions.JobApplicationId, interviewQuestions._maxQuestions);
            return new GenerateInterviewQuestionsResponse(false, null, new ProblemDetails
            {
                Type = HttpStatusCode.BadRequest.ToString(),
                Title = "Maximum Questions Reached",
                Detail = "You have reached the maximum number of interview questions for this job application.",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        throw new NotImplementedException();
    }
}
