# Controllers and Endpoints Plan - CareerPilotAi

## 1. Resources

The application manages the following primary resources:

- **Users** - User accounts and authentication
- **JobApplications** - Job application tracking records
- **Dashboard** - Aggregated statistics and metrics
- **Home** - Public pages and navigation

## 2. Controllers and Endpoints


### 2.2 JobApplicationController

Manages job application CRUD operations and related functionality.

#### 2.2.1 Display Application List (Job Board)
- **Method**: GET
- **Path**: `/JobApplication/Index`
- **Description**: Displays list of user's job applications with filters and search
- **Authentication**: Required (Authenticated user)
- **Query Parameters** (JobApplicationFiltersViewModel):
  - `searchTerm` (optional) - Search in company name and position
  - `statuses` (optional) - Array of status values for filtering
  - `minSalary` (optional) - Minimum salary filter
  - `maxSalary` (optional) - Maximum salary filter
  - `salaryPeriod` (optional) - monthly/daily/hourly/yearly (default: monthly)
  - `location` (optional) - Location filter (partial match)
  - `workModes` (optional) - Array of work modes (Remote/Hybrid/OnSite)
  - `experienceLevels` (optional) - Array of experience levels
  - `sortBy` (optional) - DateAddedDesc (default) / DateAddedAsc
- **Success Response**: 200 OK - Returns job board view with filtered applications
- **ViewModels**: JobApplicationListViewModel, JobApplicationFiltersViewModel

**JobApplicationFiltersViewModel Properties**:
- `SearchTerm` - string, optional, [MaxLength(100)]
- `Statuses` - List<ApplicationStatus>, optional
- `MinSalary` - decimal?, optional, [Range(0, 999999)]
- `MaxSalary` - decimal?, optional, [Range(0, 999999)]
- `SalaryPeriod` - SalaryPeriodType?, optional (Monthly/Daily/Hourly/Yearly)
- `Location` - string, optional, [MaxLength(100)]
- `WorkModes` - List<WorkMode>, optional (Remote/Hybrid/OnSite)
- `ExperienceLevels` - List<ExperienceLevel>, optional (Junior/Mid/Senior/NotSpecified)
- `SortBy` - SortOrder, optional (DateAddedDesc/DateAddedAsc)

**JobApplicationListViewModel Properties**:
- `Applications` - List<JobApplicationCardViewModel>
- `Filters` - JobApplicationFiltersViewModel
- `TotalCount` - int

**JobApplicationCardViewModel Properties**:
- `Id` - Guid
- `CompanyName` - string
- `Position` - string
- `Location` - string
- `WorkMode` - WorkMode enum
- `ContractType` - ContractType enum
- `Status` - ApplicationStatus enum
- `SalaryMin` - decimal?
- `SalaryMax` - decimal?
- `SalaryPeriod` - SalaryPeriodType?
- `Skills` - List<string> (first 3 skills)
- `SkillsCount` - int
- `CreatedAt` - DateTime

#### 2.2.2 Display Create Application Form
- **Method**: GET
- **Path**: `/JobApplication/Create`
- **Description**: Displays form for creating new job application
- **Authentication**: Required (Authenticated user)
- **Success Response**: 200 OK - Returns create application view
- **ViewModels**: CreateJobApplicationViewModel

