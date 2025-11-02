using Bogus;
using CareerPilotAi.E2ETests.PageObjects;
using Shouldly;

namespace CareerPilotAi.E2ETests;

[Collection("E2E collection")]
public class LoginTests
{
    private const string TestUserEmail = "test@example.com";
    private const string TestUserPassword = "Test1234!@#";

    private readonly E2ETestFixture _fixture;

    public LoginTests(E2ETestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Login_Succeeds_DefaultRedirect_WithRememberMe()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        // Act
        await login.ToggleRememberMeAsync(true);
        await login.LoginAsync(TestUserEmail, TestUserPassword);

        // Assert
        page.Url.ShouldEndWith("/job-applications");
        (await login.IsUserLoggedInAsync()).ShouldBeTrue();

        var authCookie = await login.GetCareerPilotAuthCookieAsync();
        authCookie.ShouldNotBeNull();
        authCookie.Expires.ShouldBeGreaterThan(0);

        await context.CloseAsync();
    }

    [Fact]
    public async Task Login_Succeeds_DefaultRedirect_WithoutRememberMe()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        // Act
        await login.LoginAsync(TestUserEmail, TestUserPassword);

        // Assert
        page.Url.ShouldEndWith("/job-applications");
        (await login.IsUserLoggedInAsync()).ShouldBeTrue();

        var authCookie = await login.GetCareerPilotAuthCookieAsync();
        authCookie.ShouldNotBeNull();
        authCookie.Expires.ShouldBe(-1);

        await context.CloseAsync();
    }

    [Fact]
    public async Task Login_Succeeds_WithSafeLocalReturnUrl()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        const string returnUrl = "/job-applications/create";
        await login.NavigateAsync(returnUrl);

        // Act
        await login.LoginAsync(TestUserEmail, TestUserPassword);

        // Assert
        page.Url.ShouldEndWith(returnUrl);
        (await login.IsUserLoggedInAsync()).ShouldBeTrue();

        await context.CloseAsync();
    }

    [Fact]
    public async Task Login_Fails_InvalidPassword_ShowsError()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        // Act
        await login.LoginAsync(TestUserEmail, "InvalidPassword");

        // Assert
        page.Url.ShouldContain("/auth/login");
        var validationText = await login.GetValidationSummaryTextAsync();
        validationText.ShouldNotBeNullOrEmpty();
        (await login.IsUserLoggedInAsync()).ShouldBeFalse();

        await context.CloseAsync();
    }

    [Fact]
    public async Task Login_Fails_EmptyPassword_ShowsError()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        // Act
        await login.LoginAsync(TestUserEmail, string.Empty);

        // Assert
        page.Url.ShouldContain("/auth/login");
        var validationText = await login.GetValidationSummaryTextAsync();
        validationText.ShouldNotBeNullOrEmpty();
        (await login.HasPasswordValidationErrorAsync()).ShouldBeTrue();
        (await login.IsUserLoggedInAsync()).ShouldBeFalse();

        await context.CloseAsync();
    }


    [Fact]
    public async Task Login_Fails_NonExistentUser_ShowsGenericError()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        var faker = new Faker();
        var nonExistentEmail = faker.Internet.Email();

        // Act
        await login.LoginAsync(nonExistentEmail, TestUserPassword);

        // Assert
        page.Url.ShouldContain("/auth/login");
        var validationText = await login.GetValidationSummaryTextAsync();
        validationText.ShouldContain("Login failed");
        validationText.ShouldContain("Please check your email and password");
        (await login.IsUserLoggedInAsync()).ShouldBeFalse();

        await context.CloseAsync();
    }

    [Theory]
    [InlineData("EmptyEmail", "", TestUserPassword)]
    [InlineData("EmptyPassword", TestUserEmail, "")]
    [InlineData("EmptyBoth", "", "")]
    public async Task Login_ValidationError_EmptyFields(string scenario, string email, string password)
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        // Act
        await login.FillEmailAsync(email);
        await login.FillPasswordAsync(password);
        await login.SubmitAsync();

        // Assert
        page.Url.ShouldContain("/auth/login");
        (await login.IsUserLoggedInAsync()).ShouldBeFalse();

        switch (scenario)
        {
            case "EmptyEmail":
                (await login.HasEmailValidationErrorAsync()).ShouldBeTrue();
                break;
            case "EmptyPassword":
                (await login.HasPasswordValidationErrorAsync()).ShouldBeTrue();
                break;
            case "EmptyBoth":
                (await login.HasEmailValidationErrorAsync()).ShouldBeTrue();
                (await login.HasPasswordValidationErrorAsync()).ShouldBeTrue();
                break;
        }

        await context.CloseAsync();
    }

    [Fact]
    public async Task Login_ForgotPasswordLink_WorksCorrectly()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        // Act
        await login.ClickForgotPasswordAsync();

        // Assert
        page.Url.ShouldContain("/auth/forgot-password");
        (await login.IsUserLoggedInAsync()).ShouldBeFalse();

        await context.CloseAsync();
    }

    [Fact]
    public async Task Login_SignUpLink_WorksCorrectly()
    {
        // Arrange
        var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var login = new LoginPage(page, _fixture.BaseUrl);
        await login.NavigateAsync();

        // Act
        await login.ClickRegisterAsync();

        // Assert
        page.Url.ShouldContain("/auth/register");
        (await login.IsUserLoggedInAsync()).ShouldBeFalse();

        await context.CloseAsync();
    }
}
