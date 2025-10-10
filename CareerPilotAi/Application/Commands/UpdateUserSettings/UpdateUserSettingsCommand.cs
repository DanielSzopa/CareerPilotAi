using CareerPilotAi.Application.Commands.Abstractions;

namespace CareerPilotAi.Application.Commands.UpdateUserSettings;

public record UpdateUserSettingsCommand(string UserId, string TimeZoneId) : ICommand;

