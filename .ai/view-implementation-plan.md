# Plan wdrożenia funkcjonalności "Dodawanie nowej aplikacji"

## 1. Szczegóły implementacji endpointu w controllerze

### 1.1 Routing

**Aktualna struktura**: `[Route("job-applications")]` na poziomie kontrolera

**Endpoint GET - Display Create Application Form**
```csharp
[HttpGet]
[Route("create")]
public IActionResult Create()
{
    var viewModel = new CreateJobApplicationViewModel
    {
        Status = ApplicationStatus.Draft.Status // default status
    };
    return View(viewModel);
}
```

**Endpoint POST - Parse Job Description with AI**
```csharp
[HttpPost]
[Route("api/parse-job-description")]
public async Task<IActionResult> ParseJobDescription(
    [FromBody] ParseJobDescriptionViewModel vm, 
    CancellationToken cancellationToken)
{
    try
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new 
            { 
                success = false,
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        var response = await _commandDispatcher.DispatchAsync<
            ParseJobDescriptionCommand, 
            ParseJobDescriptionResponse>(
                new ParseJobDescriptionCommand(vm.JobDescriptionText), 
                cancellationToken);

        if (!response.IsSuccess)
        {
            return StatusCode(
                response.ProblemDetails?.Status ?? 500,
                new ParseJobDescriptionResultViewModel
                {
                    Success = false,
                    ParsingResult = ParsingResultType.Failed,
                    MissingFields = new List<string>(),
                    Data = null
                });
        }

        return Ok(new ParseJobDescriptionResultViewModel
        {
            Success = true,
            ParsingResult = response.ParsingResult,
            MissingFields = response.MissingFields ?? new List<string>(),
            Data = response.ParsedData
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error during parsing job description");
        return StatusCode(500, new ParseJobDescriptionResultViewModel
        {
            Success = false,
            ParsingResult = ParsingResultType.Failed,
            MissingFields = new List<string>(),
            Data = null
        });
    }
}
```

**Endpoint POST - Create New Application**
```csharp
[HttpPost]
[Route("create")]
public async Task<IActionResult> Create(
    CreateJobApplicationViewModel vm, 
    CancellationToken cancellationToken)
{
    if (!ModelState.IsValid)
    {
        return View(vm);
    }

    try
    {
        // Validate salary range if both provided
        if (vm.SalaryMin.HasValue && vm.SalaryMax.HasValue)
        {
            if (vm.SalaryMax < vm.SalaryMin)
            {
                ModelState.AddModelError(
                    nameof(vm.SalaryMax), 
                    "Maximum salary must be greater than or equal to minimum salary.");
                return View(vm);
            }
        }

        // Validate salary type and period if salary provided
        if ((vm.SalaryMin.HasValue || vm.SalaryMax.HasValue) && 
            !vm.SalaryType.HasValue)
        {
            ModelState.AddModelError(
                nameof(vm.SalaryType), 
                "Salary type is required when salary is specified.");
            return View(vm);
        }

        var jobApplicationId = await _commandDispatcher.DispatchAsync<
            CreateJobApplicationCommand, 
            Guid>(
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
```

### 1.2 Walidacja na poziomie kontrolera

- **ModelState validation**: Automatyczna walidacja przez Data Annotations
- **Business rules validation**: SalaryMax >= SalaryMin, SalaryType required if salary provided
- **Authorization**: `[Authorize]` attribute ensures user is authenticated

## 2. Szczegóły implementacji ViewModels

### 2.1 CreateJobApplicationViewModel

