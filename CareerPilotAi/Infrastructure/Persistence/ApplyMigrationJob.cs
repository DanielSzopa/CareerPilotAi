using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Infrastructure.Persistence;
internal class ApplyMigrationJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ApplyMigrationJob> _logger;

    public ApplyMigrationJob(IServiceScopeFactory serviceScopeFactory, ILogger<ApplyMigrationJob> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start ApplyMigrationJob...");
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (!dbContext.Database.IsRelational())
        {
            _logger.LogInformation("Database is not relational!");
            return;
        }
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(stoppingToken);
        if (pendingMigrations != null && pendingMigrations.Any())
        {
            _logger.LogInformation("Found some pendingMigrations");
            await dbContext.Database.MigrateAsync(stoppingToken);
            _logger.LogInformation("Migration finished.");
        } else
        {
            _logger.LogInformation("None pendingMigrations found");
        }

        _logger.LogInformation("ApplyMigrationJob finished.");
    }
}