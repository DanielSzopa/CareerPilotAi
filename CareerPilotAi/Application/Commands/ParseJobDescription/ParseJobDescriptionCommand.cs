using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.ParseJobDescription;

public record ParseJobDescriptionCommand(
    string JobDescriptionText) : ICommand<ParseJobDescriptionResponse>;