**Lokalizacja**: `CareerPilotAi/ViewModels/JobApplication/CreateJobApplicationViewModel.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.ViewModels.JobApplication
{
    public class CreateJobApplicationViewModel
    {
        [Required(ErrorMessage = "Company name is required")]
        [MinLength(2, ErrorMessage = "Company name must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required")]
        [MinLength(2, ErrorMessage = "Position must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "Position cannot exceed 100 characters")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job description is required")]
        [MinWords(50, ErrorMessage = "Job description must contain at least 50 words")]
        [MaxWords(5000, ErrorMessage = "Job description cannot exceed 5000 words")]
        public string JobDescription { get; set; } = string.Empty;

        [MinimumCount(0)]
        [MaximumCount(20, ErrorMessage = "You can add maximum 20 skills")]
        public List<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();

        [Required(ErrorMessage = "Experience level is required")]
        [AllowedValues(
            nameof(ExperienceLevel.Junior), 
            nameof(ExperienceLevel.Mid), 
            nameof(ExperienceLevel.Senior), 
            nameof(ExperienceLevel.NotSpecified))]
        public string ExperienceLevel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [MinLength(2, ErrorMessage = "Location must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Work mode is required")]
        [AllowedValues(
            nameof(WorkMode.Remote), 
            nameof(WorkMode.Hybrid), 
            nameof(WorkMode.OnSite))]
        public string WorkMode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contract type is required")]
        [AllowedValues(
            nameof(ContractType.B2B), 
            nameof(ContractType.FTE), 
            nameof(ContractType.Other))]
        public string ContractType { get; set; } = string.Empty;

        [Range(0, 999999, ErrorMessage = "Minimum salary must be between 0 and 999,999")]
        public decimal? SalaryMin { get; set; }

        [Range(0, 999999, ErrorMessage = "Maximum salary must be between 0 and 999,999")]
        public decimal? SalaryMax { get; set; }

        [AllowedValues(nameof(SalaryType.Gross), nameof(SalaryType.Net))]
        public string? SalaryType { get; set; }

        [AllowedValues(
            nameof(SalaryPeriodType.Monthly), 
            nameof(SalaryPeriodType.Daily), 
            nameof(SalaryPeriodType.Hourly), 
            nameof(SalaryPeriodType.Yearly))]
        public string? SalaryPeriod { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [MaxLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        public string? JobUrl { get; set; }

        [Required]
        [AllowedValues(
            nameof(ApplicationStatus.Draft),
            nameof(ApplicationStatus.Submitted),
            nameof(ApplicationStatus.InterviewScheduled),
            nameof(ApplicationStatus.WaitingForOffer),
            nameof(ApplicationStatus.ReceivedOffer),
            nameof(ApplicationStatus.Rejected),
            nameof(ApplicationStatus.NoContact))]
        public string Status { get; set; } = "Draft";
    }
}
```

### 2.2 SkillViewModel

**Lokalizacja**: `CareerPilotAi/ViewModels/JobApplication/SkillViewModel.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.ViewModels.JobApplication
{
    public class SkillViewModel
    {
        [Required(ErrorMessage = "Skill name is required")]
        [MinLength(2, ErrorMessage = "Skill name must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Skill name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Skill level is required")]
        [AllowedValues(
            nameof(SkillLevel.NiceToHave),
            nameof(SkillLevel.Regular),
            nameof(SkillLevel.Advanced),
            nameof(SkillLevel.Master))]
        public string Level { get; set; } = string.Empty;
    }
}
```

### 2.3 ParseJobDescriptionViewModel

**Lokalizacja**: `CareerPilotAi/ViewModels/JobApplication/ParseJobDescriptionViewModel.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.ViewModels.JobApplication
{
    public class ParseJobDescriptionViewModel
    {
        [Required(ErrorMessage = "Job description text is required")]
        [MaxWords(5000, ErrorMessage = "Job description cannot exceed 5000 words")]
        public string JobDescriptionText { get; set; } = string.Empty;
    }
}
```

### 2.4 ParseJobDescriptionResultViewModel

**Lokalizacja**: `CareerPilotAi/ViewModels/JobApplication/ParseJobDescriptionResultViewModel.cs`

```csharp
namespace CareerPilotAi.ViewModels.JobApplication
{
    public class ParseJobDescriptionResultViewModel
    {
        public bool Success { get; set; }
        public ParsingResultType ParsingResult { get; set; }
        public List<string> MissingFields { get; set; } = new List<string>();
        public CreateJobApplicationViewModel? Data { get; set; }
    }

    public enum ParsingResultType
    {
        FullSuccess,
        PartialSuccess,
        Failed
    }
}
```

### 2.5 Enums

**Lokalizacja**: `CareerPilotAi/Core/`

