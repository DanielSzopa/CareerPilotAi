---
goal: Fix UTC DateTime Display Bug with Timezone Support for Job Application Cards
version: 1.0
date_created: 2025-01-19
last_updated: 2025-01-19
owner: Development Team
tags: [bug, timezone, datetime, localization, enhancement]
---

# Introduction

This plan addresses the UTC datetime display bug on job application cards in the Index.cshtml view. Currently, the application stores and displays datetime values in UTC format without converting them to the user's local timezone. The solution will implement timezone detection via client-side headers and server-side timezone conversion with Poland as the default fallback.

## 1. Requirements & Constraints

**Business Requirements:**
- **REQ-001**: Display datetime in user's local timezone on job application cards
- **REQ-002**: Support client-side timezone detection via 'x-timezone' header
- **REQ-003**: Use Poland timezone (Europe/Warsaw) as default fallback when header is missing
- **REQ-004**: Maintain UTC storage in database for consistency
- **REQ-005**: Apply timezone conversion across all datetime displays in the application

**Technical Requirements:**
- **REQ-006**: Follow existing C# coding standards and architectural patterns
- **REQ-007**: Implement helper classes for timezone operations
- **REQ-008**: Maintain backward compatibility with existing card display format
- **REQ-009**: Use dependency injection for timezone services
- **REQ-010**: Include proper error handling and logging

**Security Requirements:**
- **SEC-001**: Validate timezone identifiers to prevent injection attacks
- **SEC-002**: Sanitize client-provided timezone headers

**Performance Requirements:**
- **PER-001**: Minimize overhead for timezone conversions
- **PER-002**: Cache timezone information per request when possible

## 2. Implementation Steps

### Phase 1: Infrastructure Setup

**TASK-001**: Create TimeZone Helper Infrastructure
- Create `Application/Helpers/TimeZoneHelper.cs` class
- Implement timezone validation and conversion methods
- Include support for timezone ID validation against system timezone list
- Add logging for timezone operations
- Include fallback to Poland timezone (Europe/Warsaw)

**TASK-002**: Create TimeZone Header Service
- Create `Application/Services/ITimeZoneService.cs` interface
- Create `Application/Services/TimeZoneService.cs` implementation
- Implement header parsing logic for 'x-timezone' header
- Add validation for timezone identifiers
- Register service in DI container in `Program.cs`

**TASK-003**: Update Service Registration
- Register `ITimeZoneService` as Scoped in `Application/Services/ServicesExtensions.cs`
- Update `Program.cs` to include timezone services in DI container

### Phase 2: Core Implementation

**TASK-004**: Create DateTime Extension Methods
- Create `Application/Extensions/DateTimeExtensions.cs`
- Implement `ToUserTimeZone(string timeZoneId)` extension method
- Implement `ToUserTimeZone(TimeZoneInfo timeZone)` extension method
- Add proper null handling and error handling

**TASK-005**: Update CardDate Model
- Modify `Models/JobApplication/JobApplicationCardViewModel.cs`
- Update `CardDate` class constructor to accept timezone parameter
- Add overloaded constructor: `CardDate(DateTime dateTime, string timeZoneId)`
- Maintain backward compatibility with existing constructor
- Update format to clearly indicate local time

**TASK-006**: Update Controller Logic
- Modify `Controllers/JobApplicationController.cs` Index action
- Inject `ITimeZoneService` into controller constructor
- Retrieve timezone from service in Index method
- Update JobApplicationCardViewModel creation to pass timezone information
- Handle cases where timezone service fails gracefully

### Phase 3: Client-Side Implementation

**TASK-007**: Add Client-Side Timezone Detection
- Update `Views/JobApplication/Index.cshtml` with JavaScript timezone detection
- Add script to detect browser timezone using `Intl.DateTimeFormat().resolvedOptions().timeZone`
- Send 'x-timezone' header with all subsequent AJAX requests
- Implement fallback detection methods for older browsers

**TASK-008**: Update AJAX Requests
- Modify all existing AJAX calls in `Views/JobApplication/Index.cshtml`
- Add 'x-timezone' header to fetch requests in `deleteJobApplication` function
- Ensure consistent header inclusion across all API calls

### Phase 4: Testing & Validation

**TASK-009**: Create Unit Tests
- Create `tests/CareerPilotAi.Tests/Helpers/TimeZoneHelperTests.cs`
- Test timezone conversion methods with various inputs
- Test edge cases (invalid timezones, null values, DST transitions)
- Create `tests/CareerPilotAi.Tests/Services/TimeZoneServiceTests.cs`
- Test header parsing and validation logic

**TASK-010**: Update Integration Tests
- Update existing controller tests to account for timezone functionality
- Test Index action with various timezone headers
- Verify proper fallback behavior when header is missing

## 3. Alternatives

- **ALT-001**: Store timezone preference in user profile - Rejected due to complexity and requirement for immediate solution
- **ALT-002**: Use JavaScript-only solution for datetime conversion - Rejected due to SEO and accessibility concerns
- **ALT-003**: Always use browser's local timezone without header - Rejected due to potential caching issues and server-side processing needs

## 4. Dependencies

- **DEP-001**: System.TimeZoneInfo .NET API for timezone operations
- **DEP-002**: Browser Intl.DateTimeFormat API for client-side timezone detection
- **DEP-003**: ASP.NET Core HTTP headers API for header processing
- **DEP-004**: Existing CardDate model and JobApplicationController structure

## 5. Files

- **FILE-001**: `Application/Helpers/TimeZoneHelper.cs` - New helper class for timezone operations
- **FILE-002**: `Application/Services/ITimeZoneService.cs` - New interface for timezone service
- **FILE-003**: `Application/Services/TimeZoneService.cs` - New service implementation
- **FILE-004**: `Application/Extensions/DateTimeExtensions.cs` - New extension methods
- **FILE-005**: `Models/JobApplication/JobApplicationCardViewModel.cs` - Update CardDate class
- **FILE-006**: `Controllers/JobApplicationController.cs` - Update Index action
- **FILE-007**: `Views/JobApplication/Index.cshtml` - Add client-side timezone detection
- **FILE-008**: `Application/Services/ServicesExtensions.cs` - Update service registration
- **FILE-009**: `tests/CareerPilotAi.Tests/Helpers/TimeZoneHelperTests.cs` - New test file
- **FILE-010**: `tests/CareerPilotAi.Tests/Services/TimeZoneServiceTests.cs` - New test file

## 6. Testing

- **TEST-001**: Unit tests for TimeZoneHelper timezone conversion methods
- **TEST-002**: Unit tests for TimeZoneService header parsing and validation
- **TEST-003**: Unit tests for DateTime extension methods with various timezone inputs
- **TEST-004**: Integration tests for JobApplicationController Index action with timezone headers
- **TEST-005**: Manual testing with different browser timezone settings
- **TEST-006**: Edge case testing for DST transitions and invalid timezone identifiers
- **TEST-007**: Performance testing for timezone conversion overhead

## 7. Risks & Assumptions

**Risks:**
- **RISK-001**: Browser compatibility issues with Intl.DateTimeFormat API in older browsers
- **RISK-002**: Performance impact of timezone conversions on large lists of job applications
- **RISK-003**: Potential breaking changes to existing datetime display formats
- **RISK-004**: Complexity in handling DST (Daylight Saving Time) transitions
- **RISK-005**: Client-server timezone mismatch during page load vs AJAX requests

**Assumptions:**
- **ASSUMPTION-001**: Most users will have modern browsers supporting timezone APIs
- **ASSUMPTION-002**: Poland timezone (Europe/Warsaw) is appropriate default for the target audience
- **ASSUMPTION-003**: UTC storage in database will remain unchanged
- **ASSUMPTION-004**: Performance impact will be minimal for typical user loads
- **ASSUMPTION-005**: Users prefer seeing local time over UTC time

## 8. Related Specifications / Further Reading

- [.NET TimeZoneInfo Documentation](https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo)
- [MDN Intl.DateTimeFormat Documentation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Intl/DateTimeFormat)
- [ASP.NET Core Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Timezone Best Practices in Web Applications](https://stackoverflow.com/questions/15141762/how-to-handle-time-zones-in-web-applications)
