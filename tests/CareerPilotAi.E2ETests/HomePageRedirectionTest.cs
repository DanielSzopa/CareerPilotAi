using Microsoft.Playwright;
using Shouldly;

namespace CareerPilotAi.E2ETests;

/// <summary>
/// First E2E test to verify that the application is running and accessible
/// </summary>
[Collection("E2E collection")]
public class HomePageRedirectionTest
{
    private readonly E2ETestFixture _fixture;

    public HomePageRedirectionTest(E2ETestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task HomePage_RedirectsCorrectly_AndDisplaysExpectedContent()
    {
        // Arrange - Create a new page from the browser
        var page = await _fixture.Browser.NewPageAsync();

        try
        {
            // Act - Navigate to the home page
            await page.GotoAsync(_fixture.BaseUrl);

            // Assert - Check that we landed on the correct page
            var title = await page.TitleAsync();
            title.ShouldContain("CareerPilotAi", Case.Insensitive);

            // Additional assertion - verify we're on the home page URL
            var url = page.Url;
            url.ShouldStartWith(_fixture.BaseUrl);
        }
        finally
        {
            // Clean up - close the page
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task HomePage_ReturnsSuccessStatusCode()
    {
        // Arrange
        var page = await _fixture.Browser.NewPageAsync();

        try
        {
            // Act
            var response = await page.GotoAsync(_fixture.BaseUrl);

            // Assert
            response.ShouldNotBeNull();
            response.Ok.ShouldBeTrue($"Expected successful response but got status {response.Status}");
        }
        finally
        {
            await page.CloseAsync();
        }
    }
}

