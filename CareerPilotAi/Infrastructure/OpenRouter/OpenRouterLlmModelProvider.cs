namespace CareerPilotAi.Infrastructure.OpenRouter;

public class OpenRouterLlmModelProvider
{
    public const string EnhanceJobDescriptionModel = nameof(EnhanceJobDescriptionModel);
    public const string PersonalDetailsPdfUploadModel = nameof(PersonalDetailsPdfUploadModel);

    private readonly IConfiguration _configuration;

    public OpenRouterLlmModelProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetModel(string modelName)
    {
        var dict = new Dictionary<string, string>
        {
            { EnhanceJobDescriptionModel, "OpenRouter:Models:EnhanceJobDescriptionModel" },
            { PersonalDetailsPdfUploadModel, "OpenRouter:Models:PersonalDetailsPdfUploadModel" }
        };
        return _configuration.GetValue<string>(dict[modelName])
               ?? throw new InvalidOperationException($"{modelName} not configured in OpenRouter settings.");
    }
}