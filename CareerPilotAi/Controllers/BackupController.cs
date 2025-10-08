// using CareerPilotAi.Core;
// using CareerPilotAi.Infrastructure.OpenRouter;
// using CareerPilotAi.Infrastructure.Persistence;
// using CareerPilotAi.Infrastructure.Persistence.DataModels;
// using CareerPilotAi.ViewModels.JobApplication;
// using CareerPilotAi.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace CareerPilotAi.Controllers
// {
//     [Route("job-applications")]
//     [Authorize]
//     public class BackupController : Controller
//     {
//         private readonly IUserService _userService;
//         private readonly ApplicationDbContext _applicationDbContext;
//         private readonly ILogger<BackupController> _logger;
//         private readonly IHttpClientFactory _httpClientFactory;
//         private readonly IConfiguration _configuration;
//         private readonly OpenRouterService _openRouterService;

//         public BackupController(IUserService userService, ApplicationDbContext applicationDbContext, 
//             ILogger<BackupController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, OpenRouterService openRouterService)
//         {
//             _userService = userService;
//             _applicationDbContext = applicationDbContext;
//             _logger = logger;
//             _httpClientFactory = httpClientFactory;
//             _configuration = configuration;
//             _openRouterService = openRouterService;
//         }

//         [HttpGet]
//         [Route("personal-details")]
//         public async Task<IActionResult> ProvidePersonalDetails([FromQuery] Guid jobApplicationId, CancellationToken cancellationToken)
//         {
//             if (jobApplicationId == Guid.Empty)
//             {
//                 _logger.LogError("JobApplicationId is empty, cannot process {ControllerAction} action", nameof(ProvidePersonalDetails));
//                 return RedirectToAction(nameof(Index));
//             }

//             var userId = _userService.GetUserIdOrThrowException();

//             var jobApplicationDataModel = await _applicationDbContext.JobApplications
//                 .AsNoTracking()
//                 .Select(j => new
//                 {
//                     Id = j.JobApplicationId,
//                     PersonalDetails = j.PersonalDetails_Text,
//                     j.UserId
//                 })
//                 .FirstOrDefaultAsync(j => j.Id == jobApplicationId && j.UserId == userId, cancellationToken);

//             if (jobApplicationDataModel is null)
//             {
//                 _logger.LogCritical("Cannot find jobApplicationDataModel with jobApplicationId: {jobApplicationId} and userId: {userId}", jobApplicationId, userId);
//                 return RedirectToAction(nameof(Index));
//             }

//             return View(new UserDetailsViewModel()
//             {
//                 JobApplicationId = jobApplicationDataModel.Id,
//                 PersonalDetails = jobApplicationDataModel.PersonalDetails ?? ""
//             });
//         }

//         [HttpPost]
//         [Route("personal-details")]
//         public async Task<IActionResult> SaveUserDetails(UserDetailsViewModel vm, CancellationToken cancellationToken)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return View("ProvidePersonalDetails", vm);
//             }

//             var userId = _userService.GetUserIdOrThrowException();
//             var jobApplicationDataModel = await _applicationDbContext.JobApplications
//                 .FirstOrDefaultAsync(j => j.JobApplicationId == vm.JobApplicationId && j.UserId == userId, cancellationToken);

//             if (jobApplicationDataModel is null)
//             {
//                 _logger.LogCritical("Cannot find jobApplicationDataModel with jobApplicationId: {jobApplicationId} and userId: {userId}", vm.JobApplicationId, userId);
//                 return View("ProvidePersonalDetails", vm);
//             }

//             var jobApplication = new JobApplication(jobApplicationDataModel.JobApplicationId, jobApplicationDataModel.UserId, new EntryJobDetails(jobApplicationDataModel.EntryJobDetails_Text)
//                 , new PersonalDetails(vm.PersonalDetails));

//             jobApplicationDataModel.PersonalDetails_Text = jobApplication.PersonalDetails.Text;
//             await _applicationDbContext.SaveChangesAsync(cancellationToken);

//             _logger.LogInformation("User {userId} saved personal details for job application {jobApplicationId}", userId, jobApplication.JobApplicationId);

//             return RedirectToAction(nameof(SuccessPageTemporary));
//         }

//         [HttpPost]
//         [Route("process-pdf")]
//         public async Task<IActionResult> ProcessPdf(IFormFile file, CancellationToken cancellationToken)
//         {
//             var userId = _userService.GetUserIdOrThrowException();

//             #region Pdf File Validation

//             if (file == null || file.Length == 0)
//             {
//                 _logger.LogError("No file uploaded in {ControllerAction} action for user {userId}", nameof(ProcessPdf), userId);
//                 return BadRequest("No file has been uploaded.");
//             }
            
//             // Check if the file is a PDF
//             if (file.ContentType != "application/pdf" && !file.FileName.EndsWith(".pdf"))
//             {
//                 _logger.LogError("Invalid file type uploaded {invalidType} in {ControllerAction} action for user {userId}. Expected PDF.", file.ContentType, nameof(ProcessPdf), userId);
//                 return BadRequest($"Invalid file type uploaded {file.ContentType}. Only Pdf format is accepted.");
//             }

//             // Validate file size if needed (optional)
//             if (file.Length > 5242880) // 5MB limit
//             {
//                 _logger.LogError("File too large ({fileSize} bytes) in {ControllerAction} action for user {userId}", file.Length, nameof(ProcessPdf), userId);
//                 return BadRequest("File is too big. File size must not exceed 5MB.");
//             }

//             #endregion

//             string base64String;
//             using (var fileStream = file.OpenReadStream())
//             {
//                 var buffer = new byte[file.Length];
//                 await fileStream.ReadExactlyAsync(buffer, cancellationToken);
//                 base64String = Convert.ToBase64String(buffer);
//             }

//             var response = await _openRouterService.ScrapePersonalInformationPdf(file.FileName, base64String, cancellationToken);
//             return Ok(response);
//         }

//         [HttpGet]
//         [Route("test")]
//         public IActionResult Test()
//         {
//             return View("PersonalInformationDetails", new PersonalInformationViewModel());
//         }

//         [HttpGet]
//         [Route("success-page-temporary")]
//         public async Task<IActionResult> SuccessPageTemporary(CancellationToken cancellationToken)
//         {
//             return Ok("Success!");
//         }
//     }
// }