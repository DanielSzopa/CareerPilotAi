using System.Text.Json.Serialization;

namespace CareerPilotAi.Infrastructure.OpenRouter;

public class OpenRouterScrapeResumeRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;

    [JsonPropertyName("messages")]
    public Message[] Messages { get; set; }
}

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public List<Content> Content { get; set; }
}

public class Content
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("file")]
    public FileOpenRouter File { get; set; }
}

public class FileOpenRouter
{
    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("file_data")]
    public string FileData { get; set; }
}
