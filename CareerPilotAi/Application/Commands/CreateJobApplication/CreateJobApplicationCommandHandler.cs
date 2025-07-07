using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;

namespace CareerPilotAi.Application.Commands.CreateJobApplication;

public class CreateJobApplicationCommandHandler : ICommandHandler<CreateJobApplicationCommand, Guid>
{
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CreateJobApplicationCommandHandler> _logger;

    public CreateJobApplicationCommandHandler(IUserService userService, ApplicationDbContext dbContext, ILogger<CreateJobApplicationCommandHandler> logger)
    {
        _userService = userService;
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<Guid> HandleAsync(CreateJobApplicationCommand command, CancellationToken cancellationToken)
    {
        var vm = command.vm;
        var jobApplication = new JobApplication(Guid.NewGuid(), _userService.GetUserIdOrThrowException(), vm.Title, vm.Company, vm.JobDescription, vm.URL);

        await _dbContext.JobApplications.AddAsync(new JobApplicationDataModel()
        {
            JobApplicationId = jobApplication.JobApplicationId,
            UserId = jobApplication.UserId,
            Title = jobApplication.Title,
            Company = jobApplication.Company,
            Url = jobApplication.URL,
            JobDescription = jobApplication.JobDescription,
            CreatedAt = DateTime.UtcNow,
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Creating new job application with JobApplicationId: {jobApplicationId} for UserId: {userId}", jobApplication.JobApplicationId, jobApplication.UserId);

        return jobApplication.JobApplicationId;
    }
}
