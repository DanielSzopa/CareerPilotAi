using CareerPilotAi.Prompts.EnhanceJobDescription;
using CareerPilotAi.Prompts.GenerateInterviewQuestions;
using CareerPilotAi.Prompts.PrepareInterviewPreparationContent;

namespace CareerPilotAi.Prompts;

public static class PromptsExtensions
{
    public static IServiceCollection RegisterPrompts(this IServiceCollection services)
    {
        var prompts = new List<PromptBase>
        {
            new EnhanceJobDescriptionPrompt(),
            new GenerateInterviewQuestionsPrompt(),
            new PrepareInterviewPreparationContentPrompt()
        };

        var promptsProvider = new PromptsProvider();

        prompts.ForEach(prompt =>
        {
            promptsProvider.SetPrompt(prompt.Name, prompt.FullPath);
        });

        return services
            .AddSingleton(promptsProvider);
    }
}