```csharp
// ExperienceLevel.cs
public class ExperienceLevel
{
    public const string Junior = "Junior";
    public const string Mid = "Mid";
    public const string Senior = "Senior";
    public const string NotSpecified = "NotSpecified";
}

// WorkMode.cs
public class WorkMode
{
    public const string Remote = "Remote";
    public const string Hybrid = "Hybrid";
    public const string OnSite = "OnSite";
}

// ContractType.cs
public class ContractType
{
    public const string B2B = "B2B";
    public const string FTE = "FTE";
    public const string Other = "Other";
}

// SkillLevel.cs
public class SkillLevel
{
    public const string NiceToHave = "NiceToHave";
    public const string Regular = "Regular";
    public const string Advanced = "Advanced";
    public const string Master = "Master";
}

// SalaryType.cs
public class SalaryType
{
    public const string Gross = "Gross";
    public const string Net = "Net";
}

// SalaryPeriodType.cs
public class SalaryPeriodType
{
    public const string Monthly = "Monthly";
    public const string Daily = "Daily";
    public const string Hourly = "Hourly";
    public const string Yearly = "Yearly";
}
```

## 3. Szczegóły implementacji Application Layer z Command Pattern

### 3.1 CreateJobApplicationCommand

**Lokalizacja**: `CareerPilotAi/Application/Commands/CreateJobApplication/CreateJobApplicationCommand.cs`

```csharp
using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.ViewModels.JobApplication;

namespace CareerPilotAi.Application.Commands.CreateJobApplication
{
    public record CreateJobApplicationCommand(
        CreateJobApplicationViewModel ViewModel) : ICommand<Guid>;
}
```

### 3.2 CreateJobApplicationCommandHandler

**Lokalizacja**: `CareerPilotAi/Application/Commands/CreateJobApplication/CreateJobApplicationCommandHandler.cs`

```csharp
using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Application.Commands.CreateJobApplication
{
    public class CreateJobApplicationCommandHandler : 
        ICommandHandler<CreateJobApplicationCommand, Guid>
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
}
```

### 3.3 ParseJobDescriptionCommand (Placeholder dla przyszłej implementacji)

**Lokalizacja**: `CareerPilotAi/Application/Commands/ParseJobDescription/ParseJobDescriptionCommand.cs`

```csharp
using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.ParseJobDescription
{
    public record ParseJobDescriptionCommand(
        string JobDescriptionText) : ICommand<ParseJobDescriptionResponse>;
}
```

### 3.4 ParseJobDescriptionResponse

**Lokalizacja**: `CareerPilotAi/Application/Commands/ParseJobDescription/ParseJobDescriptionResponse.cs`

```csharp
using CareerPilotAi.ViewModels.JobApplication;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.ParseJobDescription
{
    public class ParseJobDescriptionResponse
    {
        public bool IsSuccess { get; set; }
        public ParsingResultType ParsingResult { get; set; }
        public List<string>? MissingFields { get; set; }
        public CreateJobApplicationViewModel? ParsedData { get; set; }
        public ProblemDetails? ProblemDetails { get; set; }
    }
}
```

### 3.5 ParseJobDescriptionCommandHandler (Mock Implementation)

**Lokalizacja**: `CareerPilotAi/Application/Commands/ParseJobDescription/ParseJobDescriptionCommandHandler.cs`

```csharp
using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.ViewModels.JobApplication;

namespace CareerPilotAi.Application.Commands.ParseJobDescription
{
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
                    ExperienceLevel = "Senior",
                    Location = "Warsaw, Poland",
                    WorkMode = "Hybrid",
                    ContractType = "B2B",
                    SalaryMin = 15000,
                    SalaryMax = 20000,
                    SalaryType = "Net",
                    SalaryPeriod = "Monthly",
                    Skills = new List<SkillViewModel>
                    {
                        new() { Name = "C#", Level = "Advanced" },
                        new() { Name = "ASP.NET Core", Level = "Advanced" },
                        new() { Name = "Entity Framework", Level = "Regular" }
                    },
                    Status = "Draft"
                }
            };
        }
    }
}
```

### 3.6 Rejestracja Commands

**Lokalizacja**: `CareerPilotAi/Application/Commands/CommandsExtensions.cs`

Dodaj rejestrację nowego handlera:

