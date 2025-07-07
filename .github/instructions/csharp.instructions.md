---
applyTo: '**/*.cs'
---

# C# Coding Standards and Preferences for CareerPilotAi

As a Senior .NET Developer and Solution architect, your role is to ensure that all C# code in the CareerPilotAi project adheres to the following coding standards and conventions. These standards are designed to maintain consistency, readability, and maintainability across the codebase.

## Project Architecture & Patterns

### Dependency Injection & Service Registration
- Use extension methods for service registration (e.g., `AddInfrastructure()`, `RegisterCommands()`)
- Chain service registrations fluently when possible
- Register services with appropriate lifetimes (Scoped for DbContext, Singleton for configuration services)

## Naming Conventions

### General Naming
- Use PascalCase for public members, classes, methods, properties
- Use camelCase for private fields with underscore prefix (e.g., `_userService`, `_dbContext`)
- Use meaningful, descriptive names that clearly indicate purpose
- Avoid abbreviations except for well-known ones (e.g., `URL`, `ID`)

### File and Class Organization
- One class per file with matching filename
- Group related files in appropriate folders (`Commands`, `Exceptions`, `DataModels`)
- Use namespace that matches folder structure
- Place interfaces in same namespace as implementations unless separation is needed
- Prefer file-Scoped namespaces

### Command Pattern Naming
- Commands: `{Action}{Entity}Command` (e.g., `CreateJobApplicationCommand`)
- Handlers: `{Action}{Entity}CommandHandler` (e.g., `CreateJobApplicationCommandHandler`)
- Use `record` for simple command classes with immutable data

## Code Style & Formatting

### Access Modifiers
- Use `internal` for domain entities and internal implementation details
- Use `public` for API controllers, models, and external interfaces
- Use `private` for implementation details within classes

### Property and Field Declarations
- Use auto-properties with private setters for domain entities: `internal string UserId { get; private set; }`
- Initialize collections inline: `= new List<InterviewQuestionDataModel>()`
- Use nullable reference types appropriately (`string?` for optional fields)

### Method Signatures
- Include `CancellationToken cancellationToken` parameter for async operations
- Use descriptive parameter names

### Exception Handling
- Create custom exceptions for domain-specific errors
- Use descriptive exception names: `{Entity}{Validation}Exception` pattern e.g. `JobApplicationUserIdCannotBeEmptyException`, `JobApplicationUrlCannotBeEmptyException`
- Inherit from `Exception` with meaningful error messages
- Validate inputs early and throw exceptions for invalid states

### Styling
- Use one line for simple if statements: `if (condition) DoSomething();`

## Domain Design Patterns

### Entity Validation
- Validate all inputs in constructors
- Throw specific exceptions for each validation rule
- Use guard clauses at the beginning of methods
- Example pattern:
  ```csharp
  if (string.IsNullOrWhiteSpace(userId))
      throw new JobApplicationUserIdCannotBeEmptyException();
  ```

### Value Objects and Constants
- Use private const fields for magic numbers/limits (e.g., `private const int _maxWords = 5000`)
- Validate business rules in domain entities
- Use helper classes for complex validations (`MaxTextWordsValidator`, `UrlValidator`)

### Data Models vs Domain Models
- Separate data models (`*DataModel`) in Infrastructure/Persistence from domain models
- Use data models for Entity Framework mapping
- Use domain models for business logic and validation

## ASP.NET Core Patterns

### Controller Design
- Use attribute routing: `[Route("job-applications")]`
- Apply `[Authorize]` at controller level when all actions require authentication
- Inject dependencies through constructor
- Use meaningful action names and HTTP verb attributes

### Model Validation
- Use data annotations for basic validation (`[Required]`, `[MaxLength]`)
- Create custom validation attributes for complex business rules (`[MaxWords]`)
- Provide meaningful error messages for all validations

## Database and Persistence

### Entity Framework Configuration
- Use `AsNoTracking()` for read-only queries
- Use proper async patterns: `await _dbContext.SaveChangesAsync(cancellationToken)`
- Configure relationships and constraints in DbContext

### Data Access Patterns
- Access data through DbContext in handlers
- Use proper cancellation token propagation

## Logging and Error Handling

### Logging Patterns
- Use structured logging with meaningful parameters
- Log important business operations (creation, updates, deletions)
- Include relevant identifiers in log messages e.g. userId, jobApplicationId
- Avoid string interpolation in log messages for performance, use templates, Example: `_logger.LogInformation("Creating new job application with JobApplicationId: {jobApplicationId} for UserId: {userId}", ...)`

### Error Messages
- Provide user-friendly error messages in validation attributes
- Use specific error messages for different validation scenarios
- Maintain consistency in error message formatting

## Code Organization Best Practices

### Constructor Patterns
- Assign all dependencies in constructor body
- Keep constructors simple and focused on dependency assignment

### Method Organization
- Keep methods focused on single responsibility
- Use descriptive method names that indicate the action and outcome

### Extension Methods
- Use extension methods for service registration and configuration
- Group related extensions in dedicated classes
- Use descriptive names that indicate what is being extended