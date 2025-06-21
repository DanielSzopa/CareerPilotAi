using System.Net.Http.Headers;

namespace CareerPilotAi.Infrastructure.OpenRouter;

internal static class OpenRouterHttpClient
{
    internal static IServiceCollection AddOpenRouterHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddHttpClient("OpenRouter", (client) =>
        {
            var baseAddress = configuration["OpenRouter:BaseAddress"] ?? throw new ArgumentNullException("BaseAddress");
            var authToken = configuration["OpenRouter:AuthToken"] ?? throw new ArgumentNullException("AuthToken");

            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        })
            .Services;
    }
}
