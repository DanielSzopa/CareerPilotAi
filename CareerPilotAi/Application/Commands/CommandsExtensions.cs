using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Commands.CreateJobApplication;
using CareerPilotAi.Application.Commands.DeleteJobApplication;
using CareerPilotAi.Application.Commands.DeleteInterviewQuestion;
using CareerPilotAi.Application.Commands.Dispatcher;
using CareerPilotAi.Application.Commands.EnhanceJobDescription;
using CareerPilotAi.Application.Commands.GenerateInterviewQuestions;
using CareerPilotAi.Application.Commands.PrepareInterviewPreparationContent;
using CareerPilotAi.Application.Commands.SaveInterviewPreparationContent;
using CareerPilotAi.Application.Commands.UpdateJobDescription;
using CareerPilotAi.Application.Commands.UpdateJobApplicationStatus;

namespace CareerPilotAi.Application.Commands;

internal static class CommandsExtensions
{    internal static IServiceCollection RegisterCommands(this IServiceCollection services)
    {
        return services
            .AddScoped<ICommandDispatcher, CommandDispatcher>()
            .AddScoped<ICommandHandler<CreateJobApplicationCommand, Guid>, CreateJobApplicationCommandHandler>()
            .AddScoped<ICommandHandler<EnhanceJobDescriptionCommand, EnhanceJobDescriptionResponse>, EnhanceJobDescriptionCommandHandler>()
            .AddScoped<ICommandHandler<UpdateJobDescriptionCommand, UpdateJobDescriptionResponse>, UpdateJobDescriptionCommandHandler>()
            .AddScoped<ICommandHandler<UpdateJobApplicationStatusCommand, UpdateJobApplicationStatusResponse>, UpdateJobApplicationStatusCommandHandler>()
            .AddScoped<ICommandHandler<DeleteJobApplicationCommand, DeleteJobApplicationResponse>, DeleteJobApplicationCommandHandler>()
            .AddScoped<ICommandHandler<DeleteInterviewQuestionCommand, DeleteInterviewQuestionResponse>, DeleteInterviewQuestionCommandHandler>()
            .AddScoped<ICommandHandler<GenerateInterviewQuestionsCommand, GenerateInterviewQuestionsResponse>, GenerateInterviewQuestionsCommandHandler>()
            .AddScoped<ICommandHandler<PrepareInterviewPreparationContentCommand, PrepareInterviewPreparationContentResponse>, PrepareInterviewPreparationContentCommandHandler>()
            .AddScoped<ICommandHandler<SaveInterviewPreparationContentCommand, SaveInterviewPreparationContentResponse>, SaveInterviewPreparationContentCommandHandler>();
    }
}