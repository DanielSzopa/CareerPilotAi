using Microsoft.Extensions.Options;

namespace CareerPilotAi.Infrastructure.OpenRouter;

public class OpenRouterFeatureSettingsProvider
{
    public const string EnhanceJobDescription = nameof(EnhanceJobDescription);
    public const string PersonalDetailsPdfUpload = nameof(PersonalDetailsPdfUpload);
    public const string GenerateInterviewQuestions = nameof(GenerateInterviewQuestions);
    public const string PrepareInterviewPreparationContent = nameof(PrepareInterviewPreparationContent);

    private readonly IOptions<OpenRouterAppSettings> _options;

    public OpenRouterFeatureSettingsProvider(IOptions<OpenRouterAppSettings> options)
    {
        _options = options;
    }

    public OpenRouterFeatures Get(string modelName)
    {
        var settings = _options.Value.Features
            .FirstOrDefault(f => f.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase));

        return settings ?? throw new InvalidOperationException($"{modelName} not configured in OpenRouter settings.");
    }
}