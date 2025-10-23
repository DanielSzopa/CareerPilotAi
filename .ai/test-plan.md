# CareerPilotAi - Comprehensive Test Plan

## Executive Summary

This document provides a comprehensive test plan for the CareerPilotAi application, a web-based job application management system built with ASP.NET Core MVC and .NET 8. The plan covers unit testing, integration testing, and end-to-end testing strategies using the specified technologies: xUnit, NSubstitute, Bogus, Shouldly, and Playwright.

---

## 1. Testing Technology Setup and Recommendations

### 1.1 xUnit Configuration

**Setup Recommendations:**
- Version: 2.4.1+ (ensure compatibility with .NET 8)
- Configure test discovery through xUnit.Runner.VisualStudio
- Use TestCaseOrderer for deterministic test execution where needed
- Set up Trait-based test categorization for organizing large test suites

**Project Structure:**
```
tests/
├── CareerPilotAi.UnitTests/
├── CareerPilotAi.IntegrationTests/
└── CareerPilotAi.E2ETests/
```

### 1.2 NSubstitute Configuration

**Best Practices:**
- Create mock factories for commonly mocked services
- Use `Substitute.For<T>()` for interface mocking
- Configure returns using `.Returns()` for synchronous operations
- Use `.Returns(x => ...)` for dynamic return values
- Implement mock verification for critical paths

### 1.3 Bogus Data Generation

**Setup:** Version 34.0+ with .NET 8 support

**Example Faker:**
```csharp
public class JobApplicationFaker : Faker<JobApplicationDataModel>
{
    public JobApplicationFaker()
    {
        RuleFor(ja => ja.JobApplicationId, f => f.Random.Guid());
        RuleFor(ja => ja.Title, f => f.Job.Title());
        RuleFor(ja => ja.Company, f => f.Company.CompanyName());
        RuleFor(ja => ja.Status, f => f.PickRandom(ApplicationStatus.ValidStatuses));
    }
}
```

### 1.4 Shouldly Assertions

**Configuration:** Version 4.1.0+

**Usage Pattern:**
```csharp
result.Should().NotBeNull();
result.ShouldBe(expectedValue);
items.ShouldHaveCount(5);
```

### 1.5 PostgreSQL Test Container Setup

**Docker Configuration:**
- Use TestContainers for PostgreSQL (port 5433 for isolation)
- Implement database initialization in test fixtures
- Use Respawn library to reset database state between tests

### 1.6 Playwright Configuration

**Setup:**
- Install: `Install-Package Microsoft.Playwright`
- Run: `pwsh bin/Debug/net8.0/playwright.ps1 install`
- Version: 1.40+

**Test Setup Pattern:**
```csharp
[Collection("Playwright collection")]
public class AuthenticationE2ETests : IAsyncLifetime
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IPage _page;
    private const string BaseUrl = "https://localhost:7001";

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        _page = await _browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _page?.CloseAsync();
        await _browser?.CloseAsync();
        _playwright?.Dispose();
    }
}
```

---

## 2. Unit Tests Section

### 2.1 Custom Validation Attributes

#### 2.1.1 EnhancedEmailAttribute Tests

**File:** `tests/CareerPilotAi.UnitTests/CustomValidationAttributes/EnhancedEmailAttributeTests.cs`

**Test Cases:**

1. **Valid Email Addresses**
   - Standard email: `user@example.com` → Should pass
   - Subdomain: `user@mail.example.com` → Should pass
   - Numbers: `user123@example.com` → Should pass
   - Special characters (RFC 5322): `user+tag@example.com` → Should pass
   - Quoted local part: `"user.name"@example.com` → Should pass

2. **Invalid Email Addresses**
   - Missing @ symbol: `userexample.com` → Should fail
   - Double @ symbols: `user@@example.com` → Should fail
   - No domain extension: `user@example` → Should fail
   - Spaces in email: `user @example.com` → Should fail
   - Invalid TLD: `user@example.c` → Should fail

