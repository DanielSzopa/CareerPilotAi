# Commit Message Guidelines for CareerPilotAi

## Overview
This document outlines the commit message standards for the CareerPilotAi project. Following these guidelines ensures a clean, readable git history that facilitates code reviews, debugging, and project maintenance.

## Commit Message Structure

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Required Elements
- **type**: The category of change (see types below)
- **subject**: Brief description (50 characters max)

### Optional Elements
- **scope**: The area of codebase affected
- **body**: Detailed explanation (when necessary)
- **footer**: References to issues, breaking changes

## Commit Types

### Primary Types (Use these most often)
- `feat`: New feature or functionality
- `fix`: Bug fix
- `refactor`: Code restructuring without changing functionality
- `docs`: Documentation changes only
- `style`: Code formatting, missing semicolons, etc. (no logic changes)
- `test`: Adding or updating tests
- `chore`: Build process, dependency updates, maintenance tasks

### Secondary Types (Use when applicable)
- `perf`: Performance improvements
- `security`: Security-related changes
- `config`: Configuration file changes
- `ci`: Continuous integration changes
- `revert`: Reverting a previous commit

## Scope Guidelines

Use these scopes to indicate which part of the application is affected:

### Core Application Areas
- `auth`: Authentication and authorization
- `api`: API controllers and endpoints
- `models`: Data models and entities
- `services`: Business logic services
- `db`: Database migrations, persistence layer
- `ui`: User interface components and views
- `validation`: Input validation and constraints

### Infrastructure & Tools
- `docker`: Docker configuration
- `deps`: Dependencies and packages
- `config`: Application configuration
- `middleware`: Request/response middleware
- `filters`: Action filters and attributes

### Feature-Specific Scopes
- `job-app`: Job application functionality
- `interview`: Interview questions generation
- `resume`: Resume builder features
- `backup`: Backup and restore functionality
- `email`: Email services and templates

## Subject Line Rules

1. **Length**: Maximum 50 characters
2. **Capitalization**: Start with uppercase letter
3. **Tense**: Use imperative mood ("Add" not "Added" or "Adding")
4. **Punctuation**: No trailing period
5. **Clarity**: Be specific and descriptive

### Good Examples
- `feat(auth): Add OAuth2 integration for Google login`
- `fix(job-app): Resolve null reference in application validation`
- `refactor(services): Extract common validation logic to base class`
- `docs(api): Update job application endpoint documentation`

### Bad Examples
- `fix bug` (too vague)
- `feat(auth): added new login feature.` (wrong tense, has period)
- `Update the user service to handle edge cases better` (too long)
- `WIP` (not descriptive)

## Body Guidelines

Include a body when:
- The change is complex or non-obvious
- Multiple files are affected
- The reasoning behind the change needs explanation
- Breaking changes are introduced

### Body Format
- Separate from subject with blank line
- Wrap at 72 characters
- Explain **what** and **why**, not **how**
- Use bullet points for multiple changes

### Example with Body
```
feat(interview): Add AI-powered question difficulty assessment

- Implement difficulty scoring algorithm based on question complexity
- Add new database fields for storing difficulty ratings
- Update UI to display difficulty indicators
- Integrate with OpenRouter API for advanced analysis

This enhancement helps users better prepare by understanding
question difficulty levels before practice sessions.
```

## Footer Guidelines

### Issue References
- `Fixes #123`: Closes the issue when commit is merged
- `Refs #123`: References the issue without closing
- `Resolves #123`: Alternative to "Fixes"

### Breaking Changes
```
BREAKING CHANGE: Authentication middleware now requires explicit user consent

The authentication flow has been updated to comply with new privacy
regulations. All existing integrations must be updated to include
the consent parameter.
```

## Special Commit Scenarios

### Merge Commits
Use the default merge commit message format:
```
Merge pull request #123 from feature/new-auth-system

feat(auth): Implement two-factor authentication
```

### Revert Commits
```
revert: feat(auth): Add OAuth2 integration

This reverts commit 1234567890abcdef due to security concerns
identified in the OAuth2 implementation.
```

### Hotfixes
```
fix(security): Patch SQL injection vulnerability in job search

Urgently addresses CVE-2024-XXXX by implementing parameterized
queries in the job application search functionality.

Fixes #456
```

## CareerPilotAi-Specific Guidelines

### Feature Development
- Use `feat(job-app)` for job application features
- Use `feat(interview)` for interview-related features
- Use `feat(resume)` for resume builder functionality
- Use `feat(auth)` for authentication improvements

### Bug Fixes
- Always include the affected component in scope
- Reference the issue number when available
- Describe the user-facing impact when relevant

### API Changes
- Use `feat(api)` or `fix(api)` as appropriate
- Include version information if applicable
- Document breaking changes in footer

### Database Changes
- Use `feat(db)` for new migrations
- Use `fix(db)` for correcting data issues
- Always include migration description

## Validation Checklist

Before committing, ensure your message:
- [ ] Uses approved type and scope
- [ ] Has clear, concise subject (≤50 chars)
- [ ] Uses imperative mood
- [ ] Includes body for complex changes
- [ ] References relevant issues
- [ ] Documents breaking changes
- [ ] Follows project naming conventions

## Tools and Automation

### Git Hooks
Consider implementing commit-msg hooks to validate:
- Message format compliance
- Subject line length
- Required elements presence

### IDE Integration
- Configure your IDE to show these guidelines
- Use commit message templates when available
- Enable spell-check for commit messages

## Examples by Category

### Feature Development
```
feat(job-app): Add bulk import functionality for job applications

- Support CSV and Excel file formats
- Implement progress tracking for large imports
- Add validation for required fields
- Include error reporting for failed imports

Resolves #234, #245
```

### Bug Fixes
```
fix(interview): Resolve question duplication in generated sets

Questions were appearing multiple times due to improper
randomization logic in the question selection algorithm.

Fixes #178
```

### Refactoring
```
refactor(services): Consolidate validation logic across controllers

- Extract common validation patterns to ValidationService
- Remove duplicate validation code from JobApplicationController
- Standardize error message formatting
- Improve unit test coverage for validation scenarios
```

### Documentation
```
docs(api): Add comprehensive OpenAPI documentation

- Document all job application endpoints
- Include request/response examples
- Add authentication requirements
- Update README with API usage guidelines
```

Remember: Good commit messages are an investment in your future self and your team. They make debugging, code reviews, and project maintenance significantly easier.