using Microsoft.Playwright;

namespace CareerPilotAi.E2ETests.PageObjects;

public class LoginPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public LoginPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    // Locators using data-test-id attributes
    private ILocator EmailInput => _page.Locator("[data-test-id='email-input']");
    private ILocator PasswordInput => _page.Locator("[data-test-id='password-input']");
    private ILocator LoginButton => _page.Locator("[data-test-id='login-button']");
    private ILocator ProfileLink => _page.Locator("[data-test-id='profile-link']");
    private ILocator LogoutButton => _page.Locator("[data-test-id='logout-button']");
    private ILocator RememberMeCheckbox => _page.Locator("[data-test-id='remember-me-checkbox']");
    private ILocator ValidationSummary => _page.Locator("[data-test-id='validation-summary']");
    private ILocator ForgotPasswordLink => _page.Locator("[data-test-id='forgot-password-link']");
    private ILocator RegisterFormLink => _page.Locator("[data-test-id='register-form-link']");
    private ILocator EmailValidation => _page.Locator("[data-test-id='email-validation']");
    private ILocator PasswordValidation => _page.Locator("[data-test-id='password-validation']");

    public async Task NavigateAsync(string? returnUrl = null)
    {
        var url = returnUrl == null 
            ? $"{_baseUrl}/auth/login" 
            : $"{_baseUrl}/auth/login?returnUrl={Uri.EscapeDataString(returnUrl)}";
        await _page.GotoAsync(url);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task FillEmailAsync(string email)
    {
        await EmailInput.FillAsync(email);
    }

    public async Task FillPasswordAsync(string password)
    {
        await PasswordInput.FillAsync(password);
    }

    public async Task SubmitAsync()
    {
        await LoginButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task LoginAsync(string email, string password)
    {
        await FillEmailAsync(email);
        await FillPasswordAsync(password);
        await SubmitAsync();
    }

    public async Task<bool> IsUserLoggedInAsync()
    {
        try
        {
            return await ProfileLink.IsVisibleAsync();
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        // Click on profile dropdown
        await ProfileLink.ClickAsync();
        // Wait for dropdown to appear
        await _page.WaitForTimeoutAsync(500);
        // Click logout button
        await LogoutButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task ToggleRememberMeAsync(bool value)
    {
        var isChecked = await RememberMeCheckbox.IsCheckedAsync();
        if (isChecked != value)
        {
            await RememberMeCheckbox.ClickAsync();
        }
    }

    public async Task ClickForgotPasswordAsync()
    {
        await ForgotPasswordLink.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task ClickRegisterAsync()
    {
        await RegisterFormLink.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<string> GetValidationSummaryTextAsync()
    {
        try
        {
            return await ValidationSummary.TextContentAsync() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<bool> HasAnyValidationSummaryAsync()
    {
        try
        {
            return await ValidationSummary.IsVisibleAsync();
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> HasEmailValidationErrorAsync()
    {
        try
        {
            var text = await EmailValidation.TextContentAsync();
            return !string.IsNullOrWhiteSpace(text);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> HasPasswordValidationErrorAsync()
    {
        try
        {
            var text = await PasswordValidation.TextContentAsync();
            return !string.IsNullOrWhiteSpace(text);
        }
        catch
        {
            return false;
        }
    }

    public string GetCurrentUrl()
    {
        return _page.Url;
    }

    public async Task<BrowserContextCookiesResult?> GetCareerPilotAuthCookieAsync()
    {
        var cookies = await _page.Context.CookiesAsync();
        return cookies.FirstOrDefault(c => c.Name == "CareerPilotAuth");
    }
}