3. **Edge Cases**
   - Null value → Should fail
   - Empty string → Should fail
   - Maximum length (254 characters) → Should pass
   - Exceeding 254 characters → Should fail

#### 2.1.2 MaxWordsAttribute Tests

**File:** `tests/CareerPilotAi.UnitTests/CustomValidationAttributes/MaxWordsAttributeTests.cs`

**Test Cases:**

1. **Valid Word Count**
   - Exactly at limit (5000 words) → Should pass
   - Below limit (2500 words) → Should pass
   - One word → Should pass

2. **Invalid Word Count**
   - Exceeds limit (5001 words) → Should fail
   - Significantly exceeds limit (10000 words) → Should fail

3. **Edge Cases**
   - Multiple spaces between words: "word  word" → Should count as 2 words
   - Tabs and newlines: "word\tword\nword" → Should count as 3 words
   - Punctuation: "hello, world!" → Should count as 2 words
   - Empty or null input → Should fail

#### 2.1.3 MinWordsAttribute Tests

**File:** `tests/CareerPilotAi.UnitTests/CustomValidationAttributes/MinWordsAttributeTests.cs`

**Test Cases:**

1. **Valid Word Count**
   - Exactly minimum words (50) → Should pass
   - More than minimum (100) → Should pass

2. **Invalid Word Count**
   - Below minimum (49) → Should fail
   - Empty string → Should fail

#### 2.1.4 ValidSalaryRangeAttribute Tests

**File:** `tests/CareerPilotAi.UnitTests/CustomValidationAttributes/ValidSalaryRangeAttributeTests.cs`

**Test Cases:**

1. **Valid Salary Ranges**
   - Both salaries equal: SalaryMin = 50000, SalaryMax = 50000 → Should pass
   - Max greater than min: SalaryMin = 40000, SalaryMax = 60000 → Should pass

2. **Invalid Salary Ranges**
   - Max less than min: SalaryMin = 60000, SalaryMax = 40000 → Should fail

3. **Partial Values**
   - Only min provided → Should pass (optional validation)
   - Only max provided → Should pass
   - Both null → Should pass

#### 2.1.5 SalaryTypeRequiredIfSalaryProvidedAttribute Tests

**File:** `tests/CareerPilotAi.UnitTests/CustomValidationAttributes/SalaryTypeRequiredIfSalaryProvidedAttributeTests.cs`

**Test Cases:**

1. **Salary Provided with Type**
   - SalaryMin provided with SalaryType → Should pass
   - SalaryMax provided with SalaryType → Should pass

2. **Salary Provided Without Type**
   - SalaryMin provided without SalaryType → Should fail
   - Both salaries without SalaryType → Should fail

3. **No Salary Provided**
   - All null values → Should pass

#### 2.1.6 MinimumCountAttribute Tests

**File:** `tests/CareerPilotAi.UnitTests/CustomValidationAttributes/MinimumCountAttributeTests.cs`

**Test Cases:**

1. **Valid Collections**
   - List with exact minimum count → Should pass
   - List exceeding minimum count → Should pass
   - Array with sufficient items → Should pass

2. **Invalid Collections**
   - List below minimum count → Should fail with appropriate error message
   - Empty collection → Should fail
   - Null collection → Should fail

3. **Edge Cases**
   - IEnumerable with sufficient items → Should pass
   - Non-collection value → Should fail with "Value must be a collection"
   - Custom minimum count (e.g., 5 items minimum) → Should validate correctly

#### 2.1.7 MaximumCountAttribute Tests

**File:** `tests/CareerPilotAi.UnitTests/CustomValidationAttributes/MaximumCountAttributeTests.cs`

**Test Cases:**

1. **Valid Collections**
   - List with count below maximum → Should pass
   - List with exact maximum count → Should pass
   - Empty collection → Should pass (allows null values)

2. **Invalid Collections**
   - List exceeding maximum count → Should fail with appropriate error message
   - Array with too many items → Should fail

