using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Commands.CreateJobApplication;
using CareerPilotAi.Application.Commands.Dispatcher;
using CareerPilotAi.Application.Commands.EnhanceJobDescription;
using CareerPilotAi.Application.Commands.UpdateJobDescription;

namespace CareerPilotAi.Application.Commands;

internal static class CommandsExtensions
{
    internal static IServiceCollection RegisterCommands(this IServiceCollection services)
    {
        return services
            .AddScoped<ICommandDispatcher, CommandDispatcher>()
            .AddScoped<ICommandHandler<CreateJobApplicationCommand, Guid>, CreateJobApplicationCommandHandler>()
            .AddScoped<ICommandHandler<EnhanceJobDescriptionCommand, EnhanceJobDescriptionResponse>, EnhanceJobDescriptionCommandHandler>()
            .AddScoped<ICommandHandler<UpdateJobDescriptionCommand, UpdateJobDescriptionResponse>, UpdateJobDescriptionCommandHandler>();
    }
}