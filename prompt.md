<codebase>
Search whole codebase!
</codebase>

You are an experienced software testing architect tasked with creating a comprehensive test plan for a .NET web application. You will analyze the provided codebase and create a detailed testing strategy that strictly adheres to the specified technology stack.

## Technology Stack Requirements

### Frontend
- Bootstrap 5, JavaScript, CSS
- ASP.NET Core MVC with Razor Views
- Chart.js for charts

### Backend
- .NET 8
- Entity Framework Core
- ASP.NET Core MVC
- ASP.NET Identity
- PostgreSQL
- OpenRouter for AI integration
- SendGrid for email integration

### CI/CD and Hosting
- GitHub Actions
- Docker (Local development only)
- Azure (if hosting is needed)

## Required Testing Technologies

### Unit Tests
- Testing framework: xUnit
- Mocking library: NSubstitute
- Fake data: Bogus
- Assertions: Shouldly or Default Assertions

### Integration Tests
- Testing framework: xUnit
- Mocking library: NSubstitute
- Fake data: Bogus
- Assertions: Shouldly or Default Assertions
- Real database: PostgreSQL in test container
- Use Respawn library to reset the database between tests

### End-to-End Tests
- Testing framework: Playwright (C#)

## Instructions

Your task is to create a comprehensive test plan that:

1. **Analyzes the codebase structure** to identify all testable components
2. **Strictly uses only the specified testing technologies** - do not deviate from the tech stack requirements above
3. **Provides detailed recommendations** for what specific tests should be implemented
4. **Categorizes tests appropriately** into Unit, Integration, and End-to-End categories
5. **Explains the rationale** behind each testing recommendation

Before creating your final test plan, in <analysis> tags inside your thinking block:
- Extract and list all key components from the codebase (controllers, services, models, repositories, etc.) - it's OK for this section to be quite long
- For each component, note which type of testing (unit, integration, E2E) would be most appropriate and why
- Map out the critical user workflows and business logic flows you can identify from the code
- List any external integrations or dependencies (database operations, third-party services, etc.) that require special testing considerations
- Note any complex business rules, validation logic, or error handling patterns that need thorough testing coverage

Structure your final test plan with the following sections:

**Unit Tests Section:**
- List specific classes/methods that should have unit tests
- Explain what aspects should be tested for each component
- Specify which testing technologies from the stack will be used

**Integration Tests Section:**
- Identify integration points that need testing
- Detail database operations that should be tested
- Specify external service integrations to test
- Explain how the PostgreSQL test container and Respawn will be utilized

**End-to-End Tests Section:**
- Map out critical user workflows to test
- Identify key UI interactions and business processes
- Specify how Playwright will be used for these tests

Each section should include specific, actionable recommendations with sufficient detail for a developer to implement the tests.

Your final output should consist only of the structured test plan and should not duplicate or rehash any of the analysis work you did in the thinking block.