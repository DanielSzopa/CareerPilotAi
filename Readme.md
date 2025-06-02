# Project description
I would like to create the application called CareerPilotAi for creating CV/Resume supported by AI. Resume should be adjusted to the job offer to fit future employer expectations.

Idea is to provide possibility to generate the Resume in the 3 steps.

### First Step
Provide the job offer details. User should see text area when he can add all job details
AI mechanism should select the most important things - but for First POC, dummy data would be enough, in order to present the flow of application.

### Second step
User can go to second step, only when all data are added in the first step. User can back to first step to correct something.

Second step is responsible to provide personal data about user. Experience, skills, educations, something about the user etc. The most common things resume/cv related.

User should have possibility to do it on two ways:

- Import pdf with resume, AI mechanism should select the most important things - but for First POC, dummy data would be enough, in order to present the flow of application.

- Provide data manualy

### Third step
User can go to Third step, only when Second step is finished. 

User should see all information from step number one and step number two. User should have possibility to correct something. This view is responsible for present the result from previous steps.

# Technologies
- Application should be write in ASP .NET Core MVC in C# language
- .NET 8
- ORM should be responsible for connecting with Database: EntityFramework
- Database Postgres, should be setup in docker compose
- Try to use firstly packages provided by ASP .NET Core platform instead of external dependencies

# Rules
- file namespaces for new classes
- private field should be in format _field instead of this.field
- If you need to pass any data to razor views, please create appropriate seperate ViewModel objects for that. e.g. ResumeViewModel, CarViewModel
- If you need to have object for DataSet entityFramework, create seperate classes e.g. ResumeDataObject. I don't want to use the same object for viewModel, model(domain) and db object.
- Please follow best practices of writing code


# Security
When designing and implementing this application, ensure that all code adheres to best security practices. The application must be built with protection against common web vulnerabilities, including but not limited to:

- SQL Injection: Use parameterized queries or ORM features to handle all database input.

- Cross-Site Scripting (XSS): Properly sanitize and encode all user-generated content before rendering it in the UI.

- Cross-Site Request Forgery (CSRF): Implement CSRF tokens in forms and ensure all state-changing requests are protected.

- Input Validation: Validate and sanitize all inputs on both client and server sides.

- Validate input files' e.g. pdf. Should be checked if it's really pdf file and shouldn't be very big, length should be limited.

- Error Handling: Avoid exposing sensitive information through error messages. Log detailed errors securely while displaying generic messages to users.

- HTTPS Only: Ensure the application enforces HTTPS and handles certificates securely.

- Apply the principle of least privilege, and follow OWASP Top Ten guidelines throughout the development process.