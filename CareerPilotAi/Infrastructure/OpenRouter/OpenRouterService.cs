using CareerPilotAi.Prompts;
using CareerPilotAi.Prompts.GenerateInterviewQuestions;
using CareerPilotAi.Prompts.ParseJobDescription;
using CareerPilotAi.Prompts.PrepareInterviewPreparationContent;
using System.Text.Json;

namespace CareerPilotAi.Infrastructure.OpenRouter;

public class OpenRouterService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenRouterFeatureSettingsProvider _openRouterFeatureSettings;
    private readonly PromptsProvider _promptsProvider;

    public OpenRouterService(IHttpClientFactory httpClientFactory, OpenRouterFeatureSettingsProvider openRouterFeatureSettings, PromptsProvider promptsProvider)
    {
        _httpClientFactory = httpClientFactory;
        _openRouterFeatureSettings = openRouterFeatureSettings;
        _promptsProvider = promptsProvider;
    }

    private async Task<OpenRouterCommonResponse> SendOpenRouterRequestAsync(string jsonContent, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("OpenRouter");
        var requestContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("api/v1/chat/completions", requestContent, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OpenRouterCommonResponse>(cancellationToken) ?? throw new InvalidOperationException("Failed to deserialize response from OpenRouter.");
    }

    public async Task<GenerateInterviewQuestionsPromptOutputModel> GenerateInterviewQuestionsAsync(GenerateInterviewQuestionsPromptInputModel inputModel, CancellationToken cancellationToken)
    {
        #region Validate Input
        if (inputModel == null)
        {
            throw new ArgumentNullException(nameof(inputModel), "Input model cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(inputModel.CompanyName))
        {
            throw new ArgumentException("Company name cannot be null or empty.", nameof(inputModel.CompanyName));
        }

        if (string.IsNullOrWhiteSpace(inputModel.InterviewQuestionsPreparation))
        {
            throw new ArgumentException("Interview questions preparation cannot be null or empty.", nameof(inputModel.InterviewQuestionsPreparation));
        }

        if (string.IsNullOrWhiteSpace(inputModel.JobRole))
        {
            throw new ArgumentException("Job role cannot be null or empty.", nameof(inputModel.JobRole));
        }
        #endregion


        var prompt = _promptsProvider.GetPrompt(new GenerateInterviewQuestionsPrompt());
        var settings = _openRouterFeatureSettings.Get(OpenRouterFeatureSettingsProvider.GenerateInterviewQuestions);

        var userInputMessage = JsonSerializer.Serialize(inputModel);
        var request = new
        {
            model = settings.Model,
            temperature = settings.Temperature,
            stream = false,
            response_format = new
            {
                type = "json_object"
            },
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = prompt
                },
                new
                {
                    role = "user",
                    content = userInputMessage
                }
            }
        };

        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
        });
        var response = await SendOpenRouterRequestAsync(requestJson, cancellationToken);
        var messageContent = response?.Choices.FirstOrDefault()?.Message?.Content ?? throw new InvalidOperationException($"No content found in the response from OpenRouter. Action: {nameof(GenerateInterviewQuestionsAsync)}");
        return JsonSerializer.Deserialize<GenerateInterviewQuestionsPromptOutputModel>(messageContent) ?? throw new InvalidOperationException($"Failed to deserialize response content to {nameof(GenerateInterviewQuestionsPromptOutputModel)}. Action: {nameof(GenerateInterviewQuestionsAsync)}");
    }

    public async Task<PrepareInterviewPreparationContentPromptOutputModel> PrepareInterviewPreparationContentAsync(PrepareInterviewPreparationContentPromptInputModel inputModel, CancellationToken cancellationToken)
    {
        #region Validate Input
        if (inputModel == null)
        {
            throw new ArgumentNullException(nameof(inputModel), "Input model cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(inputModel.JobDescription))
        {
            throw new ArgumentException("Job description cannot be null or empty.", nameof(inputModel.JobDescription));
        }

        if (string.IsNullOrWhiteSpace(inputModel.JobRole))
        {
            throw new ArgumentException("Job role cannot be null or empty.", nameof(inputModel.JobRole));
        }
        #endregion

        var prompt = _promptsProvider.GetPrompt(new PrepareInterviewPreparationContentPrompt());
        var settings = _openRouterFeatureSettings.Get(OpenRouterFeatureSettingsProvider.PrepareInterviewPreparationContent);

        var userInputMessage = JsonSerializer.Serialize(inputModel);
        var request = new
        {
            model = settings.Model,
            temperature = settings.Temperature,
            stream = false,
            response_format = new
            {
                type = "json_object"
            },
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = prompt
                },
                new
                {
                    role = "user",
                    content = userInputMessage
                }
            }
        };

        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
        });
        var response = await SendOpenRouterRequestAsync(requestJson, cancellationToken);
        var messageContent = response?.Choices.FirstOrDefault()?.Message?.Content ?? throw new InvalidOperationException($"No content found in the response from OpenRouter. Action: {nameof(PrepareInterviewPreparationContentAsync)}");
        return JsonSerializer.Deserialize<PrepareInterviewPreparationContentPromptOutputModel>(messageContent) ?? throw new InvalidOperationException($"Failed to deserialize response content to {nameof(PrepareInterviewPreparationContentPromptOutputModel)}. Action: {nameof(PrepareInterviewPreparationContentAsync)}");
    }

    public async Task<ParseJobDescriptionOutputModel> ParseJobDescriptionAsync(ParseJobDescriptionInputModel inputModel, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(inputModel.JobDescriptionText))
        {
            throw new ArgumentException("JobDescriptionText cannot be null or empty.", nameof(inputModel.JobDescriptionText));
        }

        var prompt = _promptsProvider.GetPrompt(new ParseJobDescriptionPrompt());
        var settings = _openRouterFeatureSettings.Get(OpenRouterFeatureSettingsProvider.ParseJobDescription);
        var request = new
        {
            model = settings.Model,
            temperature = settings.Temperature,
            stream = false,
            response_format = new
            {
                type = "json_object"
            },
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = prompt
                },
                new
                {
                    role = "user",
                    content = inputModel.JobDescriptionText
                }
            }
        };

        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
        });

        var response = await SendOpenRouterRequestAsync(requestJson, cancellationToken);
        var messageContent = response?.Choices.FirstOrDefault()?.Message?.Content ?? throw new InvalidOperationException($"No content found in the response from OpenRouter. Action: {nameof(ParseJobDescriptionAsync)}");
        return JsonSerializer.Deserialize<ParseJobDescriptionOutputModel>(messageContent) ?? throw new InvalidOperationException($"Failed to deserialize response content to {nameof(ParseJobDescriptionOutputModel)}. Action: {nameof(ParseJobDescriptionAsync)}");
    }
}
