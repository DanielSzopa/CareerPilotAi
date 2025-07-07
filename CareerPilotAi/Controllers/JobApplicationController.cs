using CareerPilotAi.Application.Commands.CreateJobApplication;
using CareerPilotAi.Application.Commands.DeleteJobApplication;
using CareerPilotAi.Application.Commands.Dispatcher;
using CareerPilotAi.Application.Commands.EnhanceJobDescription;
using CareerPilotAi.Application.Commands.UpdateJobDescription;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Models;
using CareerPilotAi.Models.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CareerPilotAi.Controllers
{
    [Route("job-applications")]
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<JobApplicationController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public JobApplicationController(IUserService userService, ApplicationDbContext applicationDbContext,
            ILogger<JobApplicationController> logger, ICommandDispatcher commandDispatcher)
        {
            _userService = userService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserIdOrThrowException();
            var jobApplicationsCards = await _applicationDbContext.JobApplications
                .AsNoTracking()
                .Where(j => j.UserId == userId)
                .Select(j => new JobApplicationCardViewModel
                {
                    JobApplicationId = j.JobApplicationId,
                    Title = j.Title,
                    Company = j.Company,
                    CardDate = new CardDate(j.CreatedAt)
                })
                .ToListAsync();

            return View(jobApplicationsCards);
        }

        [HttpGet]
        [Route("entry-job-details")]
        public IActionResult CreateTheEntryJobApplication()
        {
            return View("JobOfferEntryDetails", new JobOfferEntryDetailsViewModel());
        }

        [HttpPost]
        [Route("entry-job-details")]
        public async Task<IActionResult> ProcessTheEntryJobApplication(JobOfferEntryDetailsViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("JobOfferEntryDetails", vm);
            }

            var jobApplicationId = await _commandDispatcher.DispatchAsync<CreateJobApplicationCommand, Guid>(new CreateJobApplicationCommand(vm), cancellationToken);

            return RedirectToAction(nameof(JobApplicationDetails), new { jobApplicationId });
        }

        [HttpPost]
        [Route("api/enhance-job-description")]
        public async Task<IActionResult> EnhanceJobDescription([FromBody] EnhanceJobDescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _commandDispatcher.DispatchAsync<EnhanceJobDescriptionCommand, EnhanceJobDescriptionResponse>(new (request.JobDescriptionText), cancellationToken);
                if (!response.IsSuccess)
                {
                    return Problem(
                        type: response.ProblemDetails?.Type,
                        title: response.ProblemDetails?.Title,
                        detail: response.ProblemDetails?.Detail,
                        statusCode: response.ProblemDetails?.Status,
                        instance: response.ProblemDetails?.Instance
                    );
                }

                return Ok(new { Content = response.EnhancedJobDescription });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during enhancing job description");
                return Problem(
                       type: HttpStatusCode.InternalServerError.ToString(),
                       title: "Internal server error",
                       detail: "Something went wrong. Please try again or provide different text content.",
                       statusCode: (int)HttpStatusCode.InternalServerError,
                       instance: HttpContext.Request.Path.ToString()
                       );
            }
        }

        [HttpGet]
        [Route("details/{jobApplicationId:guid}")]
        public async Task<IActionResult> JobApplicationDetails(Guid jobApplicationId, CancellationToken cancellationToken)
        {
            if (jobApplicationId == Guid.Empty)
            {
                _logger.LogError("JobApplicationId cannot be empty during the action: {action}", nameof(JobApplicationDetails));
                return RedirectToAction(nameof(CreateTheEntryJobApplication));
            }
            var userId = _userService.GetUserIdOrThrowException();
            var jobApplicationDataModel = await _applicationDbContext.JobApplications
                .FirstOrDefaultAsync(j => j.JobApplicationId == jobApplicationId && j.UserId == userId, cancellationToken);

            if (jobApplicationDataModel is null)
            {
                _logger.LogError("Job application not found: {jobApplicationId}, {userId}", jobApplicationId, userId);
                return RedirectToAction(nameof(CreateTheEntryJobApplication));
            }

            var jobApplication = new JobApplication(
                jobApplicationDataModel.JobApplicationId,
                jobApplicationDataModel.UserId,
                jobApplicationDataModel.Title,
                jobApplicationDataModel.Company,
                jobApplicationDataModel.JobDescription,
                jobApplicationDataModel.Url
            );

            var viewModel = new JobApplicationDetailsViewModel
            {
                JobApplicationId = jobApplication.JobApplicationId,
                CompanyName = jobApplication.Company,
                JobTitle = jobApplication.Title,
                JobDescription = jobApplication.JobDescription,
            };

            return View("JobApplicationDetails", viewModel);
        }

        [HttpPost]
        [Route("api/update-job-description")]
        public async Task<IActionResult> UpdateJobDescription([FromBody] UpdateJobDescriptionViewModel vm, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for UpdateJobDescription: {modelState}", ModelState);
                    return Problem(
                        type: HttpStatusCode.BadRequest.ToString(),
                        title: "Invalid Input Data",
                        detail: "The provided data is invalid.",
                        statusCode: (int)HttpStatusCode.BadRequest,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                var response = await _commandDispatcher.DispatchAsync<UpdateJobDescriptionCommand, UpdateJobDescriptionResponse>(
                    new UpdateJobDescriptionCommand(vm.JobApplicationId, vm.JobDescription), cancellationToken);

                if (!response.IsSuccess)
                {
                    return Problem(
                        type: response.ProblemDetails?.Type,
                        title: response.ProblemDetails?.Title,
                        detail: response.ProblemDetails?.Detail,
                        statusCode: response.ProblemDetails?.Status,
                        instance: response.ProblemDetails?.Instance
                    );
                }

                return Ok(new { success = true, message = "Job description updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job description for JobApplicationId: {jobApplicationId}", vm.JobApplicationId);
                return Problem(
                    type: HttpStatusCode.InternalServerError.ToString(),
                    title: "Internal Server Error",
                    detail: "An error occurred while updating the job description. Please try again.",
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    instance: HttpContext.Request.Path.ToString()
                );
            }
        }

        [HttpDelete]
        [Route("api/delete/{jobApplicationId:guid}")]
        public async Task<IActionResult> DeleteJobApplication(Guid jobApplicationId, CancellationToken cancellationToken)
        {
            try
            {
                if (jobApplicationId == Guid.Empty)
                {
                    _logger.LogError("JobApplicationId cannot be empty during delete action");
                    return Problem(
                        type: HttpStatusCode.BadRequest.ToString(),
                        title: "Invalid Job Application ID",
                        detail: "The job application ID cannot be empty.",
                        statusCode: (int)HttpStatusCode.BadRequest,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                var response = await _commandDispatcher.DispatchAsync<DeleteJobApplicationCommand, DeleteJobApplicationResponse>(
                    new DeleteJobApplicationCommand(jobApplicationId), cancellationToken);

                if (!response.IsSuccess)
                {
                    return Problem(
                        type: HttpStatusCode.NotFound.ToString(),
                        title: "Job Application Not Found",
                        detail: response.ErrorMessage ?? "The job application could not be found.",
                        statusCode: (int)HttpStatusCode.NotFound,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                return Ok(new { success = true, message = "Job application deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting job application: {jobApplicationId}", jobApplicationId);
                return Problem(
                    type: HttpStatusCode.InternalServerError.ToString(),
                    title: "Internal Server Error",
                    detail: "An error occurred while deleting the job application. Please try again.",
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    instance: HttpContext.Request.Path.ToString()
                );
            }
        }
    }
}