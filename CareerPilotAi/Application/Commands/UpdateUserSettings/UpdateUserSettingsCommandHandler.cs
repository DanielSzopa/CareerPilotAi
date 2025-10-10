using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Application.Commands.UpdateUserSettings;

public class UpdateUserSettingsCommandHandler : ICommandHandler<UpdateUserSettingsCommand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UpdateUserSettingsCommandHandler> _logger;

    public UpdateUserSettingsCommandHandler(ApplicationDbContext dbContext, ILogger<UpdateUserSettingsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task HandleAsync(UpdateUserSettingsCommand command, CancellationToken cancellationToken)
    {
        var existingSettings = await _dbContext.UserSettings
            .SingleOrDefaultAsync(settings => settings.UserId == command.UserId, cancellationToken);

        if (existingSettings is null)
        {
            var newUserSettings = new UserSettingsDataModel
            {
                UserSettingsId = Guid.NewGuid(),
                UserId = command.UserId,
                TimeZoneId = command.TimeZoneId,
            };

            _dbContext.UserSettings.Add(newUserSettings);

            _logger.LogInformation("Creating new user settings for UserId: {UserId} with TimeZoneId: {TimeZoneId}", command.UserId, command.TimeZoneId);
        }
        else
        {
            existingSettings.TimeZoneId = command.TimeZoneId;

            _logger.LogInformation("Updating existing user settings for UserId: {UserId} to TimeZoneId: {TimeZoneId}", command.UserId, command.TimeZoneId);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