```csharp
services.AddScoped<ICommandHandler<ParseJobDescriptionCommand, ParseJobDescriptionResponse>, 
    ParseJobDescriptionCommandHandler>();
```

## 4. Przepływ danych

### 4.1 Scenariusz 1: Tworzenie aplikacji z AI parsing

```
1. User → GET /job-applications/create
   ↓
2. Controller.Create() → View(new CreateJobApplicationViewModel())
   ↓
3. User wypełnia textarea i klika "Parse with AI"
   ↓
4. JavaScript → POST /job-applications/api/parse-job-description
   Body: { jobDescriptionText: "..." }
   ↓
5. Controller.ParseJobDescription(ParseJobDescriptionViewModel)
   ↓
6. ParseJobDescriptionCommand → ParseJobDescriptionCommandHandler
   ↓
7. Handler → Mock AI service (zwraca wyparsowane dane)
   ↓
8. Response → ParseJobDescriptionResponse
   ↓
9. Controller → OK(ParseJobDescriptionResultViewModel)
   ↓
10. JavaScript → wypełnia formularz danymi z response
    ↓
11. User → przegląda/edytuje dane, klika "Create Application"
    ↓
12. Form POST → /job-applications/create
    Body: CreateJobApplicationViewModel
    ↓
13. Controller.Create(CreateJobApplicationViewModel)
    → Walidacja ModelState
    → Walidacja business rules
    ↓
14. CreateJobApplicationCommand → CreateJobApplicationCommandHandler
    ↓
15. Handler → Tworzy JobApplicationDataModel + SkillDataModel
    → Zapisuje do DB w transakcji
    ↓
16. Response → Guid (jobApplicationId)
    ↓
17. Controller → RedirectToAction("Details", new { jobApplicationId })
    ↓
18. User widzi szczegóły utworzonej aplikacji
```

### 4.2 Scenariusz 2: Tworzenie aplikacji ręcznie

```
1. User → GET /job-applications/create
   ↓
2. Controller.Create() → View(new CreateJobApplicationViewModel())
   ↓
3. User wypełnia formularz ręcznie
   ↓
4. User klika "Create Application"
   ↓
5. Form POST → /job-applications/create
   ↓
[Dalszy przebieg jak w scenariuszu 1, kroki 13-18]
```

## 5. Implementacja DataModel z konfiguracją Entity Framework Core

### 5.1 JobApplicationDataModel (Rozszerzenie istniejącego)

**Lokalizacja**: `CareerPilotAi/Infrastructure/Persistence/DataModels/JobApplicationDataModel.cs`

```csharp
namespace CareerPilotAi.Infrastructure.Persistence.DataModels
{
    public class JobApplicationDataModel
    {
        public Guid JobApplicationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        
        // New properties
        public string ExperienceLevel { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string WorkMode { get; set; } = string.Empty;
        public string ContractType { get; set; } = string.Empty;
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? SalaryType { get; set; }
        public string? SalaryPeriod { get; set; }
        
        public string? Url { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<SkillDataModel> Skills { get; set; } = 
            new List<SkillDataModel>();
    }
}
```

### 5.2 SkillDataModel (Nowy)

**Lokalizacja**: `CareerPilotAi/Infrastructure/Persistence/DataModels/SkillDataModel.cs`

```csharp
namespace CareerPilotAi.Infrastructure.Persistence.DataModels
{
    public class SkillDataModel
    {
        public Guid SkillId { get; set; }
        public Guid JobApplicationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;

        // Navigation property
        public virtual JobApplicationDataModel JobApplication { get; set; } = null!;
    }
}
```

### 5.3 JobApplicationConfiguration (Rozszerzenie istniejącej)

**Lokalizacja**: `CareerPilotAi/Infrastructure/Persistence/Configurations/JobApplicationConfiguration.cs`

