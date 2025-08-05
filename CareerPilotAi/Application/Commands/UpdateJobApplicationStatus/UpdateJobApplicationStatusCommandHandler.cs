using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CareerPilotAi.Application.Commands.UpdateJobApplicationStatus;

public class UpdateJobApplicationStatusCommandHandler : ICommandHandler<UpdateJobApplicationStatusCommand, UpdateJobApplicationStatusResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UpdateJobApplicationStatusCommandHandler> _logger;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateJobApplicationStatusCommandHandler(ApplicationDbContext dbContext, ILogger<UpdateJobApplicationStatusCommandHandler> logger,
        IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UpdateJobApplicationStatusResponse> HandleAsync(UpdateJobApplicationStatusCommand command, CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrowException();
        var jobApplicationDataModel = await _dbContext.JobApplications
            .FirstOrDefaultAsync(j => j.JobApplicationId == command.JobApplicationId && j.UserId == userId, cancellationToken);

        if (jobApplicationDataModel == null)
        {
            _logger.LogError("Job application not found for JobApplicationId: {jobApplicationId} by UserId: {userId}",
                command.JobApplicationId, userId);
            return new UpdateJobApplicationStatusResponse(false, new ProblemDetails
            {
                Type = HttpStatusCode.NotFound.ToString(),
                Title = "Job Application Not Found",
                Detail = "The specified job application was not found.",
                Status = (int)HttpStatusCode.NotFound,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        try
        {
            var jobApplication = new JobApplication(
                jobApplicationDataModel.JobApplicationId,
                jobApplicationDataModel.UserId,
                jobApplicationDataModel.Title,
                jobApplicationDataModel.Company,
                jobApplicationDataModel.JobDescription,
                jobApplicationDataModel.Url,
                new ApplicationStatus(jobApplicationDataModel.Status)
            );

            var newApplicationStatus = new ApplicationStatus(command.Status);
            jobApplication.UpdateStatus(newApplicationStatus);
            jobApplicationDataModel.Status = jobApplication.ApplicationStatus.Status;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated application status to {status} for JobApplicationId: {jobApplicationId} by UserId: {userId}",
                command.Status, command.JobApplicationId, userId);

            return new UpdateJobApplicationStatusResponse(true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application status for JobApplicationId: {jobApplicationId} by UserId: {userId}",
                command.JobApplicationId, userId);
            return new UpdateJobApplicationStatusResponse(false, new ProblemDetails
            {
                Type = HttpStatusCode.InternalServerError.ToString(),
                Title = "Internal Server Error",
                Detail = "An error occurred while updating the application status. Please try again.",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }
    }
}
