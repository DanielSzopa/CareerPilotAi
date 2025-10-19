using CareerPilotAi.Application.Commands.CreateJobApplication;
using CareerPilotAi.Application.Commands.DeleteJobApplication;
using CareerPilotAi.Application.Commands.Dispatcher;
using CareerPilotAi.Application.Commands.ParseJobDescription;
using CareerPilotAi.Application.Commands.UpdateJobDescription;
using CareerPilotAi.Application.Commands.UpdateJobApplicationStatus;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.ViewModels.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using CareerPilotAi.ViewModels;
using CareerPilotAi.Application.Helpers;

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
        private readonly IClock _clock;

        public JobApplicationController(IUserService userService, ApplicationDbContext applicationDbContext,
            ILogger<JobApplicationController> logger, ICommandDispatcher commandDispatcher, IClock clock)
        {
            _userService = userService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _clock = clock;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Return empty view - data will be loaded via API
            return View();
        }

        [HttpGet]
        [Route("api/cards")]
        public async Task<IActionResult> GetJobApplicationCards(CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserIdOrThrowException();
            var jobApplications = await _applicationDbContext.JobApplications
                .AsNoTracking()
                .Where(j => j.UserId == userId)
                .ToListAsync(cancellationToken);

            if (jobApplications == null || !jobApplications.Any())
                return Ok(new JobApplicationCardsViewModel() { Cards = new List<JobApplicationCardViewModel>() });
            
            var vm = new JobApplicationCardsViewModel
            {
                Cards = jobApplications.Select(j => new JobApplicationCardViewModel
                {
                    JobApplicationId = j.JobApplicationId,
                    Title = j.Title,
                    Company = j.Company,
                    CreatedAt = j.CreatedAt,
                    Status = j.Status
                }).ToList(),
                TotalApplications = jobApplications.Count,
                DraftStatusQuantity = jobApplications.Count(j => j.Status == ApplicationStatus.Draft.Status),
                RejectedStatusQuantity = jobApplications.Count(j => j.Status == ApplicationStatus.Rejected.Status),
                SubmittedStatusQuantity = jobApplications.Count(j => j.Status == ApplicationStatus.Submitted.Status),
                InterviewScheduledStatusQuantity = jobApplications.Count(j => j.Status == ApplicationStatus.InterviewScheduled.Status),
                WaitingForOfferStatusQuantity = jobApplications.Count(j => j.Status == ApplicationStatus.WaitingForOffer.Status),
                ReceivedOfferStatusQuantity = jobApplications.Count(j => j.Status == ApplicationStatus.ReceivedOffer.Status),
                NoContactStatusQuantity = jobApplications.Count(j => j.Status == ApplicationStatus.NoContact.Status)
            };

            return Ok(vm);
        }
        
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            var viewModel = new CreateJobApplicationViewModel
            {
                Status = ApplicationStatus.DefaultStatus
            };
            return View(viewModel);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CreateJobApplicationViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var jobApplicationId = await _commandDispatcher.DispatchAsync<CreateJobApplicationCommand, Guid>(
                    new CreateJobApplicationCommand(vm),
                    cancellationToken);

                TempData["SuccessMessage"] = "Application created successfully";
                return RedirectToAction(nameof(JobApplicationDetails), new { jobApplicationId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating job application");
                ModelState.AddModelError(string.Empty,
                    "An error occurred while creating the application. Please try again.");
                return View(vm);
            }
        }

        [HttpPost]
        [Route("api/parse-job-description")]
        public async Task<IActionResult> ParseJobDescription(
            [FromBody] ParseJobDescriptionViewModel vm,
            CancellationToken cancellationToken)
        {
            try
            {
                var userId = _userService.GetUserIdOrThrowException();
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for ParseJobDescription. UserId: {userId}, ModelState: {modelState}", userId, ModelState);
                    var errors = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();
                    return Problem(
                        type: HttpStatusCode.BadRequest.ToString(),
                        title: "One or more validation errors occurred.",
                        detail: string.Join(", ", errors),
                        statusCode: (int)HttpStatusCode.BadRequest,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                var response = await _commandDispatcher.DispatchAsync<
                    ParseJobDescriptionCommand,
                    ParseJobDescriptionResponse>(
                        new ParseJobDescriptionCommand(vm.JobDescriptionText),
                        cancellationToken);

                if (!response.IsSuccess)
                {
                    return Problem(
                        type: HttpStatusCode.BadRequest.ToString(),
                        title: "Failed to parse job description",
                        detail: response.FeedbackMessage,
                        statusCode: (int)HttpStatusCode.BadRequest,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                return Ok(new ParseJobDescriptionResultViewModel
                {
                    Success = true,
                    FeedbackMessage = response.FeedbackMessage,
                    Data = response.ParsedData?.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during parsing job description. UserId: {userId}", _userService.GetUserIdOrThrowException());
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
                return RedirectToAction(nameof(Create));
            }
            
            var userId = _userService.GetUserIdOrThrowException();

            var timeZoneId = await _applicationDbContext.UserSettings
                .AsNoTracking()
                .Select(us => us.TimeZoneId)
                .SingleAsync();

            var jobApplicationDataModel = await _applicationDbContext.JobApplications
                .Include(j => j.Skills)
                .FirstOrDefaultAsync(j => j.JobApplicationId == jobApplicationId && j.UserId == userId, cancellationToken);

            if (jobApplicationDataModel is null)
            {
                _logger.LogError("Job application not found: {jobApplicationId}, {userId}", jobApplicationId, userId);
                return RedirectToAction(nameof(Create));
            }

            var jobApplication = new JobApplication(
                jobApplicationDataModel.JobApplicationId,
                jobApplicationDataModel.UserId,
                jobApplicationDataModel.Title,
                jobApplicationDataModel.Company,
                jobApplicationDataModel.JobDescription,
                jobApplicationDataModel.Url,
                new ApplicationStatus(jobApplicationDataModel.Status)
            );

            var viewModel = new JobApplicationDetailsViewModel
            {
                JobApplicationId = jobApplication.JobApplicationId,
                CompanyName = jobApplication.Company,
                JobTitle = jobApplication.Title,
                JobDescription = jobApplication.JobDescription,
                Status = jobApplication.ApplicationStatus.Status,
                Location = jobApplicationDataModel.Location,
                WorkMode = jobApplicationDataModel.WorkMode,
                ContractType = jobApplicationDataModel.ContractType,
                ExperienceLevel = jobApplicationDataModel.ExperienceLevel,
                SalaryMin = jobApplicationDataModel.SalaryMin,
                SalaryMax = jobApplicationDataModel.SalaryMax,
                SalaryType = jobApplicationDataModel.SalaryType,
                SalaryPeriod = jobApplicationDataModel.SalaryPeriod,
                JobUrl = jobApplicationDataModel.Url,
                Skills = jobApplicationDataModel.Skills?.Select(s => new SkillViewModel
                {
                    Name = s.Name,
                    Level = s.Level
                }).OrderBy(s => SkillLevel.GetLevelIndex(s.Level))
                .ToList() ?? new List<SkillViewModel>(),
                CreatedAt = _clock.GetDateTimeAdjustedToTimeZone(jobApplicationDataModel.CreatedAt, timeZoneId),
                UpdatedAt = _clock.GetDateTimeAdjustedToTimeZone(jobApplicationDataModel.UpdatedAt, timeZoneId),
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

        [HttpPatch]
        [Route("api/status/{jobApplicationId:guid}")]
        public async Task<IActionResult> UpdateJobApplicationStatus(Guid jobApplicationId, [FromBody] UpdateJobApplicationStatusViewModel vm, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for UpdateJobApplicationStatus request: {jobApplicationId}", jobApplicationId);
                    return Problem(
                        type: HttpStatusCode.BadRequest.ToString(),
                        title: "Invalid Request",
                        detail: "The request data is invalid. Please check the status field.",
                        statusCode: (int)HttpStatusCode.BadRequest,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                if (jobApplicationId == Guid.Empty)
                {
                    _logger.LogError("JobApplicationId cannot be empty during status update action");
                    return Problem(
                        type: HttpStatusCode.BadRequest.ToString(),
                        title: "Invalid Job Application ID",
                        detail: "The job application ID cannot be empty.",
                        statusCode: (int)HttpStatusCode.BadRequest,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                var response = await _commandDispatcher.DispatchAsync<UpdateJobApplicationStatusCommand, UpdateJobApplicationStatusResponse>(
                    new UpdateJobApplicationStatusCommand(jobApplicationId, vm.Status), cancellationToken);

                if (!response.IsSuccess)
                {
                    return Problem(
                        type: response.ProblemDetails?.Type ?? HttpStatusCode.InternalServerError.ToString(),
                        title: response.ProblemDetails?.Title ?? "Error",
                        detail: response.ProblemDetails?.Detail ?? "An error occurred while updating the application status.",
                        statusCode: response.ProblemDetails?.Status ?? (int)HttpStatusCode.InternalServerError,
                        instance: HttpContext.Request.Path.ToString()
                    );
                }

                return Ok(new { success = true, message = "Application status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application status: {jobApplicationId}", jobApplicationId);
                return Problem(
                    type: HttpStatusCode.InternalServerError.ToString(),
                    title: "Internal Server Error",
                    detail: "An error occurred while updating the application status. Please try again.",
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    instance: HttpContext.Request.Path.ToString()
                );
            }
        }
    }
}