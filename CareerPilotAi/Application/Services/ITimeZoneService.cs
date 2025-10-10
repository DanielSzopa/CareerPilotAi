namespace CareerPilotAi.Application.Services;

public interface ITimeZoneService
{
    Task<IReadOnlyList<(string Id, string Name)>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string timeZoneId, CancellationToken cancellationToken);
}

