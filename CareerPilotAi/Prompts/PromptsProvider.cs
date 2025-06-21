namespace CareerPilotAi.Prompts;

public class PromptsProvider
{
    private readonly Dictionary<string, string> _prompts = new Dictionary<string, string>();

    public string GetPrompt(PromptBase promptObj)
    {
        if (_prompts.TryGetValue(promptObj.Name, out var prompt))
        {
            return prompt;
        }

        throw new KeyNotFoundException($"Prompt with key '{promptObj.Name}' not found.");
    }

    public void SetPrompt(string key, string path)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Key and path cannot be null or empty.");
        }

        var fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException($"Prompt file '{path}' does not exist.");
        }

        var promptContent = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(promptContent))
        {
            throw new ArgumentException($"Prompt {key}'s content cannot be empty.");
        }

        _prompts[key] = promptContent;
    }
}