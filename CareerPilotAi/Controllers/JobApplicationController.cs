using CareerPilotAi.Application.Helpers;
using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.OpenRouter;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using CareerPilotAi.Models.JobApplication;
using CareerPilotAi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CareerPilotAi.Controllers
{
    [Route("job-applications")]
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<JobApplicationController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public JobApplicationController(IUserService userService, ApplicationDbContext applicationDbContext, 
            ILogger<JobApplicationController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _userService = userService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] Guid jobApplicationId, CancellationToken cancellationToken)
        {
            if (jobApplicationId == Guid.Empty)
                return View("JobOfferDetails", new JobOfferEntryDetailsViewModel());

            var userId = _userService.GetUserIdOrThrowException();

            var jobApplicationDataModel = await _applicationDbContext.JobApplications
                .AsNoTracking()
                .Select(j => new
                {
                    Id = j.JobApplicationId,
                    Url = j.EntryJobDetails_Url,
                    Text = j.EntryJobDetails_Text,
                    j.UserId
                })
                .FirstOrDefaultAsync(j => j.Id == jobApplicationId && j.UserId == userId, cancellationToken);

            if (jobApplicationDataModel is null)
            {
                _logger.LogCritical("Cannot find jobApplicationDataModel with jobApplicationId: {jobApplicationId} and userId: {userId}", jobApplicationId, userId);
                return View("JobOfferDetails", new JobOfferEntryDetailsViewModel());
            }

            return View("JobOfferDetails", new JobOfferEntryDetailsViewModel()
            {
                JobApplicationId = jobApplicationDataModel.Id,
                Url = jobApplicationDataModel.Url ?? "",
                Text = jobApplicationDataModel.Text ?? "",
            });
        }

        [HttpPost]
        [Route("details")]
        public async Task<IActionResult> ProcessJobDetails(JobOfferEntryDetailsViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("JobOfferDetails", vm);
            }

            var entryJobDetails = new EntryJobDetails(vm.Url, vm.Text);
            var userId = _userService.GetUserIdOrThrowException();
            var jobApplicationId = vm.JobApplicationId ?? Guid.NewGuid();
            var jobApplication = new JobApplication(jobApplicationId, userId, entryJobDetails, default);

            var isJobApplicationExists = await _applicationDbContext.JobApplications
                .AnyAsync(j => j.JobApplicationId == jobApplication.JobApplicationId && j.UserId == jobApplication.UserId, cancellationToken);

            if (isJobApplicationExists)
            {
                var jobApplicationDataModel = await _applicationDbContext.JobApplications
                    .SingleAsync(j => j.JobApplicationId == jobApplication.JobApplicationId && j.UserId == jobApplication.UserId, cancellationToken);

                jobApplicationDataModel.EntryJobDetails_Text = jobApplication.EntryJobDetails.Text;
                jobApplicationDataModel.EntryJobDetails_Url = jobApplication.EntryJobDetails.Url!;

                _logger.LogInformation("Updating existing job application with JobApplicationId: {jobApplicationId} for UserId: {userId}", jobApplication.JobApplicationId, jobApplication.UserId);
            }
            else
            {
                var newJobApplicationDataModel = new JobApplicationDataModel()
                {
                    JobApplicationId = jobApplication.JobApplicationId,
                    UserId = jobApplication.UserId,
                    EntryJobDetails_Url = jobApplication.EntryJobDetails.Url!,
                    EntryJobDetails_Text = jobApplication.EntryJobDetails.Text,
                };

                await _applicationDbContext.JobApplications.AddAsync(newJobApplicationDataModel, cancellationToken);

                _logger.LogInformation("Creating new job application with JobApplicationId: {jobApplicationId} for UserId: {userId}", jobApplication.JobApplicationId, jobApplication.UserId);
            }

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(ProvidePersonalDetails), new { jobApplicationId = jobApplication.JobApplicationId });
        }

        [HttpGet]
        [Route("url-details")]
        public async Task<IActionResult> JobOfferData([FromQuery] Guid jobApplicationId, [FromQuery] string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty", nameof(url));

            UrlValidator.ValidateUrl(url);

            // Log the URL for debugging purposes
            _logger.LogInformation("Attempting to fetch job data from URL: {url} for jobApplicationId: {jobApplicationId}", url, jobApplicationId);
            
            // Simulate processing time
            await Task.Delay(3_000);
            
            // For now, return the same Lorem Ipsum data regardless of URL
            // In a real implementation, you would scrape the actual URL content here
            return Ok(MockData.MockJobDescription);
        }

        [HttpGet]
        [Route("personal-details")]
        public async Task<IActionResult> ProvidePersonalDetails([FromQuery] Guid jobApplicationId, CancellationToken cancellationToken)
        {
            if (jobApplicationId == Guid.Empty)
            {
                _logger.LogError("JobApplicationId is empty, cannot process {ControllerAction} action", nameof(ProvidePersonalDetails));
                return RedirectToAction(nameof(Index));
            }

            var userId = _userService.GetUserIdOrThrowException();

            var jobApplicationDataModel = await _applicationDbContext.JobApplications
                .AsNoTracking()
                .Select(j => new
                {
                    Id = j.JobApplicationId,
                    PersonalDetails = j.PersonalDetails_Text,
                    j.UserId
                })
                .FirstOrDefaultAsync(j => j.Id == jobApplicationId && j.UserId == userId, cancellationToken);

            if (jobApplicationDataModel is null)
            {
                _logger.LogCritical("Cannot find jobApplicationDataModel with jobApplicationId: {jobApplicationId} and userId: {userId}", jobApplicationId, userId);
                return RedirectToAction(nameof(Index));
            }

            return View(new UserDetailsViewModel()
            {
                JobApplicationId = jobApplicationDataModel.Id,
                PersonalDetails = jobApplicationDataModel.PersonalDetails ?? ""
            });
        }

        [HttpPost]
        [Route("personal-details")]
        public async Task<IActionResult> SaveUserDetails(UserDetailsViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("ProvidePersonalDetails", vm);
            }

            var userId = _userService.GetUserIdOrThrowException();
            var jobApplicationDataModel = await _applicationDbContext.JobApplications
                .FirstOrDefaultAsync(j => j.JobApplicationId == vm.JobApplicationId && j.UserId == userId, cancellationToken);

            if (jobApplicationDataModel is null)
            {
                _logger.LogCritical("Cannot find jobApplicationDataModel with jobApplicationId: {jobApplicationId} and userId: {userId}", vm.JobApplicationId, userId);
                return View("ProvidePersonalDetails", vm);
            }

            var jobApplication = new JobApplication(jobApplicationDataModel.JobApplicationId, jobApplicationDataModel.UserId, new EntryJobDetails(jobApplicationDataModel.EntryJobDetails_Url, jobApplicationDataModel.EntryJobDetails_Text)
                , new PersonalDetails(vm.PersonalDetails));

            jobApplicationDataModel.PersonalDetails_Text = jobApplication.PersonalDetails.Text;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {userId} saved personal details for job application {jobApplicationId}", userId, jobApplication.JobApplicationId);

            return RedirectToAction(nameof(SuccessPageTemporary));
        }

        [HttpPost]
        [Route("process-pdf")]
        public async Task<IActionResult> ProcessPdf(IFormFile file, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserIdOrThrowException();

            if (file == null || file.Length == 0)
            {
                _logger.LogError("No file uploaded in {ControllerAction} action for user {userId}", nameof(ProcessPdf), userId);
                return BadRequest("No file has been uploaded.");
            }
            
            // Check if the file is a PDF
            if (file.ContentType != "application/pdf" && !file.FileName.EndsWith(".pdf"))
            {
                _logger.LogError("Invalid file type uploaded {invalidType} in {ControllerAction} action for user {userId}. Expected PDF.", file.ContentType, nameof(ProcessPdf), userId);
                return BadRequest($"Invalid file type uploaded {file.ContentType}. Only Pdf format is accepted.");
            }

            // Validate file size if needed (optional)
            if (file.Length > 5242880) // 5MB limit
            {
                _logger.LogError("File too large ({fileSize} bytes) in {ControllerAction} action for user {userId}", file.Length, nameof(ProcessPdf), userId);
                return BadRequest("File is too big. File size must not exceed 5MB.");
            }

            string base64String;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            var httpClient = _httpClientFactory.CreateClient("OpenRouter");
            var llm = _configuration.GetValue<string>("OpenRouter:Models:PersonalDetailsPdfUploadModel") ?? throw new InvalidOperationException("PersonalDetailsPdfUploadModel not configured in OpenRouter settings.");
            var request = new
            {
                model = llm,
                stream = false,
                messages = new Message[]
                {
                new Message
                {
                    Role = "user",
                    Content = new List<Content>
                    {
                        new Content
                        {
                            Type = "text",
                            Text = Prompts.Prompts.ScrapeResume
                        },
                        new Content
                        {
                            Type = "file",
                            File = new FileOpenRouter
                            {
                                FileName = file.FileName,
                                FileData = $"data:application/pdf;base64,{base64String}"
                            }
                        }
                    }
                }
                }
            };

            var jsonContent = JsonSerializer.Serialize(request);
            var requestContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/v1/chat/completions", requestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            var openRouterResponse = JsonSerializer.Deserialize<OpenRouterCommonResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
            var content = openRouterResponse?.Choices.FirstOrDefault()?.Message.Content;
            return Ok(content);
        }

        [HttpGet]
        [Route("success-page-temporary")]
        public async Task<IActionResult> SuccessPageTemporary(CancellationToken cancellationToken)
        {
            return Ok("Success!");
        }
    }
}