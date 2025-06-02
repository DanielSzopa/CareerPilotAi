using System.Text.Json.Serialization;

namespace CareerPilotAi.Infrastructure.OpenRouter;

public class OpenRouterCommonResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
}

public class Choice
{
    [JsonPropertyName("logprobs")]
    public object LogProbs { get; set; }

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }

    [JsonPropertyName("native_finish_reason")]
    public string NativeFinishReason { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("message")]
    public ResponseMessage Message { get; set; }
}

public class ResponseMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("refusal")]
    public object Refusal { get; set; }

    [JsonPropertyName("reasoning")]
    public object Reasoning { get; set; }
}

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}