```csharp
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplicationDataModel>
    {
        public void Configure(EntityTypeBuilder<JobApplicationDataModel> builder)
        {
            builder.ToTable("JobApplications");

            // Primary Key
            builder.HasKey(j => j.JobApplicationId);

            // Properties
            builder.Property(j => j.JobApplicationId)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(j => j.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(j => j.Company)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(j => j.JobDescription)
                .IsRequired();

            builder.Property(j => j.ExperienceLevel)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.Location)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(j => j.WorkMode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.ContractType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.SalaryMin)
                .HasPrecision(18, 2);

            builder.Property(j => j.SalaryMax)
                .HasPrecision(18, 2);

            builder.Property(j => j.SalaryType)
                .HasMaxLength(50);

            builder.Property(j => j.SalaryPeriod)
                .HasMaxLength(50);

            builder.Property(j => j.Url)
                .HasMaxLength(500);

            builder.Property(j => j.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.CreatedAt)
                .IsRequired();

            builder.Property(j => j.UpdatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(j => j.User)
                .WithMany()
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.Skills)
                .WithOne(s => s.JobApplication)
                .HasForeignKey(s => s.JobApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(j => j.UserId);
            builder.HasIndex(j => j.Status);
            builder.HasIndex(j => j.CreatedAt);
        }
    }
}
```

### 5.4 SkillConfiguration (Nowa)

**Lokalizacja**: `CareerPilotAi/Infrastructure/Persistence/Configurations/SkillConfiguration.cs`

```csharp
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPilotAi.Infrastructure.Persistence.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<SkillDataModel>
    {
        public void Configure(EntityTypeBuilder<SkillDataModel> builder)
        {
            builder.ToTable("Skills");

            // Primary Key
            builder.HasKey(s => s.SkillId);

            // Properties
            builder.Property(s => s.SkillId)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(s => s.JobApplicationId)
                .IsRequired();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Level)
                .IsRequired()
                .HasMaxLength(50);

            // Indexes
            builder.HasIndex(s => s.JobApplicationId);
        }
    }
}
```

### 5.5 ApplicationDbContext (Aktualizacja)

**Lokalizacja**: `CareerPilotAi/Infrastructure/Persistence/ApplicationDbContext.cs`

Dodaj DbSet dla Skills:

```csharp
public DbSet<SkillDataModel> Skills { get; set; } = null!;
```

Dodaj konfigurację w OnModelCreating:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Existing configurations
    modelBuilder.ApplyConfiguration(new JobApplicationConfiguration());
    
    // New configuration
    modelBuilder.ApplyConfiguration(new SkillConfiguration());
}
```

### 5.6 Migration

Utworzyć nową migrację:

```bash
dotnet ef migrations add AddJobApplicationExtendedFields
dotnet ef database update
```

## 6. Obsługa błędów

### 6.1 Validation Errors (400 Bad Request)

**Scenariusze**:
- Brakujące wymagane pola (CompanyName, Position, JobDescription, etc.)
- JobDescription poza limitem słów (< 50 lub > 5000)
- Skill name/level invalid
- Skills count > 20
- Invalid enum values
- Invalid URL format
- SalaryMax < SalaryMin

**Implementacja**:
```csharp
if (!ModelState.IsValid)
{
    return View(vm); // Server-side: re-display form with errors
    // OR
    return BadRequest(ModelState); // API: return errors as JSON
}
```

**Response format dla API**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "CompanyName": ["Company name is required"],
    "JobDescription": ["Job description must contain at least 50 words"]
  }
}
```

### 6.2 Authentication Errors (401 Unauthorized)

**Scenariusz**: User nie jest zalogowany

**Implementacja**:
- `[Authorize]` attribute na kontrolerze
- ASP.NET Core automatycznie redirectuje do login page

### 6.3 Authorization Errors (403 Forbidden / 404 Not Found)

**Scenariusz**: User próbuje utworzyć aplikację dla innego użytkownika (mało prawdopodobne w Create, ale istotne w Edit/Delete)

**Implementacja**:
```csharp
var userId = _userService.GetUserIdOrThrowException();
// Use userId to ensure data belongs to authenticated user
```

### 6.4 Business Logic Errors (400 Bad Request)

**Scenariusze**:
- SalaryMax < SalaryMin
- SalaryType not provided when salary specified

**Implementacja**:
```csharp
if (vm.SalaryMin.HasValue && vm.SalaryMax.HasValue)
{
    if (vm.SalaryMax < vm.SalaryMin)
    {
        ModelState.AddModelError(nameof(vm.SalaryMax), 
            "Maximum salary must be greater than or equal to minimum salary.");
        return View(vm);
    }
}
```

