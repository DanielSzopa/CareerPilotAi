---
goal: Add Application Status Dropdown to Job Application Details Page
version: 1.0
date_created: 2025-08-04
last_updated: 2025-08-04
owner: CareerPilotAi Team
tags: [feature, ui, job-application, frontend]
---

# Introduction

This plan outlines the steps to add an application status dropdown to the `JobApplicationDetails.cshtml` view. This will allow users to view and update the status of their job application directly from the details page, ensuring a consistent user experience with the main job applications list (`Index.cshtml`). The implementation will be front-end only for now.

## 1. Requirements & Constraints

- **REQ-001**: The current application status must be displayed on the `JobApplicationDetails` page.
- **REQ-002**: The status display must include a colored dot and the status name, consistent with the styling on the `Index.cshtml` page.
- **REQ-003**: Users must be able to change the application status using a dropdown menu.
- **REQ-004**: The dropdown should be positioned to the right of the job title in the page header.
- **REQ-005**: Selecting a new status from the dropdown should trigger a JavaScript function.
- **CON-001**: The implementation for this plan is front-end only. No backend endpoint creation or logic is required at this stage.
- **GUD-001**: Follow existing styling and component design from `Index.cshtml` for status indicators to ensure visual consistency.
- **PAT-001**: Use Bootstrap 5 components for the dropdown menu.

## 2. Implementation Steps

### Phase 1: HTML Structure

- **TASK-1.1**: Modify `CareerPilotAi/Views/JobApplication/JobApplicationDetails.cshtml`.
- **Description**: Add the HTML structure for the status dropdown next to the job title. This will be a Bootstrap dropdown component. The dropdown button will display the current status, and the dropdown menu will contain all possible statuses.
- **Details**:
    - Locate the `div` containing the `h2` element for the job title.
    - Within the same flex container, add a `div` with the class `dropdown` for the status selector.
    - The dropdown button (`<button>`) should have an ID (e.g., `statusDropdown`), and it will display the current status. It must contain a `<span>` for the colored dot and another `<span>` for the status text.
    - The dropdown menu (`<ul>`) will be populated with a list item (`<li>`) containing a link (`<a>`) for each status defined in `Core/ApplicationStatus.cs`.
    - Each link must have a `data-status` attribute holding the status name (e.g., `data-status="Submitted"`) and a `data-color-class` attribute for the corresponding CSS class (e.g., `data-color-class="status-submitted"`).

### Phase 2: CSS Styling

- **TASK-2.1**: Add CSS to `CareerPilotAi/Views/JobApplication/JobApplicationDetails.cshtml`.
- **Description**: Add a `<style>` block to the view to style the status dropdown and indicators to match the design in `Index.cshtml`.
- **Details**:
    - Copy the `:root` CSS variables for all status colors from `Index.cshtml` to ensure color consistency.
    - Define CSS classes for each status (e.g., `.status-draft`, `.status-rejected`) to set the `background-color` of the status dot.
    - Style the dropdown button to vertically align with the job title and company name.
    - Ensure the dropdown menu items also have the colored dot for better visual feedback.

### Phase 3: JavaScript Functionality

- **TASK-3.1**: Add JavaScript to `CareerPilotAi/Views/JobApplication/JobApplicationDetails.cshtml`.
- **Description**: Implement the client-side logic to handle status changes within the `@section Scripts`.
- **Details**:
    - Add an event listener for click events on the dropdown menu items (`.dropdown-item`).
    - When a status is clicked:
        1.  Retrieve the new status text and color class from the `data-*` attributes of the clicked link.
        2.  Update the text of the `statusDropdown` button and the class of its colored dot to reflect the new selection.
        3.  Prevent the default link behavior using `e.preventDefault()`.
        4.  Call a placeholder function `updateApplicationStatus(jobApplicationId, newStatus)`.
        5.  For now, the `updateApplicationStatus` function will only log the `jobApplicationId` and `newStatus` to the browser's developer console.


## 4. Dependencies

- **DEP-001**: Bootstrap 5 (already available in the project).
- **DEP-002**: jQuery (already available in the project).
- **DEP-003**: The `JobApplicationDetailsViewModel` will eventually need to be updated on the backend to include `string CurrentStatus` and `IEnumerable<string> AllStatuses`. This is noted as a dependency for full feature completion. For this initial front-end plan, the statuses can be hardcoded in the view based on `ApplicationStatus.cs`.

## 5. Files

- **FILE-001**: `CareerPilotAi/Views/JobApplication/JobApplicationDetails.cshtml` - This is the primary file that will be modified to add the required HTML, CSS, and JavaScript.
- **FILE-002**: `CareerPilotAi/Models/JobApplication/JobApplicationDetailsViewModel.cs` - Will require modification in a future backend task (Dependency DEP-003).

## 6. Testing

- **TEST-001**: Manually verify that the status dropdown appears correctly on the Job Application Details page, positioned to the right of the job title.
- **TEST-002**: Verify that the dropdown displays all available statuses as defined in `ApplicationStatus.cs`.
- **TEST-003**: Verify that the color of the dot next to each status name is correct and consistent with `Index.cshtml`.
- **TEST-004**: Verify that selecting a new status from the dropdown correctly updates the main button's text and dot color.
- **TEST-005**: Verify that selecting a new status logs the expected message to the browser's developer console, including the job application ID and the selected status.
- **TEST-006**: Verify that the page header remains responsive and displays correctly on various screen sizes (desktop, tablet, mobile).

## 7. Risks & Assumptions

- **RISK-001**: The new UI component might affect the responsiveness of the page header. It must be thoroughly tested on different viewports to ensure it doesn't break the layout.
- **ASSUMPTION-001**: The `JobApplicationDetailsViewModel` can be modified in the future to provide the necessary data (`CurrentStatus`, `AllStatuses`). For the current scope, it is assumed that hardcoding the status list in the view is acceptable.

## 8. Related Specifications / Further Reading

- [Bootstrap 5 Dropdowns Documentation](https://getbootstrap.com/docs/5.3/components/dropdowns/)
- `CareerPilotAi/Core/ApplicationStatus.cs` (for the list of statuses)
- `CareerPilotAi/Views/JobApplication/Index.cshtml` (for styling reference)
