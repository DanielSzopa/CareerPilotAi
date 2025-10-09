namespace CareerPilotAi.Application.Services;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Registers application services in the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserService, UserService>();
    }
}