### 6.5 Database Errors (500 Internal Server Error)

**Scenariusze**:
- Connection timeout
- Unique constraint violation (unlikely with Guids)
- Transaction rollback

**Implementacja**:
```csharp
try
{
    await _dbContext.SaveChangesAsync(cancellationToken);
    await transaction.CommitAsync(cancellationToken);
}
catch (Exception ex)
{
    await transaction.RollbackAsync(cancellationToken);
    _logger.LogError(ex, "Error creating job application");
    throw; // Let global exception handler catch it
}
```

### 6.6 AI Parsing Errors (500 Internal Server Error)

**Scenariusze**:
- OpenRouter API unavailable
- Timeout
- Invalid response format

**Implementacja** (w future, teraz mock zwraca success):
```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Error during parsing job description");
    return StatusCode(500, new ParseJobDescriptionResultViewModel
    {
        Success = false,
        ParsingResult = ParsingResultType.Failed,
        MissingFields = new List<string>(),
        Data = null
    });
}
```

### 6.7 Error Logging

Wszystkie błędy logowane przez `ILogger<T>`:

```csharp
_logger.LogError(ex, 
    "Error creating job application for user: {UserId}", 
    userId);
```

### 6.8 User-Friendly Error Messages

**Server-side (MVC)**:
- Validation errors: wyświetlane inline pod polami
- General errors: wyświetlane w validation summary na górze formularza
- Success: TempData message → toast notification

**Client-side (AJAX)**:
- Network errors: "Please check your connection and try again"
- Server errors: "Something went wrong. Please try again later"
- Validation errors: Display specific messages from response

## 7. Kroki implementacji

### Krok 1: Przygotowanie DataModel i EF Configuration

1. **Rozszerz JobApplicationDataModel**:
   - Dodaj nowe właściwości (ExperienceLevel, Location, WorkMode, ContractType, Salary fields)
   - Dodaj navigation property dla Skills

2. **Utwórz SkillDataModel**:
   - Właściwości: SkillId, JobApplicationId, Name, Level
   - Navigation property do JobApplication

3. **Utwórz/zaktualizuj EF Configurations**:
   - JobApplicationConfiguration: dodaj konfigurację nowych pól, relacji, indeksów
   - SkillConfiguration: nowa konfiguracja

4. **Zaktualizuj ApplicationDbContext**:
   - Dodaj DbSet<SkillDataModel>
   - ApplyConfiguration dla SkillConfiguration

5. **Utwórz migrację**:
   ```bash
   dotnet ef migrations add AddJobApplicationExtendedFields
   dotnet ef database update
   ```

### Krok 2: Implementacja ViewModels

1. **Utwórz klasy Enums** (w Core):
   - ExperienceLevel, WorkMode, ContractType
   - SkillLevel, SalaryType, SalaryPeriodType

2. **Utwórz SkillViewModel**:
   - Właściwości z walidacją

3. **Utwórz CreateJobApplicationViewModel**:
   - Wszystkie właściwości z walidacją Data Annotations
   - Lista Skills

4. **Utwórz ParseJobDescriptionViewModel**:
   - JobDescriptionText z walidacją

5. **Utwórz ParseJobDescriptionResultViewModel**:
   - Success, ParsingResult, MissingFields, Data

### Krok 3: Implementacja Application Layer

1. **Utwórz ParseJobDescriptionCommand i Response**:
   - Command: JobDescriptionText
   - Response: IsSuccess, ParsingResult, MissingFields, ParsedData, ProblemDetails

2. **Utwórz ParseJobDescriptionCommandHandler** (mock):
   - Implementuj mock response z przykładowymi danymi
   - Dodaj delay dla symulacji async operation

3. **Zaktualizuj CreateJobApplicationCommand**:
   - Zmień parametr na CreateJobApplicationViewModel

4. **Zaktualizuj CreateJobApplicationCommandHandler**:
   - Mapuj wszystkie nowe pola z ViewModel do DataModel
   - Utwórz Skills jeśli są dostępne
   - Użyj transakcji dla spójności danych

5. **Zarejestruj handlery** w CommandsExtensions

### Krok 4: Implementacja Controller

