using Microsoft.Playwright;

namespace CareerPilotAi.E2ETests.PageObjects;

public class RegisterPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public RegisterPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    private ILocator EmailInput => _page.Locator("[data-test-id='email-input']");
    private ILocator PasswordInput => _page.Locator("[data-test-id='password-input']");
    private ILocator ConfirmPasswordInput => _page.Locator("[data-test-id='confirm-password-input']");
    private ILocator RegisterButton => _page.Locator("[data-test-id='register-button']");
    private ILocator EmailValidation => _page.Locator("[data-test-id='email-validation']");
    private ILocator PasswordValidation => _page.Locator("[data-test-id='password-validation']");
    private ILocator ConfirmPasswordValidation => _page.Locator("[data-test-id='confirm-password-validation']");
    private ILocator ValidationSummary => _page.Locator("[data-test-id='validation-summary']");
    private ILocator ProfileLink => _page.Locator("[data-test-id='profile-link']");
    private ILocator LoginLink => _page.Locator("[data-test-id='login-link']");

    public async Task NavigateAsync()
    {
        await _page.GotoAsync($"{_baseUrl}/auth/register");
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

    public async Task FillConfirmPasswordAsync(string confirmPassword)
    {
        await ConfirmPasswordInput.FillAsync(confirmPassword);
    }

    public async Task SubmitAsync()
    {
        await RegisterButton.ClickAsync();
        // Wait for navigation or error messages to appear
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task RegisterAsync(string email, string password, string confirmPassword)
    {
        await FillEmailAsync(email);
        await FillPasswordAsync(password);
        await FillConfirmPasswordAsync(confirmPassword);
        await SubmitAsync();
    }

    public async Task<string> GetValidationSummaryErrorAsync()
    {
        if (await ValidationSummary.IsVisibleAsync())
        {
            return await ValidationSummary.TextContentAsync() ?? string.Empty;
        }
        return string.Empty;
    }

    public async Task<bool> HasAnyValidationErrorSummaryAsync()
    {
        return await ValidationSummary.IsVisibleAsync();
    }

    public async Task<bool> HasAnyErrorAsync()
    {
        var hasEmailError = await EmailValidation.IsVisibleAsync();
        var hasPasswordError = await PasswordValidation.IsVisibleAsync();
        var hasConfirmPasswordError = await ConfirmPasswordValidation.IsVisibleAsync();
        var hasValidationSummaryError = await ValidationSummary.IsVisibleAsync();

        return hasEmailError || hasPasswordError || hasConfirmPasswordError || hasValidationSummaryError;
    }

    public async Task<bool> HasAnyEmailErrorAsync(){
        return await EmailValidation.IsVisibleAsync();
    }

    public async Task<bool> HasAnyPasswordErrorAsync(){
        return await PasswordValidation.IsVisibleAsync();
    }

    public async Task<bool> HasAnyConfirmPasswordErrorAsync(){
        return await ConfirmPasswordValidation.IsVisibleAsync();
    }

    public async Task<bool> HasAnyValidationSummaryErrorAsync(){
        return await ValidationSummary.IsVisibleAsync();
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

    public async Task<bool> IsLoginLinkVisibleAsync()
    {
        try
        {
            return await LoginLink.IsVisibleAsync();
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
}