3. **Edge Cases**
   - IEnumerable with excessive items → Should fail
   - Non-collection value → Should fail with "Value must be a collection"
   - Custom maximum count (e.g., 10 items maximum) → Should validate correctly

### 2.2 Helper Classes and Validators

#### 2.2.1 MaxTextWordsValidator Tests

**File:** `tests/CareerPilotAi.UnitTests/Helpers/MaxTextWordsValidatorTests.cs`

**Test Cases:**

1. **Valid Text**
   - Exactly at limit (5000 words) → Should return true
   - Below limit (2500 words) → Should return true

2. **Invalid Text**
   - Exceeds limit (5001 words) → Should return false
   - Null value → Should return false
   - Empty string → Should return false

### 2.3 Core Domain Models

#### 2.3.1 ApplicationStatus Record Tests

**File:** `tests/CareerPilotAi.UnitTests/Core/ApplicationStatusTests.cs`

**Test Cases:**

1. **Valid Status Creation**
   - Create all valid statuses: Draft, Rejected, Submitted, Interview Scheduled, etc. → Should succeed
   - Static properties access: `ApplicationStatus.Draft.Status == "Draft"` → Should be true

2. **Invalid Status Creation**
   - Invalid status string: `new ApplicationStatus("Invalid")` → Should throw ArgumentException
   - Case sensitivity: `new ApplicationStatus("draft")` → Should throw (if case-sensitive)
   - Null/Empty string → Should throw ArgumentException

3. **Record Equality**
   - Two statuses with same value → Should be equal
   - Different statuses → Should not be equal

#### 2.3.3 InterviewQuestions Domain Model Tests

**File:** `tests/CareerPilotAi.UnitTests/Core/InterviewQuestionsTests.cs`

**Test Cases:**

1. **Valid Creation**
   - With all required fields → Should succeed
   - Properties set correctly → Should match input values

2. **Invalid Creation**
   - Null/empty jobApplicationId, jobRole, companyName, interviewPreparationContent → Should throw appropriate exceptions

3. **AddQuestion Method**
   - Add valid question → Questions list should contain it
   - Add multiple questions → All should be added

4. **GetActiveQuestions Method**
   - With active questions → Should return all active ones
   - With mixed active/inactive → Should only return active
   - No active questions → Should return empty list

### 2.4 Service Classes

#### 2.4.1 UserService Tests

**File:** `tests/CareerPilotAi.UnitTests/Services/UserServiceTests.cs`

**Test Cases:**

1. **GetUserIdOrThrowException - Valid Scenarios**
   - User with valid ID in claims → Should return the user ID

2. **GetUserIdOrThrowException - Error Scenarios**
   - Null HttpContext → Should throw UserIdDoesNotExist
   - Null User in HttpContext → Should throw UserIdDoesNotExist
   - No NameIdentifier claim → Should throw UserIdDoesNotExist
   - Empty/Whitespace ID → Should throw UserIdDoesNotExist

#### 2.4.2 Clock Service Tests

**File:** `tests/CareerPilotAi.UnitTests/Services/ClockTests.cs`

**Test Cases:**

1. **GetDateTimeAdjustedToTimeZone**
   - UTC datetime with UTC timezone → Should return same datetime
   - UTC datetime with EST timezone → Should return correctly offset datetime
   - Handle half-hour offset timezones (India) → Should calculate correctly

#### 2.4.3 TimeZoneService Tests

**File:** `tests/CareerPilotAi.UnitTests/Services/TimeZoneServiceTests.cs`

**Test Cases:**

1. **GetAllAsync Method**
   - Valid cancellation token → Should return list of (Id, Name) tuples
   - Multiple time zones in database → Should return all time zones
   - Basic application time zones → Should include America/New_York, Asia/Tokyo, Europe/London, Europe/Warsaw, UTC
   - Empty database → Should return empty list
   - Ordering → Should be ordered by Name ascending
   - Data integrity → Should return correct TimeZoneId and Name pairs where Id equals Name
   - Time zone name format → Should use IANA format (e.g., "America/New_York", "Europe/Warsaw")

