using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.OpenRouter;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using CareerPilotAi.Models.JobApplication;
using CareerPilotAi.Prompts.GenerateInterviewQuestions;
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

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var interviewQuestionsSection = await _dbContext.InterviewQuestionsSections
            .Include(x => x.JobApplication)
            .Include(x => x.Questions)
            .Where(x => x.JobApplicationId == command.JobApplicationId && x.JobApplication.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

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

        var interviewQuestionsCore = new InterviewQuestions(interviewQuestionsSection.JobApplicationId,
            interviewQuestionsSection.JobApplication.Title, interviewQuestionsSection.JobApplication.Company, interviewQuestionsSection.PreparationContent);

        foreach (var question in interviewQuestionsSection.Questions)
        {
            interviewQuestionsCore.AddQuestion(new SingleInterviewQuestion(question.Id, question.Question, question.Answer, question.Guide, question.IsActive));
        } 

        var openRouterServiceResponse = await _openRouterService
            .GenerateInterviewQuestionsAsync(
                new GenerateInterviewQuestionsPromptInputModel(interviewQuestionsCore.CompanyName, interviewQuestionsCore.JobRole,
                interviewQuestionsCore.InterviewPreparationContent, interviewQuestionsCore.Questions, command.numberOfQuestionsToGenerate), cancellationToken);

        if (openRouterServiceResponse is null)
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

        if (openRouterServiceResponse.OutputStatus.ToLower() != "success")
            _logger.LogError("LLM wasn't able to generate initial interview questions for job application ID {JobApplicationId}: {FeedbackMessage}", command.JobApplicationId, openRouterServiceResponse.OutputFeedbackMessage);

        _logger.LogInformation("LLM generated {Count} interview questions for job application ID {JobApplicationId}", openRouterServiceResponse.InterviewQuestions?.Count ?? 0, command.JobApplicationId);
        
        interviewQuestionsSection.InterviewQuestionsFeedbackMessage = openRouterServiceResponse.OutputFeedbackMessage;
        interviewQuestionsSection.Status = openRouterServiceResponse.OutputStatus;

        if (openRouterServiceResponse.InterviewQuestions is not null && openRouterServiceResponse.InterviewQuestions.Any())
        {
            foreach (var q in openRouterServiceResponse.InterviewQuestions)
            {
                var newQuestionId = Guid.NewGuid();
                interviewQuestionsCore.AddQuestion(new SingleInterviewQuestion(newQuestionId, q.Question, q.Answer, q.Guide, true));

                await _dbContext.InterviewQuestions.AddAsync(new InterviewQuestionDataModel
                {
                    Id = newQuestionId,
                    InterviewQuestionsSectionId = interviewQuestionsSection.Id,
                    Question = q.Question,
                    Answer = q.Answer,
                    Guide = q.Guide,
                    IsActive = true
                }, cancellationToken);
            }
        }

        var vm = new InterviewQuestionsSectionViewModel()
        {
            FeedbackMessage = openRouterServiceResponse.OutputFeedbackMessage,
            Status = openRouterServiceResponse.OutputStatus,
            Id = interviewQuestionsSection.Id,
            InterviewQuestions = interviewQuestionsCore.GetActiveQuestions()
                .Select(q => new InterviewQuestionViewModel
                {
                    Id = q.Id,
                    Question = q.Question,
                    Answer = q.Answer,
                    Guide = q.Guide
                }).ToList(),
        };

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new GenerateInterviewQuestionsResponse(true, vm, null);
    }
}
