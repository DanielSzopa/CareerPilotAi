using SendGrid.Extensions.DependencyInjection;
using SendGrid.Helpers.Reliability;

namespace CareerPilotAi.Infrastructure.Email;

internal static class EmailServiceExtensions
{
    internal static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SendGridAppSettings>()
            .BindConfiguration("SendGrid")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var sendGridSettings = new SendGridAppSettings();
        configuration.GetSection("SendGrid").Bind(sendGridSettings);

        services.AddSendGrid(options =>
        {
            options.ApiKey = sendGridSettings.ApiKey ?? throw new ArgumentNullException("ApiKey");
            options.ReliabilitySettings = new ReliabilitySettings(4, TimeSpan.FromMilliseconds(500), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
        });

        services.AddTransient<EmailService>();

        return services;
    }
}