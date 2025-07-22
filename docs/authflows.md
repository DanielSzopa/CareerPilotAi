# CareerPilotAi Authentication Workflows

This document provides detailed documentation of all authentication workflows in the CareerPilotAi application, including user journeys, system responses, and error handling scenarios.

## Table of Contents

1. [Registration Workflow](#registration-workflow)
   - 1.1. [GET /auth/register - Display Registration Form](#1-get-authregister---display-registration-form)
   - 1.2. [POST /auth/register - Process Registration](#2-post-authregister---process-registration)
     - 1.2.1. [Successful New User Registration](#scenario-21-successful-new-user-registration)
     - 1.2.2. [Registration with Existing Confirmed Email](#scenario-22-registration-with-existing-confirmed-email)
     - 1.2.3. [Registration with Existing Unconfirmed Email](#scenario-23-registration-with-existing-unconfirmed-email)
     - 1.2.4. [Registration Failure - User Creation Error](#scenario-24-registration-failure---user-creation-error)
     - 1.2.5. [Registration Failure - Email Sending Error](#scenario-25-registration-failure---email-sending-error)

2. [Email Confirmation Workflow](#email-confirmation-workflow)
   - 2.1. [GET /auth/confirm-email - Process Email Confirmation](#get-authconfirm-email---process-email-confirmation)
     - 2.1.1. [Successful Email Confirmation](#scenario-1-successful-email-confirmation)
     - 2.1.2. [Invalid Confirmation Parameters](#scenario-2-invalid-confirmation-parameters)
     - 2.1.3. [Non-existent User Confirmation](#scenario-3-non-existent-user-confirmation)
     - 2.1.4. [Invalid Token Confirmation](#scenario-4-invalid-token-confirmation)

3. [Resend Confirmation Workflow](#resend-confirmation-workflow)
   - 3.1. [GET /auth/resend-confirmation - Display Resend Form](#1-get-authresend-confirmation---display-resend-form)
   - 3.2. [POST /auth/resend-confirmation - Process Resend Request](#2-post-authresend-confirmation---process-resend-request)
     - 3.2.1. [Successful Resend for Unconfirmed User](#scenario-21-successful-resend-for-unconfirmed-user)
     - 3.2.2. [Resend for Non-existent User](#scenario-22-resend-for-non-existent-user)
     - 3.2.3. [Resend for Already Confirmed User](#scenario-23-resend-for-already-confirmed-user)
     - 3.2.4. [Resend Email Sending Failure](#scenario-24-resend-email-sending-failure)

4. [Login Workflow](#login-workflow)
   - 4.1. [GET /auth/login - Display Login Form](#1-get-authlogin---display-login-form)
   - 4.2. [POST /auth/login - Process Login](#2-post-authlogin---process-login)
     - 4.2.1. [Successful Login](#scenario-21-successful-login)
     - 4.2.2. [Login with Unconfirmed Email (Valid Password)](#scenario-22-login-with-unconfirmed-email-valid-password)
     - 4.2.3. [Login with Non-existent User](#scenario-23-login-with-non-existent-user)
     - 4.2.4. [Login with Unconfirmed Email and Invalid Password](#scenario-24-login-with-unconfirmed-email-and-invalid-password)
     - 4.2.5. [Invalid Credentials (General)](#scenario-25-invalid-credentials-general)
     - 4.2.6. [Model Validation Failure](#scenario-26-model-validation-failure)

5. [Logout Workflow](#logout-workflow)
   - 5.1. [POST /auth/logout - Process Logout](#post-authlogout---process-logout)

6. [Forgot/Reset Password Workflow](#forgotreset-password-workflow)
   - 6.1. [GET /auth/forgot-password - Display Forgot Password Form](#1-get-authforgot-password---display-forgot-password-form)
   - 6.2. [POST /auth/forgot-password - Process Forgot Password Request](#2-post-authforgot-password---process-forgot-password-request)
     - 6.2.1. [Successful Password Reset Request for Confirmed User](#scenario-21-successful-password-reset-request-for-confirmed-user)
     - 6.2.2. [Password Reset Request for Non-existent User](#scenario-22-password-reset-request-for-non-existent-user)
     - 6.2.3. [Password Reset Request for Unconfirmed User](#scenario-23-password-reset-request-for-unconfirmed-user)
     - 6.2.4. [Password Reset Email Sending Failure](#scenario-24-password-reset-email-sending-failure)
   - 6.3. [GET /auth/forgot-password-confirmation - Display Confirmation Page](#3-get-authforgot-password-confirmation---display-confirmation-page)
   - 6.4. [GET /auth/reset-password - Display Reset Password Form](#4-get-authreset-password---display-reset-password-form)
     - 6.4.1. [Valid Reset Link Access](#scenario-41-valid-reset-link-access)
     - 6.4.2. [Invalid Reset Link Parameters](#scenario-42-invalid-reset-link-parameters)
     - 6.4.3. [Reset Link with Non-existent User](#scenario-43-reset-link-with-non-existent-user)
   - 6.5. [POST /auth/reset-password - Process Password Reset](#5-post-authreset-password---process-password-reset)
     - 6.5.1. [Successful Password Reset](#scenario-51-successful-password-reset)
     - 6.5.2. [Password Reset with Invalid/Expired Token](#scenario-52-password-reset-with-invalidexpired-token)
     - 6.5.3. [Password Reset with Non-existent User](#scenario-53-password-reset-with-non-existent-user)
     - 6.5.4. [Password Reset Failure - Password Requirements](#scenario-54-password-reset-failure---password-requirements)
   - 6.6. [GET /auth/reset-password-confirmation - Display Success Page](#6-get-authreset-password-confirmation---display-success-page)

7. [Error Handling and Security Notes](#error-handling-and-security-notes)
   - 7.1. [General Error Handling](#general-error-handling)
   - 7.2. [Security Considerations](#security-considerations)
   - 7.3. [Logging Strategy](#logging-strategy)

---

## Registration Workflow

### Overview
The registration process allows new users to create accounts that require email confirmation before login.

### 1. GET /auth/register - Display Registration Form

**User Action:** User navigates to registration page
**System Response:** 
- Returns `Register.cshtml` view
- Shows form with Email, Password, ConfirmPassword fields
- Includes password toggle functionality

---

### 2. POST /auth/register - Process Registration

#### Scenario 2.1: Successful New User Registration

**User Action:** User submits valid registration form with new email
**Preconditions:** 
- Valid email format
- Password meets requirements (8+ chars, upper/lower/digit/special)
- Passwords match
- Email doesn't exist in system

**System Process:**
1. Validates model state
2. Creates new `IdentityUser` with email as username
3. Calls `UserManager.CreateAsync()` to create user
4. Generates email confirmation token
5. Sends confirmation email via `EmailService`
6. Logs successful user creation

**User Experience:**
- Redirected to `/auth/register-confirmation`
- Sees `RegisterConfirmation.cshtml` with message: "We've sent a confirmation email..."
- Email sent with confirmation link

**Logging:** `"User created a new account with email: {Email}"`

---

#### Scenario 2.2: Registration with Existing Confirmed Email

**User Action:** User tries to register with email that already has confirmed account
**Preconditions:** 
- Email already exists in system
- Existing user has confirmed email

**System Process:**
1. Finds existing user by email
2. Checks if email is confirmed (`IsEmailConfirmedAsync`)
3. Adds model error
4. Returns same registration view with error

**User Experience:**
- Stays on registration page
- Sees error: "An account with this email already exists. Please log in or reset your password."

**Logging:** `"User tried to register with email {Email}, but account already exists and is confirmed."`

---

#### Scenario 2.3: Registration with Existing Unconfirmed Email

**User Action:** User tries to register with email that exists but isn't confirmed
**Preconditions:**
- Email already exists in system  
- Existing user has NOT confirmed email

**System Process:**
1. Finds existing user by email
2. Checks email confirmation status
3. Redirects to confirmation page with special flag

**User Experience:**
- Redirected to `/auth/register-confirmation?isAlreadyRegisteredButNotConfirmed=true`
- Sees message: "An account with this email already exists but is not confirmed. Please check your email for the confirmation link or resend it."

**Logging:** `"User tried to register with email {Email}, but account already exists and is not confirmed."`

---

#### Scenario 2.4: Registration Failure - User Creation Error

**User Action:** User submits valid form but user creation fails
**Preconditions:** Valid form data, but `UserManager.CreateAsync()` fails

**System Process:**
1. Validates model successfully
2. User creation fails (duplicate username, database error, etc.)
3. Logs all creation errors
4. Adds generic error to model state

**User Experience:**
- Stays on registration page
- Sees error: "Sign up failed, something happened on our end. Please contact support."

**Logging:** `"Error creating user {Email}: {Error}"`

---

#### Scenario 2.5: Registration Failure - Email Sending Error

**User Action:** User registers successfully but confirmation email fails to send
**Preconditions:** User created successfully, but email service throws exception

**System Process:**
1. User created successfully in database
2. Email confirmation link generated
3. Email service throws exception
4. Catches exception and adds model error

**User Experience:**
- Stays on registration page  
- Sees error: "Sign up failed, something happened on our end. Please contact support."
- User account exists but no confirmation email sent

**Logging:** `"Failed to send confirmation email to user: {Email}"`

---

## Email Confirmation Workflow

### Overview
Users must confirm their email addresses before they can log in. Confirmation happens via links sent to their email.

### GET /auth/confirm-email - Process Email Confirmation

#### Scenario 1: Successful Email Confirmation

**User Action:** User clicks confirmation link from email
**URL Parameters:** `userId` and `token`
**Preconditions:**
- Valid userId and token in URL
- User exists in system
- Token is valid and not expired

**System Process:**
1. Validates userId and token are provided
2. Finds user by userId
3. Calls `UserManager.ConfirmEmailAsync()` with token
4. Signs user in automatically
5. Redirects to home page

**User Experience:**
- Automatically signed in
- Redirected to `/` (Home page)
- Can now use the application

**Logging:** `"Email confirmed for user: {Email}, {UserId}"`

---

#### Scenario 2: Invalid Confirmation Parameters

**User Action:** User clicks malformed confirmation link or missing parameters
**Preconditions:** Missing or empty `userId` or `token` parameters

**System Process:**
1. Validates parameters
2. Logs warning about missing parameters
3. Redirects to error page

**User Experience:**
- Redirected to `/Home/Error`
- Sees generic error page

**Logging:** `"Email confirmation attempted with missing userId or token"`

---

#### Scenario 3: Non-existent User Confirmation

**User Action:** User clicks confirmation link with invalid userId
**Preconditions:** Valid format parameters but userId doesn't exist

**System Process:**
1. Attempts to find user by userId
2. User not found
3. Logs warning
4. Redirects to error page

**User Experience:**
- Redirected to `/Home/Error`
- Sees generic error page

**Logging:** `"Email confirmation attempted for non-existent user: {UserId}"`

---

#### Scenario 4: Invalid Token Confirmation

**User Action:** User clicks confirmation link with expired or invalid token
**Preconditions:** Valid userId but invalid/expired token

**System Process:**
1. Finds user successfully
2. Token validation fails in `ConfirmEmailAsync()`
3. Logs detailed error information
4. Redirects to error page

**User Experience:**
- Redirected to `/Home/Error`  
- Sees generic error page

**Logging:** `"Email confirmation failed for user: {Email}, {UserId}, Errors: {Errors}"`

---

## Resend Confirmation Workflow

### Overview
Allows users to request a new confirmation email if they haven't received or lost the original.

### 1. GET /auth/resend-confirmation - Display Resend Form

**User Action:** User navigates to resend confirmation page
**System Response:**
- Returns `ResendConfirmation.cshtml` view
- Shows form with Email field
- Includes link back to login

---

### 2. POST /auth/resend-confirmation - Process Resend Request

#### Scenario 2.1: Successful Resend for Unconfirmed User

**User Action:** User submits valid email for unconfirmed account
**Preconditions:**
- Valid email format
- User exists in system
- User email is NOT confirmed

**System Process:**
1. Validates model state
2. Finds user by email
3. Checks confirmation status
4. Generates new confirmation token
5. Sends new confirmation email
6. Redirects to confirmation page

**User Experience:**
- Redirected to `/auth/register-confirmation`
- Sees standard confirmation message
- New email sent with fresh token

**Logging:** `"Confirmation email resent to user: {Email}, {UserId}"`

---

#### Scenario 2.2: Resend for Non-existent User

**User Action:** User submits email that doesn't exist in system
**Preconditions:** Email format valid but no user with that email

**System Process:**
1. Validates model state
2. Attempts to find user by email
3. User not found
4. Logs warning but doesn't reveal user doesn't exist
5. Redirects to confirmation page (for security)

**User Experience:**
- Redirected to `/auth/register-confirmation`
- Sees standard confirmation message (no indication user doesn't exist)

**Logging:** `"Resend confirmation attempted for non-existent user: {Email}"`

---

#### Scenario 2.3: Resend for Already Confirmed User

**User Action:** User submits email for account that's already confirmed
**Preconditions:**
- User exists in system
- User email IS already confirmed

**System Process:**
1. Validates model state
2. Finds user by email
3. Checks confirmation status - already confirmed
4. Redirects to login with informational message

**User Experience:**
- Redirected to `/auth/login?isAlreadyRegisteredAndConfirmed=true`
- Sees message: "You already have registered and confirmed account. Please log in or reset your password."

**Logging:** `"Resend confirmation attempted for already confirmed user: {Email}, {UserId}"`

---

#### Scenario 2.4: Resend Email Sending Failure

**User Action:** User submits valid resend request but email fails to send
**Preconditions:** Valid unconfirmed user, but email service throws exception

**System Process:**
1. Finds user and validates not confirmed
2. Generates confirmation token successfully
3. Email service throws exception
4. Catches exception and adds model error
5. Returns to resend form

**User Experience:**
- Stays on resend confirmation page
- Sees error: "Failed to send confirmation email. Please try again later or contact support."

**Logging:** `"Failed to resend confirmation email to user: {Email}, {UserId}"`

---

## Login Workflow

### Overview
Allows confirmed users to authenticate and access the application.

### 1. GET /auth/login - Display Login Form

**User Action:** User navigates to login page
**URL Parameters:** Optional `returnUrl`, `isAlreadyRegisteredAndConfirmed`

**System Response:**
- Returns `Login.cshtml` view
- Shows form with Email, Password, RememberMe fields
- If `isAlreadyRegisteredAndConfirmed=true`, shows message about confirmed account
- Includes links to registration and forgot password

---

### 2. POST /auth/login - Process Login

#### Scenario 2.1: Successful Login

**User Action:** User submits valid credentials for confirmed account
**Preconditions:**
- Valid email and password
- User exists and is confirmed
- Account not locked out

**System Process:**
1. Validates model state
2. Calls `SignInManager.PasswordSignInAsync()`
3. Authentication succeeds
4. Signs user in with specified persistence
5. Redirects to return URL or home

**User Experience:**
- Redirected to intended destination or home page
- Signed in and can access protected resources
- If "Remember Me" checked, stays signed in longer

**Logging:** `"User logged in: {Email}"`

---

#### Scenario 2.2: Login with Unconfirmed Email (Valid Password)

**User Action:** User tries to log in with correct credentials but unconfirmed email
**Preconditions:**
- Valid email and correct password
- User exists but email not confirmed

**System Process:**
1. Validates model state
2. `PasswordSignInAsync()` returns `IsNotAllowed`
3. Finds user by email (user exists)
4. Validates password is correct using `CheckPasswordAsync()`
5. Confirms email is not verified using `IsEmailConfirmedAsync()`
6. Redirects to confirmation page

**User Experience:**
- Redirected to `/auth/register-confirmation?isAlreadyRegisteredButNotConfirmed=true`
- Sees message about needing to confirm email
- Can resend confirmation from that page

**Logging:** `"Login attempt with unconfirmed email: {Email}, {UserId}"`

---

#### Scenario 2.3: Login with Non-existent User

**User Action:** User tries to log in with email that doesn't exist in system
**Preconditions:** Email doesn't exist in database

**System Process:**
1. Validates model state
2. `PasswordSignInAsync()` returns `IsNotAllowed`
3. Attempts to find user by email (user is null)
4. Adds generic error to model state
5. Returns to login form

**User Experience:**
- Stays on login page
- Sees error: "Login failed. Please check your email and password."
- Form retains email but clears password

**Logging:** `"Login attempt with non-existent user: {Email}"`

---

#### Scenario 2.4: Login with Unconfirmed Email and Invalid Password

**User Action:** User tries to log in with unconfirmed email and incorrect password
**Preconditions:**
- Email exists in system but not confirmed
- Password is incorrect

**System Process:**
1. Validates model state
2. `PasswordSignInAsync()` returns `IsNotAllowed`
3. Finds user by email (user exists)
4. Validates password using `CheckPasswordAsync()` (password is incorrect)
5. Adds generic error to model state
6. Returns to login form

**User Experience:**
- Stays on login page
- Sees error: "Login failed. Please check your email and password."
- Form retains email but clears password

**Logging:** `"Login attempt with unconfirmed email and invalid password: {Email}, {UserId}"`

---

#### Scenario 2.5: Invalid Credentials (General)

**User Action:** User submits incorrect email or password (for confirmed accounts)
**Preconditions:** Invalid email/password combination for confirmed accounts

**System Process:**
1. Validates model state
2. `PasswordSignInAsync()` fails (not IsNotAllowed)
3. Adds generic error to model state
4. Returns to login form

**User Experience:**
- Stays on login page
- Sees error: "Login failed. Please check your email and password."
- Form retains email but clears password

**Logging:** `"Invalid login attempt for user {Email}."`

---

#### Scenario 2.6: Model Validation Failure

**User Action:** User submits form with invalid data (missing email, etc.)
**Preconditions:** Form data fails model validation

**System Process:**
1. Model validation fails
2. Returns to login form with validation errors

**User Experience:**
- Stays on login page
- Sees specific validation errors for each field
- Form retains valid data

**Logging:** No specific logging for validation failures

---

## Logout Workflow

### Overview
Allows authenticated users to sign out of the application.

### POST /auth/logout - Process Logout

**User Action:** User clicks logout (typically from navigation)
**Preconditions:** User is currently authenticated

**System Process:**
1. Calls `SignInManager.SignOutAsync()`
2. Clears authentication cookies
3. Redirects to home page

**User Experience:**
- Signed out of application
- Redirected to home page
- Can no longer access protected resources

**Logging:** `"User logged out."`

---

## Forgot/Reset Password Workflow

### Overview
Allows users to reset their passwords via email when they've forgotten them. This is a secure two-step process involving email verification.

### 1. GET /auth/forgot-password - Display Forgot Password Form

**User Action:** User navigates to forgot password page (typically from login page)
**System Response:**
- Returns `ForgotPassword.cshtml` view
- Shows form with Email field
- Includes link back to login

---

### 2. POST /auth/forgot-password - Process Forgot Password Request

#### Scenario 2.1: Successful Password Reset Request for Confirmed User

**User Action:** User submits valid email for confirmed account
**Preconditions:**
- Valid email format
- User exists in system
- User email IS confirmed

**System Process:**
1. Validates model state
2. Finds user by email
3. Checks if email is confirmed (`IsEmailConfirmedAsync`)
4. Generates password reset token (`GeneratePasswordResetTokenAsync`)
5. Creates reset link with userId and token
6. Sends password reset email via `EmailService`
7. Redirects to confirmation page

**User Experience:**
- Redirected to `/auth/forgot-password-confirmation`
- Sees message about checking email for reset instructions
- Email sent with password reset link

**Logging:** `"Password reset email sent to user: {Email}, {UserId}"`

---

#### Scenario 2.2: Password Reset Request for Non-existent User

**User Action:** User submits email that doesn't exist in system
**Preconditions:** Email format valid but no user with that email

**System Process:**
1. Validates model state
2. Attempts to find user by email (user is null)
3. Logs warning but doesn't reveal user doesn't exist
4. Redirects to confirmation page (for security)

**User Experience:**
- Redirected to `/auth/forgot-password-confirmation`
- Sees standard confirmation message (no indication user doesn't exist)
- No email sent

**Logging:** `"Password reset attempted for non-existent user: {Email}"`

---

#### Scenario 2.3: Password Reset Request for Unconfirmed User

**User Action:** User submits email for account that exists but isn't confirmed
**Preconditions:**
- User exists in system
- User email is NOT confirmed

**System Process:**
1. Validates model state
2. Finds user by email
3. Checks confirmation status - not confirmed
4. Logs warning
5. Redirects to confirmation page (for security)

**User Experience:**
- Redirected to `/auth/forgot-password-confirmation`
- Sees standard confirmation message
- No password reset email sent

**Logging:** `"Password reset attempted for unconfirmed user: {Email}"`

---

#### Scenario 2.4: Password Reset Email Sending Failure

**User Action:** User submits valid reset request but email fails to send
**Preconditions:** Valid confirmed user, but email service throws exception

**System Process:**
1. Finds user and validates confirmed
2. Generates reset token successfully
3. Email service throws exception
4. Catches exception and logs error
5. Still redirects to confirmation page (for security)

**User Experience:**
- Redirected to `/auth/forgot-password-confirmation`
- Sees standard confirmation message
- No email actually sent due to service failure

**Logging:** `"Failed to send password reset email to user: {Email}, {UserId}"`

---

### 3. GET /auth/forgot-password-confirmation - Display Confirmation Page

**User Action:** User is redirected here after submitting forgot password form
**System Response:**
- Returns `ForgotPasswordConfirmation.cshtml` view
- Shows message about checking email
- Includes link to return to login

---

### 4. GET /auth/reset-password - Display Reset Password Form

#### Scenario 4.1: Valid Reset Link Access

**User Action:** User clicks reset link from email
**URL Parameters:** `userId` and `token`
**Preconditions:**
- Valid userId and token in URL
- User exists in system

**System Process:**
1. Validates userId and token are provided
2. Finds user by userId
3. Returns reset password form

**User Experience:**
- Sees `ResetPassword.cshtml` with password fields
- Form includes hidden fields for userId and token
- Password toggle functionality available

---

#### Scenario 4.2: Invalid Reset Link Parameters

**User Action:** User clicks malformed reset link or missing parameters
**Preconditions:** Missing or empty `userId` or `token` parameters

**System Process:**
1. Validates parameters
2. Logs warning about missing parameters
3. Redirects to error page

**User Experience:**
- Redirected to `/Home/Error`
- Sees generic error page

**Logging:** `"Password reset attempted with missing userId or token"`

---

#### Scenario 4.3: Reset Link with Non-existent User

**User Action:** User clicks reset link with invalid userId
**Preconditions:** Valid format parameters but userId doesn't exist

**System Process:**
1. Attempts to find user by userId
2. User not found
3. Logs warning
4. Redirects to error page

**User Experience:**
- Redirected to `/Home/Error`
- Sees generic error page

**Logging:** `"Password reset attempted for non-existent user: {UserId}"`

---

### 5. POST /auth/reset-password - Process Password Reset

#### Scenario 5.1: Successful Password Reset

**User Action:** User submits valid new password
**Preconditions:**
- Valid password format (meets requirements)
- Passwords match
- Valid userId and token
- Token not expired

**System Process:**
1. Validates model state
2. Finds user by userId
3. Verifies reset token using `VerifyUserTokenAsync`
4. Resets password using `ResetPasswordAsync`
5. Redirects to success page

**User Experience:**
- Redirected to `/auth/reset-password-confirmation`
- Sees success message
- Can now log in with new password

**Logging:** `"Password reset successful for user: {Email}, {UserId}"`

---

#### Scenario 5.2: Password Reset with Invalid/Expired Token

**User Action:** User submits form with expired or invalid token
**Preconditions:** Valid user but token verification fails

**System Process:**
1. Validates model state
2. Finds user successfully
3. Token verification fails (`VerifyUserTokenAsync` returns false)
4. Adds error to model state
5. Returns to reset form

**User Experience:**
- Stays on reset password page
- Sees error: "Password reset link may have expired or is invalid. Please try again or request a new one."

**Logging:** `"Password reset token verification failed for user: {Email}, {UserId}, {RequestUserId}"`

---

#### Scenario 5.3: Password Reset with Non-existent User

**User Action:** User submits form but userId doesn't exist
**Preconditions:** Form submitted but user lookup fails

**System Process:**
1. Validates model state
2. Attempts to find user by userId (user is null)
3. Logs warning
4. Adds error to model state
5. Returns to reset form

**User Experience:**
- Stays on reset password page
- Sees error: "Invalid password reset request."

**Logging:** `"Password reset attempted for non-existent user: {UserId}"`

---

#### Scenario 5.4: Password Reset Failure - Password Requirements

**User Action:** User submits form but password reset fails due to policy
**Preconditions:** Valid token and user, but `ResetPasswordAsync` fails

**System Process:**
1. Validates model state and token successfully
2. Password reset fails (password policy, etc.)
3. Logs detailed error information
4. Adds error to model state
5. Returns to reset form

**User Experience:**
- Stays on reset password page
- Sees error: "Password reset link may have expired or is invalid. Please try again or request a new one."

**Logging:** `"Password reset failed for user {Email}: {Errors}"`

---

### 6. GET /auth/reset-password-confirmation - Display Success Page

**User Action:** User is redirected here after successful password reset
**System Response:**
- Returns `ResetPasswordConfirmation.cshtml` view
- Shows success message
- Includes link to login page

---

## Error Handling and Security Notes

### General Error Handling
- Generic error messages shown to users to prevent information disclosure
- Detailed errors logged for debugging
- Failed operations redirect to safe pages

### Security Considerations
- User existence not revealed in most error scenarios
- Confirmation tokens are cryptographically secure
- Password reset tokens are cryptographically secure and time-limited
- Email addresses used as usernames for simplicity
- Automatic signin after email confirmation for better UX
- Password reset requests always show success message to prevent email enumeration
- Reset tokens are verified before allowing password changes

### Logging Strategy
- All authentication events logged with relevant identifiers
- Sensitive information (passwords, tokens) never logged
- Warning level for suspicious activities
- Error level for system failures

---