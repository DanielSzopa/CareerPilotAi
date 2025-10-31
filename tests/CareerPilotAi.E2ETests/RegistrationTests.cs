using Bogus;
using CareerPilotAi.E2ETests.PageObjects;
using Microsoft.Playwright;
using Shouldly;

namespace CareerPilotAi.E2ETests;

[Collection("E2E collection")]
public class RegistrationTests
{
    private readonly E2ETestFixture _fixture;
    private readonly Faker _faker = new Faker();

    public RegistrationTests(E2ETestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Positive Tests - Successful Registration

    [Theory]
    [InlineData("newuser@example.com", "Test1234!@#", "Test1234!@#", "Basic successful registration")]
    [InlineData("user+test123@example.com", "Test1234!@#", "Test1234!@#", "Email with special characters")]
    [InlineData("minpass@example.com", "Abcd123!", "Abcd123!", "Minimum password requirements (8 chars)")]
    [InlineData("MixedCase@Example.COM", "Test1234!@#", "Test1234!@#", "Mixed case email")]
    public async Task SuccessfulRegistration_WithValidInputs_ShouldRegisterAndLoginUser(
        string email, 
        string password, 
        string confirmPassword, 
        string testDescription)
    {
        // Arrange
        var page = await _fixture.Browser.NewPageAsync();
        var registerPage = new RegisterPage(page, _fixture.BaseUrl);

        try
        {
            await registerPage.NavigateAsync();

            // Act
            await registerPage.RegisterAsync(email, password, confirmPassword);

            // Assert - User should be automatically logged in
            var isLoggedIn = await registerPage.IsUserLoggedInAsync();
            isLoggedIn.ShouldBeTrue($"User should be logged in after successful registration. Test: {testDescription}");

            // Assert - Should redirect to /job-applications
            var currentUrl = registerPage.GetCurrentUrl().ToLowerInvariant();
            currentUrl.ShouldContain("/job-applications", Case.Insensitive, 
                $"Should redirect to job applications page. Test: {testDescription}");

            // Assert - No validation errors should be displayed
            var hasErrors = await registerPage.HasAnyErrorAsync();
            hasErrors.ShouldBeFalse($"No validation errors should be displayed. Test: {testDescription}");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    #endregion

    #region Negative Tests - Invalid Email Validation

    [Theory]
    [InlineData("notanemail", "No @ symbol")]
    [InlineData("user@@example.com", "Multiple @ symbols")]
    [InlineData("user@example", "No domain extension")]
    [InlineData("a12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234@example.com", "Email exceeding 254 characters")]
    [InlineData("", "Empty email field")]
    [InlineData("user name@example.com", "Email with spaces")]
    public async Task Registration_WithInvalidEmail_ShouldShowValidationError(string email, string testDescription)
    {
        // Arrange
        var password = "Test1234!@#";
        var confirmPassword = password;
        var page = await _fixture.Browser.NewPageAsync();
        var registerPage = new RegisterPage(page, _fixture.BaseUrl);

        try
        {
            await registerPage.NavigateAsync();

            // Act
            await registerPage.RegisterAsync(email, password, confirmPassword);

            // Assert - Should have validation error
            var hasError = await registerPage.HasAnyEmailErrorAsync();
            hasError.ShouldBeTrue($"Validation error should be displayed for invalid email. Test: {testDescription}");

            // Assert - Should stay on registration page
            var currentUrl = registerPage.GetCurrentUrl().ToLowerInvariant();
            currentUrl.ShouldContain("/auth/register", Case.Insensitive, 
                $"Should stay on registration page after validation error. Test: {testDescription}");

            // Assert - User should not be logged in
            var isLoggedIn = await registerPage.IsUserLoggedInAsync();
            isLoggedIn.ShouldBeFalse($"User should not be logged in with invalid email. Test: {testDescription}");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    #endregion

    #region Negative Tests - Invalid Password Validation

    [Theory]
    [InlineData("Test1!", "Test1!", "Password too short - 6 chars")]
    [InlineData("test1234!", "test1234!", "Missing uppercase letter")]
    [InlineData("TEST1234!", "TEST1234!", "Missing lowercase letter")]
    [InlineData("TestTest!", "TestTest!", "Missing digit")]
    [InlineData("Test1234", "Test1234", "Missing special character")]
    [InlineData("", "", "Empty password field")]
    public async Task Registration_WithInvalidPassword_ShouldShowValidationError(
        string password, 
        string confirmPassword, 
        string testDescription)
    {
        // Arrange
        var email = _faker.Internet.Email();
        var page = await _fixture.Browser.NewPageAsync();
        var registerPage = new RegisterPage(page, _fixture.BaseUrl);

        try
        {
            await registerPage.NavigateAsync();

            // Act
            await registerPage.RegisterAsync(email, password, confirmPassword);

            // Assert - Should have validation error
            var hasError = await registerPage.HasAnyPasswordErrorAsync() || await registerPage.HasAnyValidationErrorSummaryAsync();
            hasError.ShouldBeTrue($"Validation error should be displayed for invalid password. Test: {testDescription}");

            // Assert - Should stay on registration page
            var currentUrl = registerPage.GetCurrentUrl().ToLowerInvariant();
            currentUrl.ShouldContain("/auth/register", Case.Insensitive, 
                $"Should stay on registration page after validation error. Test: {testDescription}");

            // Assert - User should not be logged in
            var isLoggedIn = await registerPage.IsUserLoggedInAsync();
            isLoggedIn.ShouldBeFalse($"User should not be logged in with invalid password. Test: {testDescription}");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task Registration_WithInvalidConfirmPassword_ShouldShowValidationError()
    {
        // Arrange
        var password = "Test1234!@#";
        var confirmPassword = "Different123!";
        var email = _faker.Internet.Email();
        var page = await _fixture.Browser.NewPageAsync();
        var registerPage = new RegisterPage(page, _fixture.BaseUrl);

        try
        {
            await registerPage.NavigateAsync();

            // Act
            await registerPage.RegisterAsync(email, password, confirmPassword);

            // Assert - Should have validation error
            var hasError = await registerPage.HasAnyConfirmPasswordErrorAsync();
            hasError.ShouldBeTrue("Validation error should be displayed for invalid confirm password.");

            // Assert - Should stay on registration page
            var currentUrl = registerPage.GetCurrentUrl().ToLowerInvariant();
            currentUrl.ShouldContain("/auth/register", Case.Insensitive, 
                "Should stay on registration page after validation error.");

            // Assert - User should not be logged in
            var isLoggedIn = await registerPage.IsUserLoggedInAsync();
            isLoggedIn.ShouldBeFalse("User should not be logged in with invalid password.");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    #endregion

    #region Negative Tests - Duplicate Email

    [Fact]
    public async Task Registration_WithDuplicateEmail_ShouldShowErrorAndNotLogin()
    {
        // Arrange - Register first user
        var page1 = await _fixture.Browser.NewPageAsync();
        var registerPage1 = new RegisterPage(page1, _fixture.BaseUrl);
        var email = _faker.Internet.Email();
        var testPassword = "Test1234!@#";

        try
        {
            await registerPage1.NavigateAsync();
            await registerPage1.RegisterAsync(email, testPassword, testPassword);

            // Verify first registration succeeded
            var isFirstRegistrationSuccessful = await registerPage1.IsUserLoggedInAsync();
            isFirstRegistrationSuccessful.ShouldBeTrue("First registration should succeed");

            // Act - Try to register with the same email in a new session
            var page2 = await _fixture.Browser.NewPageAsync();
            var registerPage2 = new RegisterPage(page2, _fixture.BaseUrl);

            try
            {
                await registerPage2.NavigateAsync();
                await registerPage2.RegisterAsync(email, testPassword, testPassword);

                var validationError = await registerPage2.GetValidationSummaryErrorAsync();
                validationError.ShouldContain("An account with this email already exists. Please log in or reset your password.", Case.Insensitive, 
                    "Error message should indicate email already exists");

                // Assert - Should stay on registration page
                var currentUrl = registerPage2.GetCurrentUrl().ToLowerInvariant();
                currentUrl.ShouldContain("/auth/register", Case.Insensitive, 
                    "Should stay on registration page");

                // Assert - User should not be logged in
                var isLoggedIn = await registerPage2.IsUserLoggedInAsync();
                isLoggedIn.ShouldBeFalse("User should not be logged in with duplicate email");
            }
            finally
            {
                await page2.CloseAsync();
            }
        }
        finally
        {
            await page1.CloseAsync();
        }
    }

    #endregion

    #region Edge Cases - Full Registration Cycle

    [Fact]
    public async Task FullRegistrationCycle_ShouldAllowAccessToProtectedResourcesAndRelogin()
    {
        // Arrange
        var page = await _fixture.Browser.NewPageAsync();
        var registerPage = new RegisterPage(page, _fixture.BaseUrl);
        var loginPage = new LoginPage(page, _fixture.BaseUrl);
        var email = _faker.Internet.Email();
        var testPassword = "Test1234!@#";

        try
        {
            // Act 1: Register new user
            await registerPage.NavigateAsync();
            await registerPage.RegisterAsync(email, testPassword, testPassword);

            // Assert 1: Registration successful and auto-login
            var isLoggedInAfterRegistration = await registerPage.IsUserLoggedInAsync();
            isLoggedInAfterRegistration.ShouldBeTrue("User should be logged in after registration");

            var urlAfterRegistration = registerPage.GetCurrentUrl().ToLowerInvariant();
            urlAfterRegistration.ShouldContain("/job-applications", Case.Insensitive, 
                "Should redirect to job applications after registration");

            // Act 2: Access protected resource
            await page.GotoAsync($"{_fixture.BaseUrl}/job-applications/create");
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert 2: Protected resource should be accessible (no redirect to login)
            var protectedPageUrl = page.Url.ToLowerInvariant();
            protectedPageUrl.ShouldContain("/job-applications/create", Case.Insensitive, 
                "Should be able to access protected page");
            protectedPageUrl.ShouldNotContain("/auth/login", Case.Insensitive, 
                "Should not be redirected to login page");

            // Act 3: Logout
            await loginPage.LogoutAsync();

            // Assert 3: User should be logged out
            var isLoggedInAfterLogout = await registerPage.IsLoginLinkVisibleAsync();
            isLoggedInAfterLogout.ShouldBeTrue("Login link should be visible after logout");

            // Act 4: Re-login with same credentials
            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(email, testPassword);

            // Assert 4: Re-login should be successful
            var isLoggedInAfterRelogin = await loginPage.IsUserLoggedInAsync();
            isLoggedInAfterRelogin.ShouldBeTrue("User should be able to re-login with registered credentials");

            var urlAfterRelogin = loginPage.GetCurrentUrl().ToLowerInvariant();
            urlAfterRelogin.ShouldContain("/job-applications", Case.Insensitive, 
                "Should redirect to job applications after re-login");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    #endregion
}