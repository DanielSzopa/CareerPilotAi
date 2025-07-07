using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Application.Commands.DeleteJobApplication;

public class DeleteJobApplicationCommandHandler : ICommandHandler<DeleteJobApplicationCommand, DeleteJobApplicationResponse>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUserService _userService;
    private readonly ILogger<DeleteJobApplicationCommandHandler> _logger;

    public DeleteJobApplicationCommandHandler(
        ApplicationDbContext applicationDbContext,
        IUserService userService,
        ILogger<DeleteJobApplicationCommandHandler> logger)
    {
        _applicationDbContext = applicationDbContext;
        _userService = userService;
        _logger = logger;
    }

    public async Task<DeleteJobApplicationResponse> HandleAsync(DeleteJobApplicationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userService.GetUserIdOrThrowException();
            
            var jobApplication = await _applicationDbContext.JobApplications
                .FirstOrDefaultAsync(j => j.JobApplicationId == command.JobApplicationId && j.UserId == userId, cancellationToken);

            if (jobApplication is null)
            {
                _logger.LogWarning("Job application not found for deletion: {jobApplicationId}, {userId}", command.JobApplicationId, userId);
                return new DeleteJobApplicationResponse(false, "Job application not found.");
            }

            _applicationDbContext.JobApplications.Remove(jobApplication);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Job application deleted successfully: {jobApplicationId}, {userId}", command.JobApplicationId, userId);
            return new DeleteJobApplicationResponse(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job application: {jobApplicationId}", command.JobApplicationId);
            return new DeleteJobApplicationResponse(false, "An error occurred while deleting the job application.");
        }
    }
}