2. **ExistsAsync Method**
   - Valid existing time zone ID → Should return true for America/New_York
   - Valid existing time zone ID → Should return true for Europe/Warsaw
   - Valid existing time zone ID → Should return true for UTC
   - Valid non-existing time zone ID → Should return false for "Invalid/Timezone"
   - Valid non-existing time zone ID → Should return false for "America/Invalid"
   - Null time zone ID → Should return false
   - Empty string time zone ID → Should return false
   - Cancellation token handling → Should respect cancellation

### 2.5 ViewModel Validation

#### 2.5.1 RegisterViewModel Tests

**File:** `tests/CareerPilotAi.UnitTests/ViewModels/Authentication/RegisterViewModelTests.cs`

**Test Cases:**

1. **Valid Registration**
   - Valid email and matching passwords (8+ chars) → Should be valid
   - Email at maximum length (254 chars) → Should be valid

2. **Invalid Email**
   - Invalid email format → Should be invalid
   - Exceeding max length → Should be invalid

3. **Invalid Password**
   - Less than 8 characters → Should be invalid
   - Non-matching confirm password → Should be invalid

#### 2.5.2 CreateJobApplicationViewModel Tests

**File:** `tests/CareerPilotAi.UnitTests/ViewModels/JobApplication/CreateJobApplicationViewModelTests.cs`

**Test Cases:**

1. **Company Name Validation**
   - Valid (2-100 chars) → Should be valid
   - Too short/long → Should be invalid

2. **Position Validation**
   - Valid (2-100 chars) → Should be valid

3. **Job Description Validation**
   - Valid (50-5000 words) → Should be valid
   - Less than 50 or exceeds 5000 → Should be invalid

4. **Skills Validation**
   - 0-20 skills → Should be valid
   - Exceeding 20 → Should be invalid

5. **Experience Level**
   - Valid levels → Should be valid
   - Invalid level → Should be invalid

6. **Work Mode & Contract Type**
   - Valid values → Should be valid
   - Invalid values → Should be invalid

7. **Salary Validation**
   - Valid range (Min < Max) → Should be valid
   - Invalid range → Should be invalid
   - With salary type requirement → Should validate correctly

---

## 3. Integration Tests Section

### 3.2 Command Handler Integration Tests

#### 3.2.1 CreateJobApplicationCommandHandler Tests

**File:** `tests/CareerPilotAi.IntegrationTests/Commands/CreateJobApplicationCommandHandlerTests.cs`

**Test Cases:**

1. **Successful Creation**
   - Valid ViewModel → Should return valid GUID
   - Saved to database → Database should contain entry
   - User association → Should be linked to correct user
   - Default status → Status should be "Draft"

2. **Creation with Skills**
   - 1-20 skills → All should be saved
   - Empty skills list → Should save application
   - Skill data → Name and Level should persist

3. **Transaction Handling**
   - Partial failure → Transaction should rollback
   - Verification → Application should not be in database

#### 3.2.2 UpdateJobApplicationStatusCommandHandler Tests

**File:** `tests/CareerPilotAi.IntegrationTests/Commands/UpdateJobApplicationStatusCommandHandlerTests.cs`

**Test Cases:**

1. **Successful Update**
   - Valid status change → Should succeed
   - Database updated → Should reflect change
   - UpdatedAt timestamp → Should be updated

2. **Authorization**
   - Update own application → Should succeed
   - Update other user's application → Should fail with NotFound

3. **Error Scenarios**
   - Non-existent JobApplicationId → Should return NotFound
   - Invalid status → Should fail validation

#### 3.2.3 DeleteJobApplicationCommandHandler Tests

**File:** `tests/CareerPilotAi.IntegrationTests/Commands/DeleteJobApplicationCommandHandlerTests.cs`

**Test Cases:**

1. **Successful Deletion**
   - Delete existing application → Should succeed
   - Verification → Subsequent query should return null

2. **Cascade Deletion**
   - Delete with interview questions → Questions should be deleted
   - Delete with skills → Skills should be deleted

