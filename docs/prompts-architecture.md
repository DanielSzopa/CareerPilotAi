# Prompts Architecture Documentation

## Overview

The CareerPilotAi application implements a structured architecture for managing LLM (Large Language Model) prompts and their associated data models. This architecture provides a consistent pattern for creating, managing, and executing AI-powered features through the OpenRouter service.

## Architecture Components

### 1. Directory Structure

```
CareerPilotAi/Prompts/
├── PromptBase.cs                    # Abstract base class for all prompts
├── PromptsProvider.cs               # Service for managing prompt content
├── PromptsExtensions.cs             # DI registration extensions
└── [FeatureName]/                   # Feature-specific prompt directory
    ├── [FeatureName]Prompt.cs       # Prompt class inheriting from PromptBase
    ├── [FeatureName]Prompt.md       # Actual prompt text with instructions
    ├── [FeatureName]InputModel.cs   # Input data model (if needed)
    ├── [FeatureName]OutputModel.cs  # Output data model for LLM response
    ├── [FeatureName]Input.json      # Example input (documentation)
    └── [FeatureName]Output.json     # Example output (documentation)
```

### 2. Core Components

#### PromptBase (Abstract Class)
- **Purpose**: Base class for all prompt implementations
- **Responsibilities**:
  - Validates prompt file existence during construction
  - Provides consistent naming convention
  - Manages file paths for prompt content
- **Key Properties**:
  - `Name`: Unique identifier for the prompt
  - `FullPath`: Complete file path to the .md prompt file

#### PromptsProvider (Service)
- **Purpose**: Central service for loading and accessing prompt content
- **Responsibilities**:
  - Loads prompt content from .md files into memory
  - Provides thread-safe access to prompt content
  - Validates prompt existence and content
- **Key Methods**:
  - `GetPrompt(PromptBase promptObj)`: Retrieves prompt content by prompt object
  - `SetPrompt(string key, string path)`: Loads prompt content from file

#### PromptsExtensions (DI Registration)
- **Purpose**: Dependency injection setup for prompt system
- **Responsibilities**:
  - Registers all available prompts during application startup
  - Configures PromptsProvider as singleton service
  - Loads all prompt content into memory

## Implementation Pattern

### 1. Creating a New Prompt Feature

#### Step 1: Create Feature Directory
Create a new directory under `CareerPilotAi/Prompts/` with your feature name:
```
CareerPilotAi/Prompts/[YourFeature]/
```

#### Step 2: Create Prompt Class
```csharp
namespace CareerPilotAi.Prompts.[YourFeature];

public class [YourFeature]Prompt : PromptBase
{
    public [YourFeature]Prompt() : base(nameof([YourFeature]Prompt), "[YourFeature]", "[YourFeature]Prompt.md")
    {
    }
}
```

#### Step 3: Create Prompt Content (.md file)
- Contains detailed instructions for the LLM
- Defines expected input/output formats
- Includes examples and validation rules
- Uses structured JSON schemas for input/output

#### Step 4: Create Data Models

**Input Model (if complex input is needed):**
```csharp
namespace CareerPilotAi.Prompts.[YourFeature];

public record [YourFeature]PromptInputModel(string Property1, string Property2);
```

**Output Model:**
```csharp
namespace CareerPilotAi.Prompts.[YourFeature];

public class [YourFeature]PromptOutputModel
{
    public string Status { get; set; }
    public string Message { get; set; }
    // Additional properties based on LLM response structure
}
```

#### Step 5: Create Example JSON Files
- `[YourFeature]Input.json`: Example input data for documentation
- `[YourFeature]Output.json`: Example LLM response for documentation

#### Step 6: Register in PromptsExtensions
```csharp
public static IServiceCollection RegisterPrompts(this IServiceCollection services)
{
    var prompts = new List<PromptBase>
    {
        // ...existing prompts...
        new [YourFeature]Prompt()
    };
    // ...rest of registration logic...
}
```

