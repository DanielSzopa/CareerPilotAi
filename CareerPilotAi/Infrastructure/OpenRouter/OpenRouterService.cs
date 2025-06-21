using CareerPilotAi.Prompts;
using CareerPilotAi.Prompts.EnhanceJobDescription;
using CareerPilotAi.Prompts.PersonalDataPdfScrape;
using System.Text.Json;

namespace CareerPilotAi.Infrastructure.OpenRouter;

public class OpenRouterService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenRouterLlmModelProvider _llmModelProvider;
    private readonly PromptsProvider _promptsProvider;

    public OpenRouterService(IHttpClientFactory httpClientFactory, OpenRouterLlmModelProvider llmModelProvider, PromptsProvider promptsProvider)
    {
        _httpClientFactory = httpClientFactory;
        _llmModelProvider = llmModelProvider;
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

    public async Task<PersonalDataPdfScrapeResponseModel> ScrapePersonalInformationPdf(string fileName, string base64String, CancellationToken cancellationToken)
    {
        var prompt = _promptsProvider.GetPrompt(new PersonalDataPdfScrapePrompt());
        var request = new
        {
            model = _llmModelProvider.GetModel(OpenRouterLlmModelProvider.PersonalDetailsPdfUploadModel),
            stream = false,
            response_format = new
            {
                type = "json_object"
            },
            messages = new[]
            {
                    new
                    {
                        Role = "user",
                        Content = new []
                        {
                            new
                            {
                                Type = "text",
                                Text = prompt,
                                File = new
                                {
                                    FileName = "",
                                    FileData = ""
                                }
                            },
                            new
                            {
                                Type = "file",
                                Text = "",
                                File = new
                                {
                                    FileName = fileName,
                                    FileData = $"data:application/pdf;base64,{base64String}"
                                }
                            }
                        }
                    }
            }
        };

        var response = await SendOpenRouterRequestAsync(JsonSerializer.Serialize(request), cancellationToken);
        var messageContent = response?.Choices.FirstOrDefault()?.Message?.Content ?? throw new InvalidOperationException($"No content found in the response from OpenRouter. Action: {nameof(ScrapePersonalInformationPdf)}");
        return JsonSerializer.Deserialize<PersonalDataPdfScrapeResponseModel>(messageContent) ?? throw new InvalidOperationException($"Failed to deserialize response content to {nameof(PersonalDataPdfScrapeResponseModel)}. Action: {nameof(ScrapePersonalInformationPdf)}");
    }

    public async Task<EnhanceJobDescriptionResponseModel> EnhanceJobDescriptionAsync(string rawContentText, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(rawContentText))
        {
            throw new ArgumentException("Raw content text cannot be null or empty.", nameof(rawContentText));
        }

        var prompt = _promptsProvider.GetPrompt(new EnhanceJobDescriptionPrompt());
        var request = new
        {
            model = _llmModelProvider.GetModel(OpenRouterLlmModelProvider.EnhanceJobDescriptionModel),
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
                    content = rawContentText
                }
            }
        };

        var response = await SendOpenRouterRequestAsync(JsonSerializer.Serialize(request), cancellationToken);
        var messageContent = response?.Choices.FirstOrDefault()?.Message?.Content ?? throw new InvalidOperationException($"No content found in the response from OpenRouter. Action: {nameof(EnhanceJobDescriptionAsync)}");
        return JsonSerializer.Deserialize<EnhanceJobDescriptionResponseModel>(messageContent) ?? throw new InvalidOperationException($"Failed to deserialize response content to {nameof(EnhanceJobDescriptionResponseModel)}. Action: {nameof(EnhanceJobDescriptionAsync)}");
    }
}