#### 2.2.3 Parse Job Description with AI
- **Method**: POST
- **Path**: `/JobApplication/ParseJobDescription`
- **Description**: Parses job description text using AI and returns structured data
- **Authentication**: Required (Authenticated user)
- **Request Body** (ParseJobDescriptionViewModel):
```json
{
  "jobDescriptionText": "string (required, max 5000 words)"
}
```
- **Success Response**: 200 OK - Returns parsed job application data
```json
{
  "success": true,
  "parsingResult": "FullSuccess | PartialSuccess | Failed",
  "missingFields": ["array of field names if partial success"],
  "data": {
    "companyName": "string",
    "position": "string",
    "jobDescription": "string",
    "skills": [
      {
        "name": "string",
        "level": "NiceToHave | Regular | Advanced | Master"
      }
    ],
    "experienceLevel": "Junior | Mid | Senior | NotSpecified",
    "location": "string",
    "workMode": "Remote | Hybrid | OnSite",
    "contractType": "B2B | FTE | Zlecenie | Other",
    "salaryMin": "decimal (optional)",
    "salaryMax": "decimal (optional)",
    "salaryType": "Gross | Net",
    "salaryPeriod": "Monthly | Daily | Hourly | Yearly",
    "jobUrl": "string (optional)"
  }
}
```
- **Error Responses**:
  - 400 Bad Request - Validation errors (text too long)
  - 401 Unauthorized - Unauthorized access
  - 500 Internal Server Error - AI parsing failed
- **ViewModels**: ParseJobDescriptionViewModel, ParseJobDescriptionResultViewModel

**ParseJobDescriptionViewModel Properties**:
- `JobDescriptionText` - string, [Required], [MaxWords(5000)]

**ParseJobDescriptionResultViewModel Properties**:
- `Success` - bool
- `ParsingResult` - ParsingResultType enum (FullSuccess/PartialSuccess/Failed)
- `MissingFields` - List<string>
- `Data` - CreateJobApplicationViewModel

#### 2.2.4 Create New Application
- **Method**: POST
- **Path**: `/JobApplication/Create`
- **Description**: Creates new job application
- **Authentication**: Required (Authenticated user)
- **Request Body** (CreateJobApplicationViewModel):
```json
{
  "companyName": "string (required, 2-100 chars)",
  "position": "string (required, 2-100 chars)",
  "jobDescription": "string (required, 50-5000 words)",
  "skills": [
    {
      "name": "string (required, 2-50 chars)",
      "level": "NiceToHave | Regular | Advanced | Master"
    }
  ],
  "experienceLevel": "Junior | Mid | Senior | NotSpecified",
  "location": "string (required, 2-100 chars)",
  "workMode": "Remote | Hybrid | OnSite",
  "contractType": "B2B | FTE | Zlecenie | Other",
  "salaryMin": "decimal (optional)",
  "salaryMax": "decimal (optional)",
  "salaryType": "Gross | Net",
  "salaryPeriod": "Monthly | Daily | Hourly | Yearly",
  "jobUrl": "string (optional, valid URL)",
  "status": "Draft (default)"
}
```
- **Success Response**: 302 Redirect to `/JobApplication/Details/{id}`
- **Error Responses**:
  - 400 Bad Request - Validation errors
- **ViewModels**: CreateJobApplicationViewModel

**CreateJobApplicationViewModel Properties**:
- `CompanyName` - string, [Required], [MinLength(2)], [MaxLength(100)]
- `Position` - string, [Required], [MinLength(2)], [MaxLength(100)]
- `JobDescription` - string, [Required], [MinWords(50)], [MaxWords(5000)]
- `Skills` - List<SkillViewModel>, optional, [MaximumCount(20)]
- `ExperienceLevel` - ExperienceLevel enum, [Required], [AllowedValues]
- `Location` - string, [Required], [MinLength(2)], [MaxLength(100)]
- `WorkMode` - WorkMode enum, [Required], [AllowedValues]
- `ContractType` - ContractType enum, [Required], [AllowedValues]
- `SalaryMin` - decimal?, optional, [Range(0, 999999)]
- `SalaryMax` - decimal?, optional, [Range(0, 999999)]
- `SalaryType` - SalaryType? enum, optional (Gross/Net)
- `SalaryPeriod` - SalaryPeriodType? enum, optional (Monthly/Daily/Hourly/Yearly)
- `JobUrl` - string, optional, [Url], [MaxLength(500)]
- `Status` - ApplicationStatus enum, default: Draft

**SkillViewModel Properties**:
- `Name` - string, [Required], [MinLength(2)], [MaxLength(50)]
- `Level` - SkillLevel enum, [Required], [AllowedValues] (NiceToHave/Regular/Advanced/Master)