3. **Authorization**
   - Own application → Should delete
   - Other user's application → Should fail

#### 3.2.4 GenerateInterviewQuestionsCommandHandler Tests

**File:** `tests/CareerPilotAi.IntegrationTests/Commands/GenerateInterviewQuestionsCommandHandlerTests.cs`

**Test Cases:**

1. **Successful Generation**
   - Valid JobApplicationId → Should generate questions
   - Questions saved → Should be retrievable
   - Status set → Should be "success" or error status

2. **OpenRouter Integration**
   - Successful response → Should parse correctly
   - Error response → Should handle error status
   - Null response → Should return error response

3. **Authorization**
   - Own application → Should generate
   - Other user's application → Should return NotFound

### 3.3 Authentication Integration Tests

#### 3.3.1 Register Flow Tests

**File:** `tests/CareerPilotAi.IntegrationTests/Authentication/RegisterFlowTests.cs`

**Test Cases:**

1. **Successful Registration**
   - Valid data → Should create user
   - User in Identity store → Should be retrievable
   - User settings created → Should have record
   - Email not confirmed → IsEmailConfirmed should be false

2. **Email Verification**
   - Email sent → Should be sent
   - Token validation → Should work with valid token
   - Invalid token → Should fail

3. **Duplicate Registration**
   - Existing unconfirmed email → Should redirect to resend confirmation
   - Existing confirmed email → Should show error

4. **Validation**
   - Invalid email → Should show error
   - Password too short → Should show error
   - Mismatched passwords → Should show error

#### 3.3.2 Login Flow Tests

**File:** `tests/CareerPilotAi.IntegrationTests/Authentication/LoginFlowTests.cs`

**Test Cases:**

1. **Successful Login**
   - Correct credentials → Should succeed
   - User authenticated → Context set
   - Redirect → Should go to job applications

2. **Unconfirmed Email**
   - Unconfirmed email → Should fail with IsNotAllowed
   - Redirect → Should go to RegisterConfirmation

3. **Invalid Credentials**
   - Wrong password → Should fail
   - Non-existent user → Should fail
   - Error message → Generic message (no user enumeration)

#### 3.3.3 Password Reset Tests

**File:** `tests/CareerPilotAi.IntegrationTests/Authentication/PasswordResetFlowTests.cs`

**Test Cases:**

1. **Forgot Password**
   - Request → Email should be sent
   - Link generated → Link should be valid

2. **Reset Password**
   - Valid token → Should succeed
   - New password works → Login with new password works
   - Old password fails → Should not work
   - Invalid token → Should show error

---

## 4. End-to-End Tests Section

### 4.1 Authentication E2E Tests

**File:** `tests/CareerPilotAi.E2ETests/Authentication/AuthenticationE2ETests.cs`

#### 4.1.1 User Registration E2E Test

**Scenario:** Complete user registration workflow

**Steps:**
1. Navigate to login page → "Register" link visible
2. Click "Register" → Navigate to `/auth/register`
3. Enter email: `testuser@example.com` → Accepted
4. Enter password (8+ chars) → Accepted
5. Enter matching confirm password → Accepted
6. Submit form → Success message shown
7. Verify redirect → Registration confirmation page shown
8. Email verification → Confirmation link available

**Expected Outcome:**
- Registration successful
- User account created
- Confirmation email sent

#### 4.1.2 Email Confirmation E2E Test

**Scenario:** Confirm email after registration

**Steps:**
1. Register new user → Account created
2. Extract confirmation link → From logs
3. Navigate to link → Token processed
4. Verify success → Confirmation message shown
5. Automatic login → User authenticated
6. Redirect → Job applications page shown
7. Email confirmed → Status updated

**Expected Outcome:**
- Email confirmed
- User authenticated
- Job applications page accessible

#### 4.1.3 Login E2E Test

**Scenario:** User login

**Steps:**
1. Navigate to login page → `/auth/login` loads
2. Enter email → Accepted
3. Enter password → Accepted
4. Submit → Form submitted
5. Success → Redirect to job applications
6. Authenticated → User menu visible

