using System.ComponentModel.DataAnnotations;

namespace CareerPilotAi.Infrastructure.OpenRouter;

public class OpenRouterAppSettings
{
    [Required]
    public string BaseAddress { get; set; }
    [Required]
    public string AuthToken { get; set; }

    [Required]
    public OpenRouterFeatures[] Features { get; set; }
}