1. **Dodaj nowe endpointy w JobApplicationController**:
   - GET Create(): zwraca widok z pustym ViewModel
   - POST Create(CreateJobApplicationViewModel): przetwarza formularz
   - POST ParseJobDescription(ParseJobDescriptionViewModel): parsuje opis (mock)

2. **Dodaj walidację**:
   - ModelState validation
   - Business rules validation (salary range)

3. **Dodaj error handling**:
   - Try-catch blocks
   - Logging
   - User-friendly error messages

4. **Dodaj routing**:
   - Zaktualizuj route attributes

### Krok 5: Implementacja UI

1. **Utwórz Create.cshtml**:
   - Layout z Bootstrap 5
   - AI Parsing Section (collapsible accordion)
     - Textarea z word counter
     - Parse button
     - Loading state
     - Alert dla results
   - Form section (użyj partial view)
   - Form actions (Create, Cancel)

2. **Utwórz _JobApplicationForm.cshtml** (partial view):
   - Basic Information section (Company, Position, JobDescription)
   - Skills section (input + dropdown + Add button, tags list)
   - Job Details section (ExperienceLevel, Location, WorkMode, ContractType)
   - Salary section (optional, checkbox to enable)
   - Additional section (JobUrl, Status)
   - Validation messages
   - Bootstrap styling

3. **Dodaj JavaScript dla interakcji**:
   - Word counter dla textarea
   - AI Parsing AJAX call
     - Fetch POST do /job-applications/api/parse-job-description
     - Loading state (disable UI, show spinner)
     - Success: wypełnij formularz, pokaż alert
     - Error: pokaż error alert
   - Skills manager
     - Add skill: walidacja, dodanie tagu, clear input
     - Remove skill: usunięcie tagu
     - Counter update
   - Salary section toggle
   - Form validation (client-side)

4. **Dodaj CSS dla custom styling**:
   - Skills tags
   - Loading states
   - Alerts
   - Responsive design

### Krok 6: Testowanie

1. **Unit testy**:
   - CreateJobApplicationCommandHandler
   - ParseJobDescriptionCommandHandler (mock)
   - ViewModels validation

2. **Integration testy**:
   - Controller endpoints
   - Database operations
   - Transaction rollback

3. **Manual testing**:
   - Formularz validation (client + server)
   - AI Parsing (mock) flow
   - Create application flow (z AI i bez)
   - Error scenarios
   - Responsive design (mobile, tablet, desktop)

### Krok 7: Refaktoryzacja istniejącej funkcjonalności

1. **Zmień routing**:
   - Z `/entry-job-details` na `/create`

2. **Zaktualizuj ViewModels**:
   - Z `JobOfferEntryDetailsViewModel` na `CreateJobApplicationViewModel`

3. **Zaktualizuj widoki**:
   - Z `JobOfferEntryDetails.cshtml` na `Create.cshtml`

4. **Usuń stare pliki** (jeśli nie są używane):
   - JobOfferEntryDetailsViewModel
   - JobOfferEntryDetails.cshtml

5. **Zaktualizuj linki** w aplikacji:
   - Navbar: "Add New Application" → `/job-applications/create`
   - Dashboard: "Add Your First Application" → `/job-applications/create`

---

## Podsumowanie

Plan wdrożenia funkcjonalności "Dodawanie nowej aplikacji" obejmuje:

1. **Rozszerzenie DataModel** o nowe pola i relację Skills (one-to-many)
2. **Utworzenie ViewModels** z pełną walidacją Data Annotations
3. **Implementację Command Pattern** dla CreateJobApplication i ParseJobDescription
4. **Rozszerzenie Controller** o nowe endpointy z odpowiednią walidacją i obsługą błędów
5. **Utworzenie UI** z AI Parsing section, reużywalnym formularzem i interaktywnymi elementami
6. **Mock implementation** dla AI Parsing do testowania głównej funkcjonalności
7. **Kompleksową obsługę błędów** na wszystkich poziomach aplikacji
8. **Migrację EF Core** dla nowych tabel i kolumn

Funkcjonalność będzie zgodna z wymaganiami PRD, specyfikacją API i best practices dla ASP.NET Core MVC.

