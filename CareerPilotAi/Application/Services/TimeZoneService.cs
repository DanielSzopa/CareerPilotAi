using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Application.Services;

internal class TimeZoneService : ITimeZoneService
{
    private readonly ApplicationDbContext _dbContext;

    public TimeZoneService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<(string Id, string Name)>> GetAllAsync(CancellationToken cancellationToken)
    {
        var timeZones = await _dbContext.TimeZones
            .AsNoTracking()
            .OrderBy(timeZone => timeZone.Name)
            .Select(timeZone => new { timeZone.TimeZoneId, timeZone.Name })
            .ToListAsync(cancellationToken);

        return timeZones
            .Select(timeZone => (timeZone.TimeZoneId, timeZone.Name))
            .ToList();
    }

    public Task<bool> ExistsAsync(string timeZoneId, CancellationToken cancellationToken)
    {
        return _dbContext.TimeZones
            .AsNoTracking()
            .AnyAsync(timeZone => timeZone.TimeZoneId == timeZoneId, cancellationToken);
    }
}

