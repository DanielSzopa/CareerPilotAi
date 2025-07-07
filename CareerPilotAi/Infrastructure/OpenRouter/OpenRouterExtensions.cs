using System.Net.Http.Headers;

namespace CareerPilotAi.Infrastructure.OpenRouter;

internal static class OpenRouterExtensions
{
    internal static IServiceCollection AddOpenRouterHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddHttpClient("OpenRouter", (client) =>
        {
            var openRouterAppSettings = new OpenRouterAppSettings();
            configuration.GetSection("OpenRouter")
                .Bind(openRouterAppSettings);

            var baseAddress = openRouterAppSettings.BaseAddress ?? throw new ArgumentNullException("BaseAddress");
            var authToken = openRouterAppSettings.AuthToken ?? throw new ArgumentNullException("AuthToken");

            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        })
            .Services;
    }

    internal static IServiceCollection RegisterOpenRouterAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<OpenRouterAppSettings>()
            .Bind(configuration.GetSection("OpenRouter"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
