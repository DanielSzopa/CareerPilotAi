## CareerPilotAi

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-2C3E50)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-336791?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)

### Table of Contents
- [Project description](#project-description)
- [Tech stack](#tech-stack)
- [Getting started locally](#getting-started-locally)
- [Available scripts](#available-scripts)
- [Project scope](#project-scope)
- [Project status](#project-status)
- [License](#license)

### Project description
CareerPilotAi is a web application that helps job seekers organize and track their job applications with intelligent assistance. Users can paste a job posting, and AI will structure the details into a manageable application entry. The system centralizes applications, enables quick status updates, and provides a dashboard with key metrics to monitor progress.

Key value:
- Save time with automated parsing of job postings
- Keep all applications and their status history in one place
- Filter, search, and review details quickly
- Prepare for interviews with AI-generated questions and preparation content

For authentication and AI architecture, see:
- `docs/auth-flows.md` – end-to-end Identity flows (login, register, email confirmation, reset password)
- `docs/prompts-architecture.md` – prompt system, OpenRouter integration, and best practices

### Tech stack
- **Frontend**: ASP.NET Core MVC (Razor Views), Bootstrap 5, JavaScript, Chart.js
- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core, ASP.NET Identity
- **Database**: PostgreSQL
- **AI Integration**: OpenRouter (configurable models per feature)
- **Email**: SendGrid (account verification and password reset)
- **CI/CD & Hosting**: GitHub Actions (planned), Docker (local), Azure (optional)

### Getting started locally

Prerequisites:
- .NET SDK 8.0+
- Docker Desktop (with Docker Compose v2)
- Git
- OpenRouter API key (for AI features)
- SendGrid API key and template IDs (for email confirmation/password reset)

1) Clone the repository
```bash
git clone https://github.com/your-org/CareerPilotAi.git
cd CareerPilotAi
```

2) Start PostgreSQL with Docker Compose
```bash
docker compose up -d
```
Defaults from `docker-compose.yml`:
- Image: postgres:15
- Port: 5432 → 5432
- Database: `careerpilotdb`
- Username: `postgres`
- Password: `postgres`

3) Configure application settings (development)
- The project reads `CareerPilotAi/appsettings.json` and supports user-secrets for sensitive values.
- Recommended: store secrets with `dotnet user-secrets` (per project).

```bash
# from repo root
dotnet user-secrets init -p CareerPilotAi/CareerPilotAi.csproj

# OpenRouter
dotnet user-secrets set -p CareerPilotAi/CareerPilotAi.csproj "OpenRouter:AuthToken" "YOUR_OPENROUTER_TOKEN"

# SendGrid (required for email confirmation & password reset)
dotnet user-secrets set -p CareerPilotAi/CareerPilotAi.csproj "SendGrid:ApiKey" "YOUR_SENDGRID_API_KEY"
dotnet user-secrets set -p CareerPilotAi/CareerPilotAi.csproj "SendGrid:FromEmail" "no-reply@example.com"
dotnet user-secrets set -p CareerPilotAi/CareerPilotAi.csproj "SendGrid:RegistrationVerificationTemplateId" "d-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
dotnet user-secrets set -p CareerPilotAi/CareerPilotAi.csproj "SendGrid:PasswordResetTemplateId" "d-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
```

Configuration reference (excerpt):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=careerpilotdb;Username=postgres;Password=postgres"
  },
  "SendGrid": {
    "ApiKey": "",
    "FromEmail": "",
    "RegistrationVerificationTemplateId": "",
    "PasswordResetTemplateId": ""
  },
  "OpenRouter": {
    "BaseAddress": "https://openrouter.ai",
    "AuthToken": "",
    "Features": [
      { "Name": "EnhanceJobDescription", "Model": "google/gemini-2.0-flash-lite-001", "Temperature": 0.5 },
      { "Name": "PersonalDetailsPdfUpload", "Model": "google/gemini-2.0-flash-lite-001" },
      { "Name": "GenerateInterviewQuestions", "Model": "google/gemini-2.5-flash", "Temperature": 1 },
      { "Name": "PrepareInterviewPreparationContent", "Model": "google/gemini-2.0-flash-lite-001", "Temperature": 0.5 }
    ]
  }
}
```

4) Create the database schema (EF Core migrations)
```bash
# optional: install EF CLI if not present
dotnet tool install --global dotnet-ef

# apply migrations against the development database
dotnet ef database update -p CareerPilotAi/CareerPilotAi.csproj -s CareerPilotAi/CareerPilotAi.csproj
```

5) Run the application
```bash
dotnet run --project CareerPilotAi/CareerPilotAi.csproj
```
Default URLs (from launch settings):
- HTTPS: `https://localhost:5000`
- HTTP: `http://localhost:5001`

6) Sign up and sign in
- Browse to `/auth/register` to create an account. Email confirmation is required by default.
- Ensure SendGrid is configured to receive confirmation and reset emails.

7) Using AI features
- Ensure `OpenRouter:AuthToken` is set via user-secrets. Feature models and temperatures are configured in `appsettings.json` under `OpenRouter:Features`.

### Available scripts

Common commands:
- Restore dependencies
  ```bash
  dotnet restore
  ```
- Build
  ```bash
  dotnet build
  ```
- Run (development)
  ```bash
  dotnet run --project CareerPilotAi/CareerPilotAi.csproj
  ```
- Run tests
  ```bash
  dotnet test
  ```
- Apply EF Core migrations
  ```bash
  dotnet ef database update -p CareerPilotAi/CareerPilotAi.csproj -s CareerPilotAi/CareerPilotAi.csproj
  ```
- Start/stop database (Docker Compose)
  ```bash
  docker compose up -d
  docker compose down -v
  ```

### Project scope

Per the Product Requirements Document (PRD):

In MVP scope:
- Add application with AI parsing from pasted job ad
- Full CRUD for job applications
- Status system with history of changes
- Filtering and search (status, salary range, location, work mode, experience; text search)
- Sorting by date added
- Dashboard with key metrics and charts (status distribution, applications over time, recent activity)
- Responsive design (mobile-first)
- User authentication and authorization

Out of MVP scope (post-MVP examples):
- CV/Resume generation and tailoring
- Cover letter generation
- Bulk actions, archiving, pagination
- Onboarding/tutorial and landing page for anonymous users
- Monetization, multi-currency
- Advanced analytics and metrics

### Project status
- Active development; not production-hardened yet.
- Authentication, email flows, and AI features follow the patterns documented in `docs/auth-flows.md` and `docs/prompts-architecture.md`.

### License
No license has been specified yet. It will be added later.