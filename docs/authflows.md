# CareerPilotAi Authentication Workflows

This document provides detailed documentation of all authentication workflows in the CareerPilotAi application, including user journeys, system responses, and error handling scenarios.

## Table of Contents

1. [Registration Workflow](#registration-workflow)
2. [Email Confirmation Workflow](#email-confirmation-workflow)
3. [Resend Confirmation Workflow](#resend-confirmation-workflow)
4. [Login Workflow](#login-workflow)
5. [Logout Workflow](#logout-workflow)

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

## Error Handling and Security Notes

### General Error Handling
- Generic error messages shown to users to prevent information disclosure
- Detailed errors logged for debugging
- Failed operations redirect to safe pages

### Security Considerations
- User existence not revealed in most error scenarios
- Confirmation tokens are cryptographically secure
- Email addresses used as usernames for simplicity
- Automatic signin after email confirmation for better UX

### Logging Strategy
- All authentication events logged with relevant identifiers
- Sensitive information (passwords, tokens) never logged
- Warning level for suspicious activities
- Error level for system failures

---