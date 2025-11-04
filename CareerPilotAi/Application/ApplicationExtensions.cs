using CareerPilotAi.Application.Services;

namespace CareerPilotAi.Application;

public static class ApplicationExtensions
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITimeZoneService, TimeZoneService>()
            .AddScoped<IClock, Clock>()
            .AddScoped<IDashboardDataService, DashboardDataService>();
    }
}