#### Step 7: Implement OpenRouter Service Method
```csharp
public async Task<[YourFeature]PromptOutputModel> [YourFeature]Async([YourFeature]PromptInputModel inputModel, CancellationToken cancellationToken)
{
    // Input validation
    var prompt = _promptsProvider.GetPrompt(new [YourFeature]Prompt());
    var settings = _openRouterFeatureSettings.Get(OpenRouterFeatureSettingsProvider.[YourFeature]);
    
    // Prepare OpenRouter request
    var userInputMessage = JsonSerializer.Serialize(inputModel);
    var request = new
    {
        model = settings.Model,
        temperature = settings.Temperature,
        stream = false,
        response_format = new { type = "json_object" },
        messages = new[]
        {
            new { role = "system", content = prompt },
            new { role = "user", content = userInputMessage }
        }
    };
    
    // Send request and handle response
    var requestJson = JsonSerializer.Serialize(request);
    var response = await SendOpenRouterRequestAsync(requestJson, cancellationToken);
    var messageContent = response?.Choices.FirstOrDefault()?.Message?.Content ?? 
        throw new InvalidOperationException($"No content found in the response from OpenRouter. Action: {nameof([YourFeature]Async)}");
    
    return JsonSerializer.Deserialize<[YourFeature]PromptOutputModel>(messageContent) ?? 
        throw new InvalidOperationException($"Failed to deserialize response content to {nameof([YourFeature]PromptOutputModel)}. Action: {nameof([YourFeature]Async)}");
}
```

## Current Implementation Examples

### 1. GenerateInterviewQuestions Feature
- **Input**: Job description, company name, job role
- **Output**: Structured interview questions with answers and feedback
- **Pattern**: System prompt + JSON user input → JSON response

### 2. EnhanceJobDescription Feature
- **Input**: Raw job description text
- **Output**: Enhanced and structured job description
- **Pattern**: System prompt + text user input → JSON response

### 3. PersonalDataPdfScrape Feature
- **Input**: PDF file (base64) + filename
- **Output**: Extracted personal information
- **Pattern**: System prompt + file attachment → JSON response

## Best Practices

### 1. Prompt Design (.md files)
- **Clear Instructions**: Define the LLM's role and task explicitly
- **Structured Output**: Always specify JSON schema for responses
- **Input Validation**: Include instructions for handling malformed input
- **Examples**: Provide clear input/output examples
- **Error Handling**: Define error statuses and feedback mechanisms

### 2. Data Models
- **Input Models**: Use `record` types for simple, immutable input data
- **Output Models**: Use classes with public setters for JSON deserialization
- **Naming Convention**: Follow `[Feature]PromptInputModel` / `[Feature]PromptOutputModel` pattern
- **Validation**: Include validation attributes where appropriate

### 3. File Organization
- **Consistent Naming**: All files in feature directory follow `[Feature]Prompt.*` pattern
- **Documentation**: Include example JSON files for developer reference
- **Single Responsibility**: Each prompt directory handles one specific AI feature

### 4. Error Handling
- **Input Validation**: Validate all inputs before sending to LLM
- **Response Validation**: Ensure LLM response can be deserialized
- **Meaningful Exceptions**: Provide context-specific error messages
- **Logging**: Include feature name in error messages for debugging

## OpenRouter Integration

### Request Structure
All prompts follow consistent OpenRouter API request structure:
```json
{
    "model": "configured-model",
    "temperature": "configured-temperature", 
    "stream": false,
    "response_format": { "type": "json_object" },
    "messages": [
        { "role": "system", "content": "prompt-content" },
        { "role": "user", "content": "user-input" }
    ]
}
```

### Response Handling
- All responses expect JSON format from LLM
- Consistent error handling for missing or invalid responses
- Proper deserialization with meaningful error messages

## Performance Considerations

### 1. Memory Management
- **Singleton PromptsProvider**: All prompts loaded once at startup
- **Efficient Lookup**: Dictionary-based prompt retrieval
- **Immutable Content**: Prompt content is read-only after loading

### 2. Async Operations
- **Cancellation Support**: All OpenRouter calls support `CancellationToken`
- **Proper Async/Await**: Non-blocking I/O operations
- **HttpClient Factory**: Proper HTTP client management

## Security Considerations

### 1. Input Sanitization
- Validate all user inputs before sending to LLM
- Prevent prompt injection attacks through input validation
- Sanitize file uploads (PDF processing)