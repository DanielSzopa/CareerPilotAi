## CareerPilotAi

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-2C3E50)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-336791?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)

### Table of Contents
- [Project description](#project-description)
- [Features](#features)
- [Tech stack](#tech-stack)
- [Getting started locally](#getting-started-locally)
- [Configuration](#configuration)
- [Testing](#testing)
- [Project roadmap](#project-roadmap)
- [Project status](#project-status)

### Project description
CareerPilotAi is a web application that helps job seekers organize and track their job applications with intelligent assistance. Users can paste a job offer from a job board, and AI will structure the details into a manageable application entry. The system centralizes applications, enables quick status updates, and provides a dashboard with key metrics to monitor progress.

An amazing feature is that you can generate interview questions tailored to the job offer.

Key value:
- Save time with automated parsing of job postings
- Keep all applications and their status history in one place
- Filter, search, and review details quickly
- Prepare for interviews with AI-generated questions.

### Features

*   **Job Application Management**
    *   **AI-Powered Creation**: Paste a job description and let AI automatically parse and fill in details like company, title, location, salary, and required skills.
    *   **Comprehensive Tracking**: Manage full application details, including status, job URL, contract type, and experience level.
    *   **CRUD**: Create, view and delete job applications. Edit feature is not yet implemented.
    *   **Advanced Search & Filtering**: Quickly find applications with powerful filters for status, salary range, location, work mode, and experience level, plus a text search for company and title.
    *   **Status Updates**: Easily update the status of each application to track its progress.

*   **AI-Powered Interview Preparation**
    *   **Custom Question Generation**: Generate interview questions tailored specifically to the job description and your skills.

*   **Personal Dashboard**
    *   **At-a-Glance Metrics**: A central dashboard provides key statistics on your job search.
    *   **Visual Insights**: Charts visualize application status distribution and track your activity over time.

*   **Account Management**
    *   **Secure Authentication**: Standard user registration with email confirmation, login, and password reset flows.
    *   **User Settings**: Customize your experience with settings like your local time zone for accurate date tracking.

### Tech stack
- **Frontend**: ASP.NET Core MVC (Razor Views), Bootstrap 5, JavaScript, Chart.js
- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core
- **Identity**: ASP.NET Identity
- **Database**: PostgreSQL
- **AI Integration**: OpenRouter (configurable models per feature)
- **Email**: SendGrid (account verification and password reset)
- **CI/CD & Hosting**: GitHub Actions, Docker (local), Azure (production)

### Getting started locally

There are two primary ways to run the application locally, depending on your needs.

**Prerequisites:**
- .NET SDK 8.0+
- Docker Desktop (with Docker Compose v2)
- Git
- OpenRouter API key (Required for AI features)

---

#### Option 1: Full Docker Compose (Recommended for quick start)

This approach runs both the application and the PostgreSQL database in Docker containers. It's the fastest way to get the application running.

1.  **Clone the repository**
    ```bash
    git clone https://github.com/DanielSzopa/CareerPilotAi.git
    cd CareerPilotAi
    ```

2.  **Create a `.env` file**
    In the root directory of the project, create a file named `.env` and add your OpenRouter API key. This is required for the application to start.

    ```
    OPENROUTER__AUTHTOKEN=YOUR_OPENROUTER_TOKEN
    ```

3.  **Start the services**
    ```bash
    docker compose up -d
    ```
    This command will build the application image and start the `app` and `postgres` services. The database connection is pre-configured for the container environment.

    > **Note:** The application automatically applies any pending Entity Framework database migrations on startup.

4.  **Access the application**
    The application will be available at `http://localhost:8080`.

---

#### Option 2: Hybrid - Database in Docker, App via `dotnet run` (Recommended for development)

This approach is ideal for development, as it allows you to run and debug the application directly from your IDE or command line, while the database runs in a Docker container.

1.  **Clone the repository**
    ```bash
    git clone https://github.com/DanielSzopa/CareerPilotAi.git
    cd CareerPilotAi
    ```

2.  **Start the database**
    Run only the PostgreSQL service from the `docker-compose.yml` file.
    ```bash
    docker compose up -d postgres
    ```
    The database will be available at `localhost:5432`.

3.  **Configure application secrets**
    Use the .NET user secrets manager to store your OpenRouter API key. This is required.
    ```bash
    # Navigate to the project directory
    cd CareerPilotAi

    # Initialize user secrets for the project
    dotnet user-secrets init

    # Set your OpenRouter token
    dotnet user-secrets set "OpenRouter:AuthToken" "YOUR_OPENROUTER_TOKEN"
    ```

    > **Optional: Configure SendGrid**
    > If you want to use email features (like email confirmation or password reset), configure your SendGrid secrets. Otherwise, make sure `Features:ConfirmRegistration` is set to `false` in `appsettings.Development.json`.
    > ```bash
    > dotnet user-secrets set "SendGrid:ApiKey" "YOUR_SENDGRID_API_KEY"
    > dotnet user-secrets set "SendGrid:FromEmail" "no-reply@yourdomain.com"
    > # ... and other SendGrid settings
    > ```

4.  **Run the application**
    ```bash
    dotnet run --project CareerPilotAi.csproj
    ```
    The application will start and connect to the PostgreSQL database running in Docker. On startup, it will automatically apply any pending database migrations.

5.  **Access the application**
    The application will be available at `https://localhost:5000` and `http://localhost:5001`.

### Configuration

The application is configured through `appsettings.json` and environment-specific files like `appsettings.Development.json`. Sensitive data should be stored using user secrets or environment variables.

#### Features

The `Features` section allows toggling certain application behaviors.

```json
"Features": {
  "ConfirmRegistration": true
}
```

-   **`ConfirmRegistration`**: When set to `true`, new users must confirm their email address to log in. For local development, it's recommended to set this to `false` in `appsettings.Development.json` to bypass email validation.

#### SendGrid (Email Service)

SendGrid is used for sending transactional emails like registration confirmation and password resets.

```json
"SendGrid": {
  "ApiKey": "YOUR_SENDGRID_API_KEY",
  "FromEmail": "no-reply@yourdomain.com",
  "RegistrationVerificationTemplateId": "d-xxxxxxxxxxxxxxxx",
  "PasswordResetTemplateId": "d-xxxxxxxxxxxxxxxx"
}
```

-   **`ApiKey`**: Your SendGrid API key.
-   **`FromEmail`**: The email address from which emails are sent.
-   **`RegistrationVerificationTemplateId`** and **`PasswordResetTemplateId`**: IDs of the dynamic templates created in your SendGrid account.

If you are not using email features, ensure `Features:ConfirmRegistration` is set to `false`.

#### OpenRouter (AI Integration)

OpenRouter provides access to various Large Language Models (LLMs) used for AI-powered features. An `AuthToken` is required.

```json
"OpenRouter": {
  "BaseAddress": "https://openrouter.ai",
  "AuthToken": "YOUR_OPENROUTER_TOKEN",
  "Features": [
    { "Name": "ParseJobDescription", "Model": "google/gemini-2.5-flash-lite-preview-09-2025", "Temperature": 0.3 },
    { "Name": "GenerateInterviewQuestions", "Model": "google/gemini-2.5-flash-lite-preview-09-2025", "Temperature": 1 },
    { "Name": "PrepareInterviewPreparationContent", "Model": "google/gemini-2.5-flash-lite-preview-09-2025", "Temperature": 0.5 }
  ]
}
```

-   **`AuthToken`**: Your OpenRouter API key. This is required for the application to function correctly.
-   **`Features`**: Each AI-powered feature can be configured independently. You can specify the `Model` to use and its `Temperature` (creativity level). This allows for fine-tuning the AI's behavior for different tasks.

### Testing

The project follows a comprehensive testing strategy with unit, integration, and end-to-end (E2E) tests. The testing stack includes:
- **Test Framework**: xUnit
- **Mocking**: NSubstitute
- **Assertions**: Shouldly
- **Data Generation**: Bogus
- **E2E Testing**: Playwright

The test projects are structured as follows:
```
tests/
├── CareerPilotAi.UnitTests/
├── CareerPilotAi.IntegrationTests/
└── CareerPilotAi.E2ETests/
```
- **Unit Tests (`UnitTests`)**: Fully implemented, verifying individual application components in isolation.
- **Integration Tests (`IntegrationTests`)**: Currently in the planning stage and not yet implemented.
- **End-to-End Tests (`E2ETests`)**: Verify complete application flows from the user's perspective.

#### Running E2E Tests

E2E tests are designed for flexibility. By default, they automatically set up the required environment (the application and database) in Docker containers using the **Testcontainers** library.

For easier development and debugging, the environment can also be run manually using the `docker-compose-e2e-test.yml` file. To switch to this mode, change the `isSelfHostedRun` flag to `false` in `tests/CareerPilotAi.E2ETests/E2ETestFixture.cs`. In this mode, the tests will expect the containers to be running, allowing for faster iterations and database state inspection.

To run all tests, use the command from the root directory:
```bash
dotnet test
```

To run specific test suites:
```bash
# Run Unit Tests
dotnet test tests/CareerPilotAi.UnitTests

# Run End-to-End (E2E) Tests
dotnet test tests/CareerPilotAi.E2ETests
```

### Project roadmap

- CV/Resume generation and tailoring
- Cover letter generation
- Advance interview preparation with AI Agent
- Bulk actions, archiving, pagination
- Onboarding/tutorial
- Advanced analytics and metrics
- Job Application editing
- Implement OpenTelemetry + logs improvement
- Implement cache mechanisms
- Optimise db queries and indexes

### Project status
Active development.