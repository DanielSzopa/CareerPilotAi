---
goal: Implement Forgotten Password Feature for CareerPilotAi
version: 1.0
date_created: 2025-07-22
last_updated: 2025-07-22
owner: Development Team
tags: [feature, authentication, security, email, identity]
---

# Introduction

This plan outlines the implementation of a comprehensive forgotten password feature for the CareerPilotAi application. The feature will allow users to securely reset their passwords through an email-based verification process, integrating with the existing ASP.NET Core Identity framework and SendGrid email service.

## 1. Requirements & Constraints

- **REQ-001**: Implement forgotten password functionality accessible from the login page
- **REQ-002**: User must enter email address to initiate password reset process
- **REQ-003**: Send password reset email with secure token to registered users only
- **REQ-004**: Redirect to confirmation page regardless of email existence (security measure)
- **REQ-005**: Password reset page must be accessible only with valid token
- **REQ-006**: New password must comply with existing password validation rules
- **REQ-007**: Include password confirmation field for validation
- **REQ-008**: Token-based security to prevent unauthorized password resets
- **SEC-001**: Prevent email enumeration attacks by showing same confirmation message
- **SEC-002**: Use secure token generation from ASP.NET Core Identity
- **SEC-003**: Tokens must expire within reasonable timeframe (default 24 hours)
- **SEC-004**: Validate token ownership before allowing password reset
- **CON-001**: Must integrate with existing ASP.NET Core Identity configuration
- **CON-002**: Must use existing SendGrid email service infrastructure
- **CON-003**: Must follow existing password validation rules from IdentityExtensions
- **CON-004**: Must maintain consistent UI/UX styling with existing authentication pages
- **STY-001**: Use identical styling patterns from Login.cshtml and Register.cshtml for form pages
- **STY-002**: Use identical styling patterns from RegisterConfirmation.cshtml for confirmation pages
- **STY-003**: Maintain consistent login-container, login-card, and login-input CSS classes
- **STY-004**: Use same color scheme (#4a90e2 primary, #357abd hover, #2c3e50 text)
- **STY-005**: Include password toggle functionality with eye icons for password fields
- **STY-006**: Use Font Awesome icons consistently (envelope for confirmation pages)
- **STY-007**: Maintain responsive design with same breakpoints and grid structure
- **STY-008**: Use @@media syntax for CSS media queries within Razor views
- **GUD-001**: Follow existing C# coding standards and naming conventions
- **GUD-002**: Use structured logging for security and audit purposes
- **GUD-003**: Implement proper error handling and user feedback
- **PAT-001**: Follow existing controller action patterns (GET/POST pairs)
- **PAT-002**: Use existing view model pattern for data binding
- **PAT-003**: Follow existing email service pattern for consistency

## 2. Implementation Steps

### Phase 1: Backend Infrastructure Updates

#### TASK-001: Update SendGrid Configuration
- Add `PasswordResetTemplateId` property to `SendGridAppSettings` class
- Update email service configuration to include password reset template
- Update appsettings.json files with new template ID configuration

#### TASK-002: Extend EmailService
- Add `SendPasswordResetEmailAsync` method to `EmailService` class
- Implement proper error handling and logging for password reset emails
- Add validation for password reset template configuration

#### TASK-003: Add Controller Actions
- Add `ForgotPassword` GET action to `AuthController`
- Add `ForgotPassword` POST action to `AuthController`
- Add `ForgotPasswordConfirmation` GET action to `AuthController`
- Add `ResetPassword` GET action to `AuthController`
- Add `ResetPassword` POST action to `AuthController`
- Add `ResetPasswordConfirmation` GET action to `AuthController`

### Phase 2: View Models and Validation

#### TASK-004: Create Additional View Model
- Create `ForgotPasswordConfirmationViewModel` for confirmation page messaging
- Ensure all view models follow existing validation patterns

### Phase 3: Frontend Implementation

#### TASK-005: Create Forgotten Password Views
- Create `ForgotPassword.cshtml` view with email input form using Login.cshtml styling pattern
- Create `ForgotPasswordConfirmation.cshtml` view using RegisterConfirmation.cshtml styling pattern
- Create `ResetPassword.cshtml` view with password and confirmation fields using Register.cshtml styling pattern
- Create `ResetPasswordConfirmation.cshtml` view using RegisterConfirmation.cshtml styling pattern
- Include password toggle functionality for password fields (eye icons)
- Use Font Awesome envelope icon for confirmation pages
- Maintain consistent responsive design and CSS class structure
- Ensure all styling uses identical color scheme and transitions

#### TASK-006: Styling Consistency Validation
- Verify all new views use identical CSS classes: login-container, login-card, login-input, login-btn
- Ensure consistent padding, margins, and responsive breakpoints match existing pages
- Validate color scheme consistency (#4a90e2, #357abd, #2c3e50, #7f8c8d)
- Test responsive behavior across all device sizes

### Phase 4: Manual Testing and Validation

#### TASK-007: Manual Flow Testing
- Test complete forgotten password flow end-to-end manually
- Verify token security and expiration behavior
- Test email delivery and template rendering
- Validate password rules enforcement matches registration requirements
- Verify styling consistency across all new pages

## 4. Dependencies

- **DEP-001**: ASP.NET Core Identity framework (already configured)
- **DEP-002**: SendGrid email service (already configured) 
- **DEP-003**: Existing password validation rules from `IdentityExtensions`
- **DEP-004**: Entity Framework Core for user data access
- **DEP-005**: SendGrid password reset email template (to be created)

## 5. Files

- **FILE-001**: `CareerPilotAi/Infrastructure/Email/SendGridAppSettings.cs` - Add password reset template ID
- **FILE-002**: `CareerPilotAi/Infrastructure/Email/EmailService.cs` - Add password reset email method
- **FILE-003**: `CareerPilotAi/Controllers/AuthController.cs` - Add forgotten password actions
- **FILE-004**: `CareerPilotAi/Models/Authentication/ForgotPasswordConfirmationViewModel.cs` - New view model
- **FILE-005**: `CareerPilotAi/Views/Auth/ForgotPassword.cshtml` - Forgotten password form
- **FILE-006**: `CareerPilotAi/Views/Auth/ForgotPasswordConfirmation.cshtml` - Confirmation page
- **FILE-007**: `CareerPilotAi/Views/Auth/ResetPassword.cshtml` - Password reset form
- **FILE-008**: `CareerPilotAi/Views/Auth/ResetPasswordConfirmation.cshtml` - Success confirmation
- **FILE-009**: `CareerPilotAi/appsettings.json` - Add password reset template configuration
- **FILE-010**: `CareerPilotAi/appsettings.Development.json` - Add development template configuration

## 6. Manual Testing Checklist

- **MANUAL-001**: Test forgotten password form submission with valid email
- **MANUAL-002**: Test forgotten password form submission with non-existent email
- **MANUAL-003**: Verify confirmation page displays same message for both scenarios
- **MANUAL-004**: Test password reset link in email redirects to correct page
- **MANUAL-005**: Test password reset form with valid token and matching passwords
- **MANUAL-006**: Test password reset form with invalid/expired token
- **MANUAL-007**: Verify password validation rules match registration requirements
- **MANUAL-008**: Test responsive design on mobile and desktop devices
- **MANUAL-009**: Verify styling consistency with existing authentication pages
- **MANUAL-010**: Test complete user journey from login to password reset completion

## 7. Related Specifications / Further Reading

- [ASP.NET Core Identity Password Reset Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm)
- [SendGrid Dynamic Templates Documentation](https://docs.sendgrid.com/ui/sending-email/how-to-send-an-email-with-dynamic-transactional-templates)
- [ASP.NET Core Security Best Practices](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [OWASP Authentication Guidelines](https://owasp.org/www-project-authentication-cheat-sheet/)