using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;

namespace CareerPilotAi.Infrastructure.Identity;

internal static class IdentityExtensions
{
    internal static IServiceCollection AddIdentityExtensions(this IServiceCollection services, IConfiguration configuration) {


        var featuresSettings = new FeaturesSettings();
        configuration.GetSection("Features").Bind(featuresSettings);

        services.AddIdentity<IdentityUser, IdentityRole>(options => 
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            
            // Email settings
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = featuresSettings.ConfirmRegistration;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "CareerPilotAuth";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(14);
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
            options.AccessDeniedPath = "/auth/access-denied";
            options.SlidingExpiration = true;
        });

        return services;
    }
}