using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CareerPilotAi.Application.Commands.UpdateJobDescription;

public class UpdateJobDescriptionCommandHandler : ICommandHandler<UpdateJobDescriptionCommand, UpdateJobDescriptionResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UpdateJobDescriptionCommandHandler> _logger;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateJobDescriptionCommandHandler(ApplicationDbContext dbContext, ILogger<UpdateJobDescriptionCommandHandler> logger,
    IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UpdateJobDescriptionResponse> HandleAsync(UpdateJobDescriptionCommand command, CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrowException();
        var jobApplicationDataModel = await _dbContext.JobApplications
            .FirstOrDefaultAsync(j => j.JobApplicationId == command.JobApplicationId && j.UserId == userId, cancellationToken);

        if (jobApplicationDataModel == null)
        {
            _logger.LogError("Job application not found for JobApplicationId: {jobApplicationId} by UserId: {userId}",
                command.JobApplicationId, userId);
            return new UpdateJobDescriptionResponse(false, new ProblemDetails
            {
                Type = HttpStatusCode.NotFound.ToString(),
                Title = "Job Application Not Found",
                Detail = "The specified job application was not found.",
                Status = (int)HttpStatusCode.NotFound,
                Instance = _httpContextAccessor?.HttpContext?.Request.Path.ToString()
            });
        }

        var jobApplication = new JobApplication(
            jobApplicationDataModel.JobApplicationId,
            jobApplicationDataModel.UserId,
            jobApplicationDataModel.Title,
            jobApplicationDataModel.Company,
            jobApplicationDataModel.JobDescription,
            jobApplicationDataModel.Url
        );

        jobApplication.UpdateJobDescription(command.JobDescription);
        jobApplicationDataModel.JobDescription = jobApplication.JobDescription;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated job description for JobApplicationId: {jobApplicationId} by UserId: {userId}",
            command.JobApplicationId, userId);

        return new UpdateJobDescriptionResponse(true, null);
    }
}

