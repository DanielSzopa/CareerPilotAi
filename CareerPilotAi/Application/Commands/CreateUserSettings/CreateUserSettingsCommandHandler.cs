using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.Infrastructure.Persistence.DataModels;

namespace CareerPilotAi.Application.Commands.CreateUserSettings;

public class CreateUserSettingsCommandHandler : ICommandHandler<CreateUserSettingsCommand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CreateUserSettingsCommandHandler> _logger;

    public CreateUserSettingsCommandHandler(ApplicationDbContext dbContext, ILogger<CreateUserSettingsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task HandleAsync(CreateUserSettingsCommand command, CancellationToken cancellationToken)
    {
        var userSettings = new UserSettingsDataModel
        {
            UserId = command.UserId,
            TimeZoneId = "Europe/Warsaw",
        };

        await _dbContext.UserSettings.AddAsync(userSettings, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created user settings for UserId: {UserId}", command.UserId);
    }
}
