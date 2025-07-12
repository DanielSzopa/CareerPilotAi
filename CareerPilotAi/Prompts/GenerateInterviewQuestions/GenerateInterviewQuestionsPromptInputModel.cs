using CareerPilotAi.Core;

namespace CareerPilotAi.Prompts.GenerateInterviewQuestions;

public record GenerateInterviewQuestionsPromptInputModel(string CompanyName, string JobRole, string InterviewQuestionsPreparation, List<SingleInterviewQuestion> Questions, byte numberOfQuestionsToGenerate);