**Expected Outcome:**
- User logged in
- Session established
- Protected pages accessible

#### 4.1.4 Logout E2E Test

**Scenario:** User logout

**Steps:**
1. Login → Authenticated
2. Click logout → Logout executed
3. Confirmation → Logout confirmation shown
4. Access protected page → Redirect to login
5. Session cleared → No authentication cookie

**Expected Outcome:**
- User logged out
- Session cleared
- Protected pages inaccessible

#### 4.1.5 Password Reset E2E Test

**Scenario:** Complete password reset workflow

**Steps:**
1. Login page → "Forgot password" link visible
2. Click link → Go to `/auth/forgot-password`
3. Enter email → Accepted
4. Submit → Confirmation message shown
5. Extract reset link → From logs
6. Navigate to link → Reset form displayed
7. Enter new password → Accepted
8. Submit → Success message shown
9. Login with new password → Works
10. Old password → Doesn't work

**Expected Outcome:**
- Password reset successful
- New password works
- Old password invalid

#### 4.1.6 User Settings E2E Test

**Scenario:** Update user timezone

**Steps:**
1. Login → Authenticated
2. Navigate to settings → `/auth/user-settings`
3. View current timezone → Displayed
4. Select different timezone → Eastern Time
5. Submit → Saved
6. Success message → Shown
7. Logout and login → Settings persist
8. Verify timezone → New timezone applied

**Expected Outcome:**
- Timezone updated
- Settings persisted
- Timezone applied to dates

### 4.2 Job Application Management E2E Tests

**File:** `tests/CareerPilotAi.E2ETests/JobApplications/JobApplicationManagementE2ETests.cs`

#### 4.2.1 Create Job Application E2E Test

**Scenario:** Create new job application

**Steps:**
1. Login → Authenticated
2. Navigate to job applications → List displayed
3. Click "Create" → Form loaded
4. Fill required fields:
   - Company: `Acme Corp`
   - Position: `Software Engineer`
   - Description: 50+ words
   - Experience: `Senior`
   - Location: `New York`
   - Work Mode: `Remote`
   - Contract: `FTE`
5. Optional fields:
   - Salary: 100000-150000
   - Salary Type: `Gross`
   - Skills: Python, React
   - URL: Valid job posting link
6. Submit → Success message
7. Redirect → Details page
8. Verify all data → Displayed correctly

**Expected Outcome:**
- Application created
- Data saved
- Details displayed

#### 4.2.2 View Job Application Details E2E Test

**Scenario:** View detailed job application

**Steps:**
1. Login → Authenticated
2. Navigate to applications → List shown
3. Click application → Details page
4. Verify fields → All visible
5. Verify timestamps → Created/Updated At shown
6. Verify timezone → Dates adjusted to user timezone
7. Action buttons → Edit, Delete, Generate Questions available

**Expected Outcome:**
- Details displayed correctly
- All information visible
- Correct timezone

#### 4.2.3 Update Job Application E2E Test

**Scenario:** Edit existing job application

**Steps:**
1. View application → Details shown
2. Click "Edit" → Form loaded
3. Change company → New value
4. Change status → `Submitted`
5. Add skills → New skills added
6. Update description → Modified
7. Submit → Success message
8. Redirect → Details page
9. Verify changes → New data visible
10. Verify timestamp → UpdatedAt recent

**Expected Outcome:**
- Changes saved
- Details updated
- Timestamp updated

#### 4.2.4 Change Application Status E2E Test

**Scenario:** Update application status

**Steps:**
1. View application → Details shown
2. Click status → Options displayed
3. Select new status → `Interview Scheduled`
4. Confirm → Submitted
5. Verify success → Message shown
6. Verify change → Status updated
7. Cycle statuses → All transitions work

**Expected Outcome:**
- Status updated
- Changes reflected
- All transitions work

#### 4.2.5 Delete Job Application E2E Test

**Scenario:** Delete job application

**Steps:**
1. View application → Details shown
2. Click "Delete" → Confirmation shown
3. Confirm → Delete executed
4. Verify deletion → Removed from list
5. Redirect → List page
6. Search → Not found
7. Verify cascade → Related data deleted

**Expected Outcome:**
- Application deleted
- Not in list
- Related data removed

### 4.3 Search and Filtering E2E Tests

**File:** `tests/CareerPilotAi.E2ETests/JobApplications/SearchAndFilteringE2ETests.cs`

#### 4.3.1 Search E2E Test

**Scenario:** Search applications

**Prerequisites:** Multiple applications created

**Steps:**
1. Navigate to list → All applications shown
2. Enter search term: `Acme` → Results filter
3. Verify results → Matching companies shown
4. Clear search → All applications shown
5. Search by title: `Engineer` → Matching titles shown
6. Partial match: `soft` → "Software Engineer" found
7. Case insensitive: `ACME` vs `acme` → Both work
8. No results: `NonExistent` → Empty state shown

**Expected Outcome:**
- Search works
- Partial matching supported
- Case insensitive
- Empty state handled

#### 4.3.2 Filter by Status E2E Test

**Scenario:** Filter by status

**Steps:**
1. Navigate to list → All statuses shown
2. Select single status: `Submitted` → Filtered
3. Verify display → Only submitted shown
4. Select multiple: `Draft`, `Submitted` → Both shown
5. Clear filters → All shown

**Expected Outcome:**
- Single filter works
- Multiple filters work
- Clear functionality works

#### 4.3.3 Filter by Salary E2E Test

**Scenario:** Filter by salary range

**Prerequisites:** Applications with salary ranges

**Steps:**
1. Navigate to list → Filter controls visible
2. Enter min: `80000` → Filters >= value
3. Enter max: `120000` → Filters <= value
4. Apply → Results in range shown
5. Adjust values → Filters updated
6. Select period and type → Filters applied
7. Remove filters → All shown

**Expected Outcome:**
- Range filtering works
- Multiple filters work
- Results update correctly

#### 4.3.4 Filter by Work Mode and Experience E2E Test

**Scenario:** Filter by work mode and experience

**Steps:**
1. Select work mode: `Remote` → Filtered
2. Select multiple modes → Both shown
3. Select experience: `Senior` → Filtered
4. Combine filters → All apply
5. Clear individual filters → Others remain

**Expected Outcome:**
- Multiple filters work together
- Combinations apply correctly
- Can clear independently

#### 4.3.5 Sorting E2E Test

**Scenario:** Sort applications

**Steps:**
1. Default sort → Most recent first (DateAddedDesc)
2. Click sort dropdown → Options shown
3. Sort ascending → Oldest first
4. Sort descending → Newest first (back to default)
5. Verify sorting → Correct order
6. Refresh (if persistent) → Sort maintained

**Expected Outcome:**
- Sorting works
- Both ascending/descending available
- Default is most recent

### 4.4 Interview Questions Workflow E2E Tests

**File:** `tests/CareerPilotAi.E2ETests/InterviewQuestions/InterviewQuestionsE2ETests.cs`

#### 4.4.1 Generate Interview Questions E2E Test

**Scenario:** Generate AI-powered questions

**Prerequisites:** Job application created

**Steps:**
1. View application → Details shown
2. Click "Generate Questions" → Generation starts
3. Show loading state → Spinner/status shown
4. Wait for completion → AI generates
5. Display questions → Q&A pairs shown
6. Verify content → Questions, Answers, Guides populated
7. Verify quantity → Reasonable count
8. Error handling → Error message if failed

**Expected Outcome:**
- Questions generated
- Questions displayed with content
- Error handling shown

#### 4.4.2 Prepare Interview Preparation Content E2E Test

**Scenario:** Generate preparation content

**Steps:**
1. View application → Details shown
2. Click "Prepare Content" → Generation starts
3. Wait for processing → Show loading
4. Display content → Content shown
5. Verify quality → Relevant content shown
6. Edit (if applicable) → Editable
7. Save → Persisted

**Expected Outcome:**
- Content generated
- Content relevant
- Saved correctly

#### 4.4.3 Save Custom Preparation Content E2E Test

**Scenario:** Save custom notes

**Steps:**
1. View application → Details shown
2. Find preparation section → Visible
3. Edit content → Type/paste custom
4. Save → Saved
5. Refresh → Persists
6. Edit again → Updatable
7. Clear (if allowed) → Allowed

**Expected Outcome:**
- Content saved
- Persists across sessions
- Updatable

#### 4.4.4 Delete Interview Question E2E Test

**Scenario:** Remove interview question

**Steps:**
1. View questions → List shown
2. Click delete → Question removed
3. Verify removed → Not in list
4. Verify count → Decreased

**Expected Outcome:**
- Question deleted
- Not displayed
- Count updated

### 4.5 Navigation and Access Control E2E Tests

**File:** `tests/CareerPilotAi.E2ETests/Navigation/NavigationE2ETests.cs`

#### 4.5.1 Public Page Access E2E Test

**Scenario:** Access public pages

**Steps:**
1. Navigate to home: `/` → Accessible
2. Navigate to login: `/auth/login` → Accessible
3. Navigate to register: `/auth/register` → Accessible
4. Verify no user menu → Not shown

**Expected Outcome:**
- Public pages accessible
- No authentication required

#### 4.5.2 Protected Page Access E2E Test

**Scenario:** Try accessing protected pages

**Steps:**
1. Navigate to applications: `/job-applications` → Redirect to login
2. Navigate to create: `/job-applications/create` → Redirect
3. Navigate to details: `/job-applications/details/{id}` → Redirect
4. Verify redirect → Login page shown
5. Login → Provide credentials
6. Navigate again → Accessible

**Expected Outcome:**
- Protected pages redirect to login
- After login, pages accessible

#### 4.5.3 Navigation Menu E2E Test

**Scenario:** Navigate using menu

**Steps:**
1. Login → Authenticated
2. Verify menu → Navigation shown
3. Click "Applications" → List page
4. Click "Create" → Create form
5. Click logo → Dashboard
6. Click user menu → Settings/Profile
7. Click logout → Logout executed

**Expected Outcome:**
- Menu navigation works
- Links go to correct pages
- User menu shows options

---

## 5. Additional Testing Recommendations

### 5.1 Performance Testing

Consider future addition of:

1. **Load Testing**
   - Concurrent registrations: 100+ simultaneous
   - Concurrent searches: 50+ with filters
   - Interview question generation: Load on OpenRouter

2. **Database Performance**
   - Query N+1 problems: Ensure efficient loading
   - Index performance: Sorting/filtering on large datasets
   - Pagination: With 10,000+ applications

### 5.2 Security Testing

1. **CSRF Protection**
   - Verify tokens in forms
   - Test token validation

2. **Authentication Security**
   - Password hashing: Strong algorithm
   - Token expiration: Session timeout
   - Concurrent logins: Same user, multiple devices

3. **Authorization**
   - User data isolation: Users can't access others' data
   - Role-based access (if roles exist)

4. **Input Validation**
   - SQL injection in search/filters
   - XSS attempts in text fields
   - File upload security (if applicable)

### 5.4 Browser Compatibility

Playwright tests should run against:
- Chromium (primary)
- Firefox (if needed)
- WebKit (Safari equivalent)

### 5.5 CI/CD Integration

```yaml
- name: Run Unit Tests
  run: dotnet test tests/CareerPilotAi.UnitTests
  
- name: Run Integration Tests
  run: dotnet test tests/CareerPilotAi.IntegrationTests
  
- name: Run E2E Tests
  run: dotnet test tests/CareerPilotAi.E2ETests
```
---

## Conclusion

This comprehensive test plan provides detailed guidance for implementing robust testing for CareerPilotAi. The layered approach (unit → integration → E2E) ensures rapid feedback and comprehensive validation of user workflows.