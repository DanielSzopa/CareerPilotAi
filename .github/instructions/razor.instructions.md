---
applyTo: '**/*.cshtml'
---

# ASP.NET Core Razor Views - Coding Standards & Guidelines

## Project Structure & Context
This is a **CareerPilotAi** application - a job application tracking and AI-powered career assistance platform. The application helps users manage job applications, generate interview questions, build CVs, and enhance job descriptions using AI.

## ViewData & Page Titles
- **Always set ViewData["Title"]** in every view
- Use descriptive, user-friendly titles that reflect the page content
- For details pages, combine job title and company: `Model.JobTitle + " - " + Model.CompanyName`
- Examples:
  ```csharp
  @{ ViewData["Title"] = "Job Applications"; }
  @{ ViewData["Title"] = "Log in"; }
  @{ ViewData["Title"] = Model.JobTitle + " - " + Model.CompanyName; }
  ```

## Layout & Structure Standards
- Use **Bootstrap 5** classes for responsive design
- Follow consistent container structure:
  ```html
  <div class="container-fluid">
      <div class="row">
          <div class="col-12">
              <!-- Page header with title and actions -->
          </div>
      </div>
  </div>
  ```

## Form Standards
### Form Layout
- Use **responsive form layouts** with Bootstrap grid system
- Center login/register forms: `<div class="row justify-content-center"><div class="col-md-6">`
- Use `form-group mb-3` for consistent spacing between form elements

### Form Controls & Validation
- **Always include ASP.NET Core Tag Helpers** for forms:
  ```html
  <form asp-action="ActionName" method="post">
      <div asp-validation-summary="All" class="text-danger"></div>
      
      <div class="form-group mb-3">
          <label asp-for="PropertyName"></label>
          <input asp-for="PropertyName" class="form-control" />
          <span asp-validation-for="PropertyName" class="text-danger"></span>
      </div>
  </form>
  ```

### Textarea & Word Counting
- For large text inputs, implement **word counting functionality**:
  ```html
  <textarea asp-for="JobDescription" class="form-control" rows="15"></textarea>
  <div class="form-text">Maximum 5,000 words. <span id="wordCount">0</span> words used.</div>
  <div id="wordLimitError" class="text-danger mt-1" style="display: none;">
      Text exceeds the maximum limit of 5,000 words.
  </div>
  ```
- Include word-counter JavaScript: `<script src="~/js/word-counter.js"></script>`

## Navigation & User Experience
### Consistent Navigation Patterns
- **Back buttons** should always be present: `<a asp-controller="Controller" asp-action="Index" class="btn btn-secondary"><i class="fas fa-arrow-left me-2"></i>Back</a>`
- Use **Font Awesome icons** with consistent spacing (`me-2` class)
- Action buttons on the right, navigation on the left

### Button Styling
- **Primary actions**: `btn btn-primary` with relevant icons
- **Secondary actions**: `btn btn-outline-secondary` or `btn btn-secondary`
- **Destructive actions**: `btn btn-outline-danger` (delete operations)
- **Success actions**: `btn btn-success` (save operations)

## Cards & Content Display
### Card Structure
- Use **Bootstrap cards** for content grouping:
  ```html
  <div class="card">
      <div class="card-header">
          <h5 class="card-title mb-0">Title</h5>
      </div>
      <div class="card-body">
          <!-- Content -->
      </div>
  </div>
  ```

### Job Application Cards
- Use **hover effects** and **responsive grid**:
  ```html
  <div class="col-12 col-md-6 col-lg-4 col-xl-3">
      <div class="card h-100 job-application-card">
          <div class="card-body d-flex flex-column">
              <!-- Card content with flex layout -->
          </div>
      </div>
  </div>
  ```

## Tab Navigation
- Use **Bootstrap tabs** for multi-section pages:
  ```html
  <ul class="nav nav-tabs" id="tabName" role="tablist">
      <li class="nav-item" role="presentation">
          <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#section1">
              <i class="fas fa-icon me-2"></i>Section Name
          </button>
      </li>
  </ul>
  ```

## JavaScript Integration
### Fetch API Standards
- Use **modern async/await** with proper error handling
- Include **CSRF protection**: `'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value`
- Implement **loading states** and **user feedback**
- Use **AbortController** for timeout handling (40-second timeout for AI operations)

### AJAX Patterns
```javascript
async function apiCall(url, data) {
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify(data)
        });
        
        if (response.ok) {
            const result = await response.json();
            // Handle success
        } else {
            // Handle error
        }
    } catch (error) {
        console.error('Error:', error);
        // Show user-friendly error message
    }
}
```

## Styling Guidelines
### CSS Organization
- Include **component-specific styles** within `<style>` tags in views
- Use **CSS custom properties** for consistent colors: `#4a90e2` (primary blue), `#357abd` (hover blue)
- Implement **hover effects** and **transitions** for interactive elements

### CSS Media Queries in Razor
- **IMPORTANT**: Use `@@media` instead of `@media` in CSS within Razor views
- The `@` symbol has special meaning in Razor syntax, so it must be escaped
- Example:
  ```css
  @@media (max-width: 768px) {
      .job-application-card {
          margin-bottom: 1rem;
      }
  }
  ```
- This prevents Razor compilation errors and ensures proper CSS rendering

### Responsive Design
- Use **Bootstrap responsive utilities**
- Test on mobile viewports with media queries when needed
- Ensure **accessibility** with proper ARIA labels and roles

## Error Handling & User Feedback
### Status Messages
- Create **dedicated containers** for dynamic messages: `<div id="statusMessageContainer"></div>`
- Use **Bootstrap alert classes** for consistent styling
- Implement **loading indicators** with spinners for long operations

### Validation
- Always include **client-side validation scripts**: `@await Html.PartialAsync("_ValidationScriptsPartial")`
- Use **server-side validation** with Tag Helpers
- Provide **real-time feedback** for form inputs

## AI Integration Patterns
- **AI Enhancement buttons**: Use `btn btn-outline-secondary btn-sm` with appropriate icons
- **Loading states**: Show spinners during AI processing
- **Error handling**: Graceful degradation when AI services are unavailable
- **Word limits**: Enforce and display word counts for AI-processed content

## Security & Performance
- **Always use CSRF tokens** in forms and AJAX requests
- **Minimize tool calls** by reading large chunks of files
- **Validate file uploads** (PDF processing, single file constraints)
- **Implement timeouts** for external API calls

## Common Patterns to Follow
1. **Consistent page headers** with titles and action buttons
2. **Responsive form layouts** with proper validation
3. **Card-based content organization**
4. **Tab navigation** for complex pages
5. **Font Awesome icons** with consistent spacing
6. **Bootstrap utility classes** for spacing and alignment
7. **Modern JavaScript** with proper error handling
8. **Loading states** and user feedback
9. **Accessible markup** with proper ARIA attributes
10. **Mobile-first responsive design**