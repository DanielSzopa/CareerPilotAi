using CareerPilotAi.Infrastructure.Email;
using CareerPilotAi.Infrastructure.Identity;
using CareerPilotAi.Infrastructure.OpenRouter;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddSingleton<OpenRouterFeatureSettingsProvider>()
        .AddIdentityExtensions(configuration)
        .AddOpenRouter(configuration)
        .RegisterOpenRouterAppSettings(configuration)
        .AddEmailServices(configuration)
        .RegisterFeaturesSettings(configuration)
        .AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }

    private static IServiceCollection RegisterFeaturesSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<FeaturesSettings>()
            .BindConfiguration("Features")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}