#### 2.2.5 Display Application Details
- **Method**: GET
- **Path**: `/JobApplication/Details/{id}`
- **Description**: Displays full details of a job application
- **Authentication**: Required (Authenticated user - must own the application)
- **URL Parameters**:
  - `id` (required) - GUID of the job application
- **Success Response**: 200 OK - Returns application details view
- **Error Responses**:
  - 404 Not Found - Application not found or user doesn't own it
- **ViewModels**: JobApplicationDetailsViewModel

**JobApplicationDetailsViewModel Properties**:
- `Id` - Guid
- `CompanyName` - string
- `Position` - string
- `JobDescription` - string
- `Skills` - List<SkillViewModel>
- `ExperienceLevel` - ExperienceLevel enum
- `Location` - string
- `WorkMode` - WorkMode enum
- `ContractType` - ContractType enum
- `SalaryMin` - decimal?
- `SalaryMax` - decimal?
- `SalaryType` - SalaryType? enum
- `SalaryPeriod` - SalaryPeriodType? enum
- `JobUrl` - string
- `Status` - ApplicationStatus enum
- `CreatedAt` - DateTime
- `UpdatedAt` - DateTime

#### 2.2.6 Display Edit Application Form
- **Method**: GET
- **Path**: `/JobApplication/Edit/{id}`
- **Description**: Displays form for editing existing job application
- **Authentication**: Required (Authenticated user - must own the application)
- **URL Parameters**:
  - `id` (required) - GUID of the job application
- **Success Response**: 200 OK - Returns edit application view with pre-filled data
- **Error Responses**:
  - 404 Not Found - Application not found or user doesn't own it
- **ViewModels**: EditJobApplicationViewModel

#### 2.2.7 Update Application
- **Method**: POST
- **Path**: `/JobApplication/Edit/{id}`
- **Description**: Updates existing job application
- **Authentication**: Required (Authenticated user - must own the application)
- **URL Parameters**:
  - `id` (required) - GUID of the job application
- **Request Body** (EditJobApplicationViewModel):
```json
{
  "id": "guid (required)",
  "companyName": "string (required, 2-100 chars)",
  "position": "string (required, 2-100 chars)",
  "jobDescription": "string (required, 50-5000 words)",
  "skills": [
    {
      "name": "string (required, 2-50 chars)",
      "level": "NiceToHave | Regular | Advanced | Master"
    }
  ],
  "experienceLevel": "Junior | Mid | Senior | NotSpecified",
  "location": "string (required, 2-100 chars)",
  "workMode": "Remote | Hybrid | OnSite",
  "contractType": "B2B | FTE | Zlecenie | Other",
  "salaryMin": "decimal (optional)",
  "salaryMax": "decimal (optional)",
  "salaryType": "Gross | Net",
  "salaryPeriod": "Monthly | Daily | Hourly | Yearly",
  "jobUrl": "string (optional, valid URL)",
  "status": "Draft | Submitted | InterviewScheduled | WaitingForOffer | ReceivedOffer | Rejected | NoContact"
}
```
- **Success Response**: 302 Redirect to `/JobApplication/Details/{id}`
- **Error Responses**:
  - 400 Bad Request - Validation errors
  - 404 Not Found - Application not found or user doesn't own it
- **ViewModels**: EditJobApplicationViewModel

**EditJobApplicationViewModel Properties**:
- Same as CreateJobApplicationViewModel plus:
- `Id` - Guid, [Required]

#### 2.2.8 Delete Application
- **Method**: POST
- **Path**: `/JobApplication/Delete/{id}`
- **Description**: Permanently deletes a job application
- **Authentication**: Required (Authenticated user - must own the application)
- **URL Parameters**:
  - `id` (required) - GUID of the job application
- **Success Response**: 302 Redirect to `/JobApplication/Index` with success message
- **Error Responses**:
  - 404 Not Found - Application not found or user doesn't own it
- **ViewModels**: None

#### 2.2.9 Update Application Status (Quick Action)
- **Method**: POST
- **Path**: `/JobApplication/UpdateStatus/{id}`
- **Description**: Updates only the status of a job application (quick action)
- **Authentication**: Required (Authenticated user - must own the application)
- **URL Parameters**:
  - `id` (required) - GUID of the job application
- **Request Body** (UpdateStatusViewModel):
```json
{
  "status": "Draft | Submitted | InterviewScheduled | WaitingForOffer | ReceivedOffer | Rejected | NoContact"
}
```
- **Success Response**: 200 OK - Returns success with updated status
```json
{
  "success": true,
  "newStatus": "Submitted"
}
```
- **Error Responses**:
  - 400 Bad Request - Invalid status value
  - 404 Not Found - Application not found or user doesn't own it
- **ViewModels**: UpdateStatusViewModel

**UpdateStatusViewModel Properties**:
- `Status` - ApplicationStatus enum, [Required], [AllowedValues]

### 2.3 HomeController

Manages dashboard, home page, and general navigation.

#### 2.3.1 Display Landing Page
- **Method**: GET
- **Path**: `/` or `/Home/Index`
- **Description**: Displays landing page
- **Authentication**: Anonymous
- **Success Response**: 
  - 200 OK - Returns landing page view (if unauthenticated)
- **ViewModels**: None

#### 2.3.2 Display Dashboard
- **Method**: GET
- **Path**: `/Home/Dashboard`
- **Description**: Displays user dashboard with statistics and charts
- **Authentication**: Required (Authenticated user)
- **Query Parameters**:
  - `timeframe` (optional) - 30/60/90 days for timeline chart (default: 30)
- **Success Response**: 200 OK - Returns dashboard view with metrics
- **ViewModels**: DashboardViewModel

**DashboardViewModel Properties**:
- `Metrics` - ApplicationMetricsViewModel
- `StatusDistribution` - List<StatusDistributionItemViewModel>
- `Timeline` - ApplicationTimelineViewModel
- `RecentApplications` - List<RecentApplicationViewModel>

**ApplicationMetricsViewModel Properties**:
- `TotalApplications` - int
- `SubmittedApplications` - int
- `InterviewsScheduled` - int
- `OffersReceived` - int

**StatusDistributionItemViewModel Properties**:
- `Status` - ApplicationStatus enum
- `Count` - int
- `Percentage` - decimal

**ApplicationTimelineViewModel Properties**:
- `Timeframe` - int (30/60/90)
- `DataPoints` - List<TimelineDataPointViewModel>

**TimelineDataPointViewModel Properties**:
- `Date` - DateTime
- `Count` - int

**RecentApplicationViewModel Properties**:
- `Id` - Guid
- `CompanyName` - string
- `Position` - string
- `Status` - ApplicationStatus enum
- `CreatedAt` - DateTime

#### 2.3.3 Display Privacy Policy
- **Method**: GET
- **Path**: `/Home/Privacy`
- **Description**: Displays privacy policy page
- **Authentication**: Anonymous
- **Success Response**: 200 OK - Returns privacy policy view
- **ViewModels**: None

#### 2.3.4 Display Error Page
- **Method**: GET
- **Path**: `/Home/Error`
- **Description**: Displays error page for unhandled exceptions
- **Authentication**: Anonymous
- **Success Response**: 200 OK - Returns error view
- **ViewModels**: ErrorViewModel

**ErrorViewModel Properties**:
- `RequestId` - string
- `ShowRequestId` - bool

### 2.4 InterviewQuestionsController

Manages interview preparation features (out of MVP scope but included for future).

## 3. Authentication and Authorization

### 3.1 Authentication Mechanism

The application uses **ASP.NET Core Identity** for authentication and authorization:

