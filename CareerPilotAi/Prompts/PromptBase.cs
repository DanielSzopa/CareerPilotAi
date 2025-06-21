namespace CareerPilotAi.Prompts;

public abstract class PromptBase {
    public string Name { get; }
    
    public string FullPath { get; }
    protected PromptBase(string name, string subDirectory, string fileName)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(subDirectory) || string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Name, subdirectory, and file name cannot be null or empty.");
        }

        var basePath = AppContext.BaseDirectory;
        var fullPath = Path.Combine(basePath, "Prompts", subDirectory, fileName);
        var fileInfo = new FileInfo(fullPath);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException($"Prompt file '{fullPath}' does not exist.");
        }

        Name = name;
        FullPath = fullPath;
    }
}
