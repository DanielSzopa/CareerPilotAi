---
goal: Implement PATCH endpoint for updating job application status
version: 1.0
date_created: 2025-01-08
last_updated: 2025-01-08
owner: CareerPilotAi Development Team
tags: [feature, api, status-management, security, validation]
---

# Update Application Status Endpoint Implementation Plan

This plan outlines the implementation of a new PATCH endpoint for updating job application status. The implementation will follow existing patterns in the codebase and ensure proper security, validation, and logging.

## 1. Requirements & Constraints

- **REQ-001**: Create PATCH endpoint `/job-applications/api/status/{jobApplicationId:guid}` for updating application status
- **REQ-002**: Only authenticated users can update job application status
- **REQ-003**: Users can only update job applications that belong to them
- **REQ-004**: Status must be a valid ApplicationStatus as defined in `ApplicationStatus.cs`
- **REQ-005**: Endpoint must return appropriate HTTP status codes and error messages
- **REQ-006**: All operations must be logged with appropriate context (userId, jobApplicationId, status)
- **REQ-007**: Follow existing command pattern architecture used in the application
- **REQ-008**: Implement proper error handling with ProblemDetails responses
- **REQ-009**: Use cancellation tokens for async operations
- **REQ-010**: Validate all inputs and return meaningful error messages

- **SEC-001**: Enforce authorization - only authenticated users can access the endpoint
- **SEC-002**: Implement user ownership validation - users can only update their own job applications
- **SEC-003**: Validate status input to prevent unauthorized status transitions

- **CON-001**: Must follow existing C# coding standards defined in project instructions
- **CON-002**: Must use existing ApplicationDbContext for database operations
- **CON-003**: Must use ICommandDispatcher pattern for command handling
- **CON-004**: Must return consistent response format matching other endpoints

- **GUD-001**: Follow existing logging patterns with structured logging
- **GUD-002**: Use existing exception handling patterns
- **GUD-003**: Maintain consistency with existing API endpoints

- **PAT-001**: Follow Command pattern architecture
- **PAT-002**: Use record types for simple command classes
- **PAT-003**: Implement proper dependency injection
- **PAT-004**: Use async/await patterns with cancellation tokens

## 2. Implementation Steps

### Phase 1: Create Command and Response Models

- **TASK-1.1**: Create `UpdateJobApplicationStatusCommand` record
- **Description**: Define command record to encapsulate status update request
- **Details**:
  - File path: `CareerPilotAi/Application/Commands/UpdateJobApplicationStatus/UpdateJobApplicationStatusCommand.cs`
  - Record should contain: `Guid JobApplicationId`, `string Status`
  - Implement `ICommand` interface
  - Use record type for immutability

- **TASK-1.2**: Create `UpdateJobApplicationStatusResponse` record
- **Description**: Define response record for status update operation
- **Details**:
  - File path: `CareerPilotAi/Application/Commands/UpdateJobApplicationStatus/UpdateJobApplicationStatusResponse.cs`
  - Record should contain: `bool IsSuccess`, `ProblemDetails? ProblemDetails`
  - Follow existing response pattern used in other commands

### Phase 2: Implement Command Handler

- **TASK-2.1**: Create `UpdateJobApplicationStatusCommandHandler` class
- **Description**: Implement command handler following existing patterns
- **Details**:
  - File path: `CareerPilotAi/Application/Commands/UpdateJobApplicationStatus/UpdateJobApplicationStatusCommandHandler.cs`
  - Implement `ICommandHandler<UpdateJobApplicationStatusCommand, UpdateJobApplicationStatusResponse>`
  - Inject dependencies: `ApplicationDbContext`, `IUserService`, `ILogger`, `IHttpContextAccessor`
  - Validate user authentication using `_userService.GetUserIdOrThrowException()`
  - Validate job application ownership by checking `UserId` matches authenticated user
  - Validate status is valid using `ApplicationStatus` constructor validation
  - Update `Status` field in `JobApplicationDataModel`
  - Include comprehensive error handling with ProblemDetails responses
  - Log information messages for successful operations
  - Log error messages for failed operations with context

### Phase 3: Add Request Model

- **TASK-3.1**: Create `UpdateJobApplicationStatusRequest` class
- **Description**: Define request model for API endpoint
- **Details**:
  - File path: `CareerPilotAi/Models/JobApplication/UpdateJobApplicationStatusRequest.cs`
  - Properties: `string Status` with data annotations
  - Add `[Required]` validation for Status field
  - Add meaningful error messages for validation attributes

### Phase 4: Implement Controller Endpoint

- **TASK-4.1**: Add PATCH endpoint to `JobApplicationController`
- **Description**: Implement API endpoint following existing patterns
- **Details**:
  - Method signature: `public async Task<IActionResult> UpdateJobApplicationStatus(Guid jobApplicationId, [FromBody] UpdateJobApplicationStatusRequest request, CancellationToken cancellationToken)`
  - Route: `[HttpPatch]` `[Route("api/status/{jobApplicationId:guid}")]`
  - Validate ModelState and return BadRequest with ProblemDetails if invalid
  - Use `_commandDispatcher.DispatchAsync` to execute command
  - Return appropriate responses:
    - 200 OK with success message for successful updates
    - 400 BadRequest for invalid input data
    - 404 NotFound for non-existent job applications
    - 500 InternalServerError for unexpected errors
  - Include comprehensive exception handling with structured logging
  - Follow existing error response patterns using `Problem()` method

### Phase 5: Register Command Handler

- **TASK-5.1**: Register command handler in DI container
- **Description**: Add command handler registration to dependency injection
- **Details**:
  - File path: `CareerPilotAi/Application/Commands/CommandsExtensions.cs`
  - Add registration in `RegisterCommands()` method
  - Use scoped lifetime: `.AddScoped<ICommandHandler<UpdateJobApplicationStatusCommand, UpdateJobApplicationStatusResponse>, UpdateJobApplicationStatusCommandHandler>()`

### Phase 6: Add Domain Model Method

- **TASK-6.1**: Add status update method to `JobApplication` domain model
- **Description**: Implement domain logic for status updates
- **Details**:
  - File path: `CareerPilotAi/Core/JobApplication.cs`
  - Add method: `internal void UpdateStatus(ApplicationStatus applicationStatus)`
  - Validate applicationStatus is not null
  - Update internal `ApplicationStatus` property
  - Maintain existing validation patterns

### Phase 7: Update Frontend JavaScript

- **TASK-7.1**: Implement actual API call in `JobApplicationDetails.cshtml`
- **Description**: Replace placeholder function with actual API call
- **Details**:
  - File path: `CareerPilotAi/Views/JobApplication/JobApplicationDetails.cshtml`
  - Replace `updateApplicationStatus` placeholder function with fetch API call
  - Use PATCH method with proper headers including anti-forgery token
  - Implement proper error handling and user feedback
  - Show success/error messages using existing utility functions
  - Follow existing AJAX patterns used in the application

## 3. Alternatives

- **ALT-001**: Use PUT instead of PATCH - Not chosen because PATCH is more semantically correct for partial updates
- **ALT-002**: Include status update in existing UpdateJobDescription endpoint - Not chosen to maintain single responsibility principle
- **ALT-003**: Create separate endpoint for each status transition - Not chosen due to complexity and maintenance overhead

## 4. Dependencies

- **DEP-001**: Existing `ApplicationStatus` class for status validation
- **DEP-002**: Existing `ICommandDispatcher` and command pattern infrastructure
- **DEP-003**: Existing `IUserService` for user authentication
- **DEP-004**: Existing `ApplicationDbContext` for database operations
- **DEP-005**: Existing logging infrastructure (`ILogger<T>`)
- **DEP-006**: Existing error handling patterns with `ProblemDetails`

## 5. Files

- **FILE-001**: `CareerPilotAi/Application/Commands/UpdateJobApplicationStatus/UpdateJobApplicationStatusCommand.cs` - Command record
- **FILE-002**: `CareerPilotAi/Application/Commands/UpdateJobApplicationStatus/UpdateJobApplicationStatusResponse.cs` - Response record
- **FILE-003**: `CareerPilotAi/Application/Commands/UpdateJobApplicationStatus/UpdateJobApplicationStatusCommandHandler.cs` - Command handler implementation
- **FILE-004**: `CareerPilotAi/Models/JobApplication/UpdateJobApplicationStatusRequest.cs` - Request model
- **FILE-005**: `CareerPilotAi/Controllers/JobApplicationController.cs` - Controller endpoint (modification)
- **FILE-006**: `CareerPilotAi/Application/Commands/CommandsExtensions.cs` - DI registration (modification)
- **FILE-007**: `CareerPilotAi/Core/JobApplication.cs` - Domain model method (modification)
- **FILE-008**: `CareerPilotAi/Views/JobApplication/JobApplicationDetails.cshtml` - Frontend implementation (modification)

## 6. Testing

- **TEST-001**: Verify PATCH endpoint returns 200 OK when updating status for owned job application
- **TEST-002**: Verify PATCH endpoint returns 404 NotFound when job application doesn't exist
- **TEST-003**: Verify PATCH endpoint returns 404 NotFound when user tries to update job application they don't own
- **TEST-004**: Verify PATCH endpoint returns 400 BadRequest when invalid status is provided
- **TEST-005**: Verify PATCH endpoint returns 400 BadRequest when request model validation fails
- **TEST-006**: Verify PATCH endpoint requires authentication (401 Unauthorized for anonymous users)
- **TEST-007**: Verify status is actually updated in database after successful API call
- **TEST-008**: Verify logging is performed for both successful and failed operations
- **TEST-009**: Verify frontend successfully calls API and updates UI on status change
- **TEST-010**: Verify frontend displays appropriate error messages for failed API calls

## 7. Risks & Assumptions

- **RISK-001**: Concurrent updates to the same job application could cause conflicts - Mitigated by using entity framework change tracking
- **RISK-002**: Invalid status values could be passed despite validation - Mitigated by using ApplicationStatus constructor validation
- **RISK-003**: Frontend and backend status validation could become inconsistent - Mitigated by using same ApplicationStatus class for validation

- **ASSUMPTION-001**: Users should be able to change status to any valid ApplicationStatus without workflow restrictions
- **ASSUMPTION-002**: No additional business logic is required for status transitions
- **ASSUMPTION-003**: Existing authentication and authorization mechanisms are sufficient
- **ASSUMPTION-004**: Current ApplicationStatus enum covers all required statuses

## 8. Related Specifications / Further Reading

- [C# Coding Standards](d:\Repositories\CareerPilotAi\.github\instructions\csharp.instructions.md)
- [Razor Views Guidelines](d:\Repositories\CareerPilotAi\.github\instructions\razor.instructions.md)
- [Existing Command Pattern Implementation Examples](UpdateJobDescriptionCommand, DeleteJobApplicationCommand)
- [ApplicationStatus Class Reference](d:\Repositories\CareerPilotAi\CareerPilotAi\Core\ApplicationStatus.cs)
- [JobApplicationController Existing Patterns](d:\Repositories\CareerPilotAi\CareerPilotAi\Controllers\JobApplicationController.cs)