- **Cookie-based authentication** for web sessions
- **Email and password** as primary credentials
- **Remember Me** functionality for persistent login
- **Email confirmation** tokens for account verification
- **Password reset** tokens with 24-hour expiration

### 3.2 Implementation Details

#### 3.2.1 Authentication Configuration
```csharp
// In Program.cs
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    
    // Email settings
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
});
```

### 3.3 Security Measures

1. **CSRF Protection**: Automatic anti-forgery token validation on all POST/PUT/DELETE requests
2. **XSS Prevention**: Automatic HTML encoding in Razor views
3. **SQL Injection Prevention**: Parameterized queries via Entity Framework Core
4. **Secure Password Storage**: ASP.NET Identity password hashing
5. **Rate Limiting**: Consider implementing for authentication endpoints
6. **HTTPS Enforcement**: Redirect HTTP to HTTPS in production

## 4. Validation and Business Logic

### 4.1 Validation Rules by Resource

#### 4.1.1 User Registration
- **Email**:
  - Required
  - Valid email format
  - Unique in system
  - Custom validation: Enhanced email validation (no disposable emails recommended)
- **Password**:
  - Required
  - Minimum 8 characters
  - Must contain uppercase letter
  - Must contain digit
  - Must contain lowercase letter
- **ConfirmPassword**:
  - Required
  - Must match Password

#### 4.1.2 Job Application
- **CompanyName**:
  - Required
  - Minimum length: 2 characters
  - Maximum length: 100 characters
- **Position**:
  - Required
  - Minimum length: 2 characters
  - Maximum length: 100 characters
- **JobDescription**:
  - Required
  - Minimum: 50 words
  - Maximum: 5000 words
  - Custom validation: Word count validation
- **Skills**:
  - Optional collection
  - Maximum 20 items
  - Each skill:
    - Name: Required, 2-50 characters
    - Level: Required, one of [NiceToHave, Regular, Advanced, Master]
- **ExperienceLevel**:
  - Required
  - One of [Junior, Mid, Senior, NotSpecified]
- **Location**:
  - Required
  - Minimum length: 2 characters
  - Maximum length: 100 characters
- **WorkMode**:
  - Required
  - One of [Remote, Hybrid, OnSite]
- **ContractType**:
  - Required
  - One of [B2B, FTE, Zlecenie, Other]
- **Salary** (Min/Max):
  - Optional
  - Range: 0 to 999,999
  - If both provided: Max must be >= Min
  - Currency: PLN only (hardcoded)
- **SalaryType**:
  - Optional (required if salary provided)
  - One of [Gross, Net]
- **SalaryPeriod**:
  - Optional (required if salary provided)
  - One of [Monthly, Daily, Hourly, Yearly]
  - Default: Monthly
- **JobUrl**:
  - Optional
  - Valid URL format
  - Maximum length: 500 characters
- **Status**:
  - Required
  - One of [Draft, Submitted, InterviewScheduled, WaitingForOffer, ReceivedOffer, Rejected, NoContact]
  - Default: Draft

### 4.2 Business Logic Implementation

#### 4.2.1 AI Job Description Parsing

**Process**:
1. User pastes job description text (max 5000 words)
2. Frontend validates text length
3. POST to `/JobApplication/ParseJobDescription`
4. Backend sends text to OpenRouter API
5. AI attempts to extract structured data
6. System classifies result:
   - **Full Success**: All required fields parsed
   - **Partial Success**: Some fields missing, list provided
   - **Failure**: Unable to parse, show error with retry option
7. Return parsed data to frontend
8. Frontend populates form with parsed data
9. User reviews/edits and submits


#### 4.2.4 Dashboard Metrics Calculation

**Metrics**:
1. **Total Applications**: Count of all applications
2. **Submitted**: Count with status = Submitted
3. **Interviews**: Count with status = InterviewScheduled
4. **Offers**: Count with status = ReceivedOffer

**Status Distribution**:
- Group by status
- Calculate count and percentage for each status
- Order by count descending

**Timeline**:
- Group applications by date within selected timeframe
- Aggregate counts per day/week
- Fill gaps with zero counts for complete timeline

**Recent Activity**:
- Last 5 applications ordered by CreatedAt descending

### 4.3 Error Handling

#### 4.3.1 Validation Errors
- Return 400 Bad Request with ModelState errors
- Display validation errors inline in forms
- Red border around invalid fields
- Clear error messages below each field

#### 4.3.2 Authentication Errors
- 401 Unauthorized for invalid credentials
- Redirect to login page for unauthenticated access
- Display friendly error messages (avoid revealing security details)

#### 4.3.3 Authorization Errors
- 403 Forbidden for insufficient permissions
- 404 Not Found for resources user doesn't own (to avoid information leakage)

#### 4.3.4 AI Processing Errors
- Graceful degradation when AI parsing fails
- Allow user to proceed with manual entry
- Log errors for monitoring
- Display user-friendly retry options

#### 4.3.5 Database Errors
- 500 Internal Server Error for unexpected failures
- Log full error details server-side
- Display generic error message to user
- Redirect to error page for unhandled exceptions

## 5. Enums and Types

### 5.1 ApplicationStatus
```csharp
public enum ApplicationStatus
{
    Draft,
    Submitted,
    InterviewScheduled,
    WaitingForOffer,
    ReceivedOffer,
    Rejected,
    NoContact
}
```

### 5.2 WorkMode
```csharp
public enum WorkMode
{
    Remote,
    Hybrid,
    OnSite
}
```

### 5.3 ContractType
```csharp
public enum ContractType
{
    B2B,
    FTE,
    Zlecenie,
    Other
}
```

### 5.4 ExperienceLevel
```csharp
public enum ExperienceLevel
{
    Junior,
    Mid,
    Senior,
    NotSpecified
}
```

### 5.5 SkillLevel
```csharp
public enum SkillLevel
{
    NiceToHave,
    Regular,
    Advanced,
    Master
}
```

### 5.6 SalaryType
```csharp
public enum SalaryType
{
    Gross,
    Net
}
```

### 5.7 SalaryPeriodType
```csharp
public enum SalaryPeriodType
{
    Monthly,
    Daily,
    Hourly,
    Yearly
}
```

### 5.8 ParsingResultType
```csharp
public enum ParsingResultType
{
    FullSuccess,
    PartialSuccess,
    Failed
}
```

### 5.9 SortOrder
```csharp
public enum SortOrder
{
    DateAddedDesc,
    DateAddedAsc
}
```

## 6. Response Formats and Status Codes

### 6.1 Success Responses

- **200 OK**: Successful GET request, returns view or data
- **201 Created**: Resource created (not used in MVC pattern)
- **302 Found**: Redirect after successful POST operation
- **204 No Content**: Successful operation with no content to return

### 6.2 Client Error Responses

- **400 Bad Request**: Validation errors, malformed request
- **401 Unauthorized**: Authentication required or failed
- **403 Forbidden**: User doesn't have permission
- **404 Not Found**: Resource doesn't exist or user doesn't own it
- **409 Conflict**: Resource conflict (e.g., duplicate email)

### 6.3 Server Error Responses

- **500 Internal Server Error**: Unexpected server error
- **503 Service Unavailable**: External service (AI, email) unavailable

## 7. Additional Considerations

### 7.1 AJAX Endpoints

For enhanced UX, some operations can be AJAX-enabled:
- Status updates (UpdateStatus endpoint returns JSON)
- AI parsing (ParseJobDescription returns JSON)
- Filter/search operations (can be enhanced to return partial view)

### 7.2 Toast Notifications

Success/error messages displayed via toast notifications:
- After successful creation: "Application created successfully"
- After successful update: "Changes saved"
- After successful deletion: "Application deleted"
- After status change: "Status updated to [New Status]"

### 7.3 Loading States

Display loading indicators for:
- AI parsing operations ("Analyzing job description...")
- Dashboard data loading
- Form submissions