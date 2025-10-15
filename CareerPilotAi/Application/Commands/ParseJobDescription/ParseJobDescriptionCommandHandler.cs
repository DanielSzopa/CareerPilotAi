using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.ViewModels.JobApplication;
using CareerPilotAi.Core;

namespace CareerPilotAi.Application.Commands.ParseJobDescription;

public class ParseJobDescriptionCommandHandler :
    ICommandHandler<ParseJobDescriptionCommand, ParseJobDescriptionResponse>
{
    private readonly ILogger<ParseJobDescriptionCommandHandler> _logger;

    public ParseJobDescriptionCommandHandler(
        ILogger<ParseJobDescriptionCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<ParseJobDescriptionResponse> HandleAsync(
        ParseJobDescriptionCommand command,
        CancellationToken cancellationToken)
    {
        // TODO: Implement actual AI parsing with OpenRouter
        // For now, return mock response for testing

        _logger.LogInformation("Parsing job description (MOCK)");

        // Simulate async operation
        await Task.Delay(1000, cancellationToken);

        // Mock response - FullSuccess with sample data
        return new ParseJobDescriptionResponse
        {
            IsSuccess = true,
            ParsingResult = ParsingResultType.FullSuccess,
            MissingFields = new List<string>(),
            ParsedData = new CreateJobApplicationViewModel
            {
                CompanyName = "Example Company",
                Position = "Senior Software Developer",
                JobDescription = command.JobDescriptionText,
                ExperienceLevel = ExperienceLevel.Senior,
                Location = "Warsaw, Poland",
                WorkMode = WorkMode.Hybrid,
                ContractType = ContractType.B2B,
                SalaryMin = 15000,
                SalaryMax = 20000,
                SalaryType = Core.SalaryType.Net,
                SalaryPeriod = SalaryPeriodType.Monthly,
                Skills = new List<SkillViewModel>
                {
                    new() { Name = "C#", Level = SkillLevel.Advanced },
                    new() { Name = "ASP.NET Core", Level = SkillLevel.Advanced },
                    new() { Name = "Entity Framework", Level = SkillLevel.Regular }
                },
                Status = ApplicationStatus.DefaultStatus
            }
        };
    }
}

