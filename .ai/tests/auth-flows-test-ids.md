# Proponowane atrybuty `data-test-id` dla widoków autoryzacji

Ten dokument zawiera proponowane atrybuty `data-test-id` dla każdego widoku w procesie uwierzytelniania i rejestracji, aby ułatwić testowanie E2E za pomocą Playwright.

---

## `Views/Shared/_LoginPartial.cshtml` (Navigation Authentication Elements)

### Dla zalogowanych użytkowników:
- **Link "My Profile" (dropdown)**: `profile-link`
- **Link "Settings" w dropdown**: `settings-link`
- **Przycisk "Logout" w dropdown**: `logout-button`

### Dla niezalogowanych użytkowników:
- **Link "Log in"**: `login-link`
- **Przycisk "Sign up"**: `register-link`

---

## `Views/Auth/Login.cshtml`

-   **Formularz**: `login-form`
-   **Pole Email**: `email-input`
-   **Walidacja Email**: `email-validation`
-   **Pole Hasło**: `password-input`
-   **Przycisk Pokaż/Ukryj Hasło**: `password-toggle-button`
-   **Walidacja Hasła**: `password-validation`
-   **Checkbox "Zapamiętaj mnie"**: `remember-me-checkbox`
-   **Przycisk "Continue" (Zaloguj)**: `login-button`
-   **Link "Forgot password?"**: `forgot-password-link`
-   **Link "Sign up"**: `register-form-link`
-   **Podsumowanie walidacji**: `validation-summary`
-   **Banner z komunikatem**: `login-message-banner` (dla komunikatów, np. o już istniejącym koncie)

---

## `Views/Auth/Register.cshtml`

-   **Formularz**: `register-form`
-   **Pole Email**: `email-input`
-   **Walidacja Email**: `email-validation`
-   **Pole Hasło**: `password-input`
-   **Przycisk Pokaż/Ukryj Hasło**: `password-toggle-button`
-   **Walidacja Hasła**: `password-validation`
-   **Pole "Confirm Password"**: `confirm-password-input`
-   **Przycisk Pokaż/Ukryj "Confirm Password"**: `confirm-password-toggle-button`
-   **Walidacja Confirm Password**: `confirm-password-validation`
-   **Przycisk "Continue" (Zarejestruj)**: `register-button`
-   **Link "Log in"**: `login-link`
-   **Podsumowanie walidacji**: `validation-summary`

---

## `Views/Auth/RegisterConfirmation.cshtml`

-   **Tytuł**: `register-confirmation-title`
-   **Banner informacyjny**: `info-isAlreadyRegisteredButNotConfirmed-banner` (dla `isAlreadyRegisteredButNotConfirmed`)
-   **Link "resend confirmation email"**: `resend-confirmation-link`
-   **Przycisk "Return to Login"**: `return-to-login-button`

---

## `Views/Auth/ResendConfirmation.cshtml`

-   **Formularz**: `resend-confirmation-form`
-   **Pole Email**: `email-input`
-   **Walidacja Email**: `email-validation`
-   **Przycisk "Resend Confirmation Email"**: `resend-confirmation-button`
-   **Link "Back to Login"**: `back-to-login-link`
-   **Podsumowanie walidacji**: `validation-summary`

---

## `Views/Auth/ForgotPassword.cshtml`

-   **Formularz**: `forgot-password-form`
-   **Pole Email**: `email-input`
-   **Walidacja Email**: `email-validation`
-   **Przycisk "Continue"**: `continue-button`
-   **Link "Back to Login"**: `back-to-login-link`
-   **Podsumowanie walidacji**: `validation-summary`

---

## `Views/Auth/ForgotPasswordConfirmation.cshtml`

-   **Tytuł**: `forgot-password-confirmation-title`
-   **Komunikat o wysłaniu instrukcji**: `confirmation-message`
-   **Przycisk "Return to Login"**: `return-to-login-button`

---

## `Views/Auth/ResetPassword.cshtml`

-   **Formularz**: `reset-password-form`
-   **Pole "New password"**: `password-input`
-   **Przycisk Pokaż/Ukryj Hasło**: `password-toggle-button`
-   **Walidacja Hasła**: `password-validation`
-   **Pole "Confirm new password"**: `confirm-password-input`
-   **Przycisk Pokaż/Ukryj "Confirm new password"**: `confirm-password-toggle-button`
-   **Walidacja Confirm Password**: `confirm-password-validation`
-   **Przycisk "Reset Password"**: `reset-password-button`
-   **Link "Back to Login"**: `back-to-login-link`
-   **Podsumowanie walidacji**: `validation-summary`

---

## `Views/Auth/ResetPasswordConfirmation.cshtml`

-   **Tytuł**: `reset-password-confirmation-title`
-   **Przycisk "Continue to Login"**: `continue-to-login-button`

---

## `Views/Auth/LogoutConfirmation.cshtml`

-   **Tytuł**: `logout-confirmation-title`
-   **Przycisk "Log Back In"**: `log-back-in-button`
-   **Przycisk "Go to Homepage"**: `go-to-homepage-button`
-   **Link "View your job applications"**: `view-job-applications-link`
-   **Link "start a new application"**: `new-application-link`

---

## `Views/Auth/AccessDenied.cshtml`

-   **Tytuł**: `access-denied-title`
-   **Komunikat o braku dostępu**: `access-denied-message`
-   **Przycisk "Return to Home Page"**: `return-to-home-button`
-   **Przycisk "Go to Sign In"**: `go-to-signin-button`

---

## `Views/Auth/UserSettings.cshtml`

-   **Formularz**: `user-settings-form`
-   **Tytuł**: `user-settings-title`
-   **Banner o sukcesie**: `success-message-banner`
-   **Pole Email (tylko do odczytu)**: `email-input`
-   **Lista wyboru strefy czasowej**: `timezone-select`
-   **Walidacja strefy czasowej**: `timezone-validation`
-   **Przycisk "Save"**: `save-button`
-   **Podsumowanie walidacji**: `validation-summary`

---

## Dodatkowe funkcjonalności dla testów E2E

### Walidacja pól formularzy

Wszystkie pola formularzy mają dedykowane identyfikatory dla błędów walidacji:

-   **Walidacja Email**: `[fieldname]-validation` (np. `email-validation`)
-   **Walidacja Hasła**: `password-validation`
-   **Walidacja Confirm Password**: `confirm-password-validation`
-   **Walidacja innych pól**: `[fieldname]-validation` (np. `timezone-validation`)

### Komunikaty błędów

Komunikaty błędów są generowane dynamicznie przez ASP.NET MVC i wyświetlane w:

-   **Podsumowaniu walidacji**: `validation-summary` (dla błędów ogólnych)
-   **Specyficznych polach**: `[fieldname]-validation` (dla błędów pól)