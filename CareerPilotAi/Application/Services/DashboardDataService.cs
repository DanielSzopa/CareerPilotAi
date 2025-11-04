using CareerPilotAi.Core;
using CareerPilotAi.Infrastructure.Persistence;
using CareerPilotAi.ViewModels.Dashboard;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CareerPilotAi.Application.Services;

internal class DashboardDataService : IDashboardDataService
{
    private const string DefaultTimeZoneId = "UTC";

    private static readonly IReadOnlyDictionary<string, string> StatusColorMap = new Dictionary<string, string>
    {
        [ApplicationStatus.Draft.Status] = "#0d6efd",
        [ApplicationStatus.Submitted.Status] = "#0dcaf0",
        [ApplicationStatus.InterviewScheduled.Status] = "#6f42c1",
        [ApplicationStatus.WaitingForOffer.Status] = "#ffc107",
        [ApplicationStatus.ReceivedOffer.Status] = "#198754",
        [ApplicationStatus.Rejected.Status] = "#dc3545",
        [ApplicationStatus.NoContact.Status] = "#6c757d"
    }.ToImmutableDictionary();

    private readonly ApplicationDbContext _dbContext;
    private readonly IClock _clock;
    private readonly ILogger<DashboardDataService> _logger;

    public DashboardDataService(ApplicationDbContext dbContext, IClock clock, ILogger<DashboardDataService> logger)
    {
        _dbContext = dbContext;
        _clock = clock;
        _logger = logger;
    }

    public async Task<DashboardViewModel> GetDashboardViewModelAsync(string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User identifier cannot be null or empty.", nameof(userId));
        }

        var timeZoneId = await ResolveUserTimeZoneAsync(userId, cancellationToken);

        var applications = await _dbContext.JobApplications
            .AsNoTracking()
            .Where(application => application.UserId == userId)
            .Select(application => new ApplicationSnapshot(application.Status, application.CreatedAt))
            .ToListAsync(cancellationToken);

        if (!applications.Any())
        {
            return new DashboardViewModel
            {
                HasApplications = false
            };
        }

        var metrics = BuildMetrics(applications);
        var chartsData = BuildChartsData(applications, timeZoneId);

        return new DashboardViewModel
        {
            HasApplications = true,
            Metrics = metrics,
            ChartsData = chartsData
        };
    }

    private async Task<string> ResolveUserTimeZoneAsync(string userId, CancellationToken cancellationToken)
    {
        var timeZoneId = await _dbContext.UserSettings
            .AsNoTracking()
            .Where(settings => settings.UserId == userId)
            .Select(settings => settings.TimeZoneId)
            .SingleOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            _logger.LogWarning("Time zone not configured for user {userId}. Falling back to UTC.", userId);
            return DefaultTimeZoneId;
        }

        try
        {
            _ = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return timeZoneId;
        }
        catch (TimeZoneNotFoundException ex)
        {
            _logger.LogError(ex, "Invalid time zone '{timeZoneId}' configured for user {userId}. Falling back to UTC.", timeZoneId, userId);
            return DefaultTimeZoneId;
        }
        catch (InvalidTimeZoneException ex)
        {
            _logger.LogError(ex, "Corrupted time zone '{timeZoneId}' configured for user {userId}. Falling back to UTC.", timeZoneId, userId);
            return DefaultTimeZoneId;
        }
    }

    private static DashboardMetricsViewModel BuildMetrics(IEnumerable<ApplicationSnapshot> applications)
    {
        var statusCounts = applications
            .GroupBy(application => application.Status)
            .ToDictionary(group => group.Key, group => group.Count());

        var total = statusCounts.Values.Sum();

        return new DashboardMetricsViewModel
        {
            TotalApplications = total,
            DraftCount = statusCounts.GetValueOrDefault(ApplicationStatus.Draft.Status),
            SubmittedCount = statusCounts.GetValueOrDefault(ApplicationStatus.Submitted.Status),
            InterviewScheduledCount = statusCounts.GetValueOrDefault(ApplicationStatus.InterviewScheduled.Status),
            WaitingForOfferCount = statusCounts.GetValueOrDefault(ApplicationStatus.WaitingForOffer.Status),
            ReceivedOfferCount = statusCounts.GetValueOrDefault(ApplicationStatus.ReceivedOffer.Status),
            RejectedCount = statusCounts.GetValueOrDefault(ApplicationStatus.Rejected.Status),
            NoContactCount = statusCounts.GetValueOrDefault(ApplicationStatus.NoContact.Status)
        };
    }

    private DashboardChartsDataViewModel BuildChartsData(IEnumerable<ApplicationSnapshot> applications, string timeZoneId)
    {
        var localSnapshots = applications
            .Select(application => new
            {
                application.Status,
                LocalCreatedAt = _clock.GetDateTimeAdjustedToTimeZone(application.CreatedAtUtc, timeZoneId)
            })
            .ToList();

        var totalApplications = localSnapshots.Count;

        var statusDistribution = localSnapshots
            .GroupBy(snapshot => snapshot.Status)
            .Select(group => new StatusChartItemViewModel
            {
                Status = group.Key,
                Count = group.Count(),
                Color = StatusColorMap.TryGetValue(group.Key, out var color) ? color : "#0d6efd",
                Percentage = totalApplications == 0 ? 0 : Math.Round((decimal)group.Count() / totalApplications * 100, 1)
            })
            .OrderByDescending(item => item.Count)
            .ToList();

        var localDates = localSnapshots
            .Select(snapshot => DateOnly.FromDateTime(snapshot.LocalCreatedAt))
            .ToList();

        var nowLocal = _clock.GetDateTimeAdjustedToTimeZone(DateTime.UtcNow, timeZoneId);

        return new DashboardChartsDataViewModel
        {
            StatusDistribution = statusDistribution,
            ThirtyDaysData = BuildDailySeries(localDates, nowLocal, 30),
            SixtyDaysData = BuildWeeklySeries(localDates, nowLocal, 60),
            NinetyDaysData = BuildWeeklySeries(localDates, nowLocal, 90)
        };
    }

    private static ApplicationsOverTimeDataViewModel BuildDailySeries(IReadOnlyCollection<DateOnly> localDates, DateTime nowLocal, int days)
    {
        var dateCounts = localDates
            .GroupBy(date => date)
            .ToDictionary(group => group.Key, group => group.Count());

        var startDate = DateOnly.FromDateTime(nowLocal.Date).AddDays(-(days - 1));
        var labels = new List<string>();
        var values = new List<int>();

        for (var current = startDate; current <= DateOnly.FromDateTime(nowLocal.Date); current = current.AddDays(1))
        {
            labels.Add(current.ToString("dd MMM", CultureInfo.CurrentCulture));
            values.Add(dateCounts.GetValueOrDefault(current));
        }

        return new ApplicationsOverTimeDataViewModel
        {
            Labels = labels,
            Values = values
        };
    }

    private static ApplicationsOverTimeDataViewModel BuildWeeklySeries(IReadOnlyCollection<DateOnly> localDates, DateTime nowLocal, int days)
    {
        var dateCounts = localDates
            .GroupBy(date => date)
            .ToDictionary(group => group.Key, group => group.Count());

        var startDate = DateOnly.FromDateTime(nowLocal.Date).AddDays(-(days - 1));
        var labels = new List<string>();
        var values = new List<int>();

        var index = 1;
        for (var currentStart = startDate; currentStart <= DateOnly.FromDateTime(nowLocal.Date); currentStart = currentStart.AddDays(7))
        {
            var currentEnd = currentStart.AddDays(6);
            if (currentEnd > DateOnly.FromDateTime(nowLocal.Date))
            {
                currentEnd = DateOnly.FromDateTime(nowLocal.Date);
            }

            var total = 0;
            for (var day = currentStart; day <= currentEnd; day = day.AddDays(1))
            {
                total += dateCounts.GetValueOrDefault(day);
            }

            labels.Add($"Week {index} ({currentStart:dd.MM}-{currentEnd:dd.MM})");
            values.Add(total);
            index++;
        }

        return new ApplicationsOverTimeDataViewModel
        {
            Labels = labels,
            Values = values
        };
    }

    private sealed record ApplicationSnapshot(string Status, DateTime CreatedAtUtc);
}

