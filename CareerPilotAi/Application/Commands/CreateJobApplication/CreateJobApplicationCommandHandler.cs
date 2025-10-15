using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;

namespace CareerPilotAi.Application.Commands.CreateJobApplication;

public class CreateJobApplicationCommandHandler : ICommandHandler<CreateJobApplicationCommand, Guid>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly ILogger<CreateJobApplicationCommandHandler> _logger;

    public CreateJobApplicationCommandHandler(
        ApplicationDbContext dbContext,
        IUserService userService,
        ILogger<CreateJobApplicationCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userService = userService;
        _logger = logger;
    }

    public async Task<Guid> HandleAsync(
        CreateJobApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrowException();
        var vm = command.ViewModel;

        // Create JobApplication data model
        var jobApplicationDataModel = new JobApplicationDataModel
        {
            JobApplicationId = Guid.NewGuid(),
            UserId = userId,
            Title = vm.Position,
            Company = vm.CompanyName,
            JobDescription = vm.JobDescription,
            ExperienceLevel = vm.ExperienceLevel,
            Location = vm.Location,
            WorkMode = vm.WorkMode,
            ContractType = vm.ContractType,
            SalaryMin = vm.SalaryMin,
            SalaryMax = vm.SalaryMax,
            SalaryType = vm.SalaryType,
            SalaryPeriod = vm.SalaryPeriod,
            Url = vm.JobUrl,
            Status = vm.Status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Create skills if provided
        if (vm.Skills != null && vm.Skills.Any())
        {
            jobApplicationDataModel.Skills = vm.Skills.Select(s => new SkillDataModel
            {
                SkillId = Guid.NewGuid(),
                JobApplicationId = jobApplicationDataModel.JobApplicationId,
                Name = s.Name,
                Level = s.Level
            }).ToList();
        }

        // Use transaction for data consistency
        await using var transaction = await _dbContext.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            await _dbContext.JobApplications.AddAsync(
                jobApplicationDataModel,
                cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Job application created successfully. ID: {JobApplicationId}, User: {UserId}",
                jobApplicationDataModel.JobApplicationId,
                userId);

            return jobApplicationDataModel.JobApplicationId;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex,
                "Error creating job application for user: {UserId}",
                userId);
            throw;
        }
    }
}
