using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.OpenRouter;
using CareerPilotAi.Prompts.ParseJobDescription;
using CareerPilotAi.ViewModels.JobApplication;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.ParseJobDescription;

public class ParseJobDescriptionCommandHandler :
    ICommandHandler<ParseJobDescriptionCommand, ParseJobDescriptionResponse>
{
    private readonly ILogger<ParseJobDescriptionCommandHandler> _logger;
    private readonly OpenRouterService _openRouterService;
    private readonly IUserService _userService;
    public ParseJobDescriptionCommandHandler(
        ILogger<ParseJobDescriptionCommandHandler> logger,
        OpenRouterService openRouterService,
        IUserService userService)
    {
        _logger = logger;
        _openRouterService = openRouterService;
        _userService = userService;
    }

    public async Task<ParseJobDescriptionResponse> HandleAsync(
        ParseJobDescriptionCommand command,
        CancellationToken cancellationToken)
    {
         var userId = _userService.GetUserIdOrThrowException();

        try
        {
            var input = new ParseJobDescriptionInputModel(command.JobDescriptionText);
            var parsedResult = await _openRouterService.ParseJobDescriptionAsync(input, cancellationToken);
           
            if (parsedResult.IsSuccess == false)
            {
                _logger.LogError("Failed to parse job description: {feedbackMessage}, UserId: {userId}", parsedResult.FeedbackMessage, userId);
                return new ParseJobDescriptionResponse
                {
                    IsSuccess = false,
                    ProblemDetails = new ProblemDetails
                    {
                        Title = "Failed to parse job description",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = parsedResult.FeedbackMessage,
                    }
                };
            }

            return new ParseJobDescriptionResponse
            {
                IsSuccess = true,
                FeedbackMessage = parsedResult.FeedbackMessage,
                ParsedData = MapToViewModel(parsedResult)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while parsing job description. UserId: {userId}", userId);
            return new ParseJobDescriptionResponse
            {
                IsSuccess = false,
                ProblemDetails = new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An error occurred while parsing job description. Please try again later.",
                }
            };
        }
    }

    private static ParseJobDescriptionResultViewModel MapToViewModel(ParseJobDescriptionOutputModel model)
    {
        return new ParseJobDescriptionResultViewModel
        {
            Data = new CreateJobApplicationViewModel 
            {
                CompanyName = model.CompanyName,
                Position = model.Position,
                JobDescription = model.JobDescription,
                ExperienceLevel = model.ExperienceLevel,
                Location = model.Location,
                WorkMode = model.WorkMode,
                ContractType = model.ContractType,
                SalaryMin = model.SalaryMin,
                SalaryMax = model.SalaryMax,
                SalaryType = model.SalaryType,
                SalaryPeriod = model.SalaryPeriod,
                Skills = model.Skills?.Select(s => new SkillViewModel
                {
                    Name = s.Name,
                    Level = s.Level
                }).ToList() ?? new List<SkillViewModel>(),
                Status = ApplicationStatus.DefaultStatus
            },
            FeedbackMessage = model.FeedbackMessage
        };
    }
}