---
goal: Implement Status Display Features for Job Application Cards with Status Summary Bar
version: 1.0
date_created: 2025-01-23
last_updated: 2025-01-23
owner: Development Team
tags: [feature, ui, enhancement, job-applications]
---

# Introduction

This plan implements comprehensive status display features for the job application cards system. The goal is to add a status summary bar above the job application cards showing quantities for each status, and to display status indicators with colored dots on individual cards. The implementation enhances user visibility into their job application pipeline without adding filtering functionality.

## 1. Requirements & Constraints

- **REQ-001**: Display status summary bar above job application cards showing all 7 statuses with quantities
- **REQ-002**: Add status indicator in top-right corner of each job application card
- **REQ-003**: Each status must display with a colored dot and status name (e.g., "• Draft", "• Rejected")
- **REQ-004**: Status dots must use specific color coding: Draft (grey), Rejected (red), Submitted (green), Interview Scheduled (green), Waiting for Offer (green), Received Offer (green), No Contact (red)
- **REQ-005**: Status dots must be centered vertically and horizontally within their container
- **REQ-006**: Use existing `JobApplicationCardsViewModel` model which already contains status quantities
- **REQ-007**: No filtering functionality required - display only
- **REQ-008**: No unit tests required for this implementation
- **CON-001**: Must maintain existing card layout and functionality
- **CON-002**: Must follow existing Bootstrap styling patterns in the application
- **CON-003**: Must be responsive across all device sizes
- **PAT-001**: Follow C# coding standards from project instructions
- **PAT-002**: Use CSS custom properties for color management and consistency

## 2. Implementation Steps

### Phase 1: Update View Model Binding
- **TASK-001**: Update Index.cshtml to use `JobApplicationCardsViewModel` instead of `List<JobApplicationCardViewModel>`
- **TASK-002**: Verify controller is already properly returning the `JobApplicationCardsViewModel` with status quantities

### Phase 2: Implement Status Summary Bar
- **TASK-003**: Create status summary bar component above job application cards
- **TASK-004**: Display all 7 statuses with their respective quantities in a responsive grid layout
- **TASK-005**: Add consistent styling matching the application's design system
- **TASK-006**: Ensure proper spacing and alignment with existing page elements

### Phase 3: Add Status Indicators to Cards
- **TASK-007**: Add status indicator container in top-right corner of each card
- **TASK-008**: Implement colored dot system with proper centering for each status
- **TASK-009**: Add status text next to each dot using the format "• StatusName"
- **TASK-010**: Ensure status indicators don't interfere with existing card content layout

### Phase 4: Implement Color Coding System
- **TASK-011**: Define CSS custom properties for status colors (grey, red, green)
- **TASK-012**: Create CSS classes for each status type with appropriate dot colors
- **TASK-013**: Implement dynamic class assignment based on job application status
- **TASK-014**: Ensure color accessibility and contrast requirements are met

### Phase 5: Responsive Design Implementation
- **TASK-015**: Test and adjust status summary bar for mobile, tablet, and desktop viewports
- **TASK-016**: Ensure status indicators on cards remain properly positioned across all screen sizes
- **TASK-017**: Verify text remains readable and dots maintain proper size scaling

## 3. Alternatives

- **ALT-001**: Use separate API endpoint for status quantities - rejected because controller already provides this data in `JobApplicationCardsViewModel`
- **ALT-002**: Implement status as badges instead of dots - rejected to maintain specific requirement for dot-based design
- **ALT-003**: Add status filtering dropdown - rejected as explicitly not required in specifications
- **ALT-004**: Use icon fonts instead of CSS dots - rejected for simpler implementation and better accessibility

## 4. Dependencies

- **DEP-001**: Existing `JobApplicationCardsViewModel` class with status quantity properties
- **DEP-002**: Existing `ApplicationStatus` class containing all status definitions
- **DEP-003**: Bootstrap CSS framework already integrated in the application
- **DEP-004**: FontAwesome icons (if needed for enhanced status display)

## 5. Files

- **FILE-001**: `Views/JobApplication/Index.cshtml` - Main view file requiring model type update and UI enhancements
- **FILE-002**: `Models/JobApplication/JobApplicationCardsViewModel.cs` - View model already contains required status quantities (verify only)
- **FILE-003**: `Controllers/JobApplicationController.cs` - Controller already populates status quantities (verify only)
- **FILE-004**: CSS styles within Index.cshtml - Add new styles for status summary and status indicators

## 6. Testing

- **TEST-001**: Manual testing across different screen sizes (mobile, tablet, desktop)
- **TEST-002**: Visual verification of status summary bar displaying correct quantities
- **TEST-003**: Verification of status dot colors matching specifications
- **TEST-004**: Testing with various job application datasets (empty, single status, multiple statuses)
- **TEST-005**: Accessibility testing for color contrast and screen reader compatibility
- **TEST-006**: Cross-browser compatibility testing

## 7. Risks & Assumptions

- **RISK-001**: Current controller implementation may not be properly passing `JobApplicationCardsViewModel` to view
- **RISK-002**: CSS changes might conflict with existing card styling
- **RISK-003**: Color accessibility requirements might not be met with specified colors
- **ASSUMPTION-001**: Existing `JobApplicationCardsViewModel` contains accurate status quantity calculations
- **ASSUMPTION-002**: All job applications have valid status values matching `ApplicationStatus` definitions
- **ASSUMPTION-003**: Bootstrap grid system is available and functional in current view
- **ASSUMPTION-004**: No internationalization requirements for status names

## 8. Related Specifications / Further Reading

- [Bootstrap Grid Documentation](https://getbootstrap.com/docs/5.1/layout/grid/)
- [CSS Custom Properties](https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_custom_properties)
- [Web Accessibility Color Contrast Guidelines](https://www.w3.org/WAI/WCAG21/Understanding/contrast-minimum.html)
- [ASP.NET Core Razor Views Documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/overview)
