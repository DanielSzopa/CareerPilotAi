using CareerPilotAi.Infrastructure.Identity;
using CareerPilotAi.Infrastructure.OpenRouter;
using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddSingleton<OpenRouterService>()
        .AddSingleton<OpenRouterFeatureSettingsProvider>()
        .AddIdentityExtensions()
        .AddOpenRouterHttpClient(configuration)
        .RegisterOpenRouterAppSettings(configuration)
        .AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }
}