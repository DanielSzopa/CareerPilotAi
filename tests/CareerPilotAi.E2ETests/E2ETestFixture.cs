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

    private static readonly int _port = 8080;
    public IBrowser Browser => _browser ?? throw new InvalidOperationException("Browser not initialized");
    public string BaseUrl { get; private set; } = $"http://localhost:{_port}";

    public async Task InitializeAsync()
    {
        // isSelfHostedRun is set to true then e2e tests will run docker containers itself.
        // If it is set to false then e2e tests will use containers runned manualy thanks to docker-compose-e2e-test.yml file.
        var isSelfHostedRun = true;

        if(isSelfHostedRun)
        {
            await InitializeSelfHostedE2ETestsRunAsync();
        }

        // Step 5: Initialize Playwright and launch Chromium browser
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    private async Task InitializeSelfHostedE2ETestsRunAsync()
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
            .WithPortBinding(_port, _port)
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "e2e")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(_port))
            .Build();

        await _appContainer.StartAsync();
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

