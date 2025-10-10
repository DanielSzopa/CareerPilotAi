using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.CreateUserSettings;

public record CreateUserSettingsCommand(string UserId) : ICommand;