using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.EnhanceJobDescription;

public record EnhanceJobDescriptionCommand(string JobDescriptionText) : ICommand;
