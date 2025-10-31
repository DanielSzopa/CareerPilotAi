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

    public async Task NavigateAsync()
    {
        await _page.GotoAsync($"{_baseUrl}/auth/login");
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

    public string GetCurrentUrl()
    {
        return _page.Url;
    }
}