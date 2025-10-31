using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.Playwright;
using Testcontainers.PostgreSql;

namespace CareerPilotAi.E2ETests;

/// <summary>
/// Fixture for E2E tests that manages Docker containers and Playwright browser
/// </summary>
public class E2ETestFixture : IAsyncLifetime
{
    private INetwork? _network;
    private PostgreSqlContainer? _postgresContainer;
    private IContainer? _appContainer;
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public IBrowser Browser => _browser ?? throw new InvalidOperationException("Browser not initialized");
    public string BaseUrl { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        // Step 1: Create Docker network
        _network = new NetworkBuilder()
            .WithName($"e2e-network-{Guid.NewGuid()}")
            .Build();
        await _network.CreateAsync();

        // Step 2: Start PostgreSQL container in the network
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithNetwork(_network)
            .WithNetworkAliases("postgres-e2e")
            .WithDatabase("careerpilotai_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
        await _postgresContainer.StartAsync();

        // Step 3: Build and start the application container
        var contextPath = CommonDirectoryPath.GetSolutionDirectory().DirectoryPath;
        var dockerfilePath = "Dockerfile.e2e";

        var appImage = new ImageFromDockerfileBuilder()
                .WithDockerfileDirectory(contextPath)
                .WithDockerfile(dockerfilePath)
                .WithName($"career-pilot-ai-e2e-app")
                .Build();

        await appImage.CreateAsync();

        _appContainer = new ContainerBuilder()
            .WithNetwork(_network)
            .WithImage(appImage)
            .WithPortBinding(0, 8080) // 0 = dynamic port on host
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "e2e")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
            .Build();

        await _appContainer.StartAsync();
        
        // Step 4: Get mapped port and create BaseUrl
        var port = _appContainer.GetMappedPublicPort(8080);
        BaseUrl = $"http://localhost:{port}";

        // Step 5: Initialize Playwright and launch Chromium browser
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    public async Task DisposeAsync()
    {
        // Dispose in reverse order
        if (_browser != null)
        {
            await _browser.DisposeAsync();
        }

        if (_playwright != null)
        {
            _playwright.Dispose();
        }

        if (_appContainer != null)
        {
            await _appContainer.DisposeAsync();
        }

        if (_postgresContainer != null)
        {
            await _postgresContainer.DisposeAsync();
        }

        if (_network != null)
        {
            await _network.DeleteAsync();
        }
    }
}

