using CareerPilotAi.Application.Helpers;

namespace CareerPilotAi.Application.Services;

/// <summary>
/// Service implementation for handling timezone operations based on HTTP headers
/// </summary>
public class TimeZoneService : ITimeZoneService
{
    private const string TimezoneHeaderName = "x-timezone";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TimeZoneService> _logger;

    public TimeZoneService(IHttpContextAccessor httpContextAccessor, ILogger<TimeZoneService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Gets the timezone ID from the current HTTP request header with fallback to default
    /// </summary>
    /// <returns>Timezone ID from header or default (Europe/Warsaw)</returns>
    public string GetTimeZoneFromHeader()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            _logger.LogWarning("HttpContext is null, returning default timezone");
            return TimeZoneHelper.DefaultTimeZoneId;
        }

        // Try to get timezone from header
        if (httpContext.Request.Headers.TryGetValue(TimezoneHeaderName, out var headerValue))
        {
            var timeZoneId = headerValue.FirstOrDefault();
            var validatedTimeZone = ValidateTimeZoneFromHeader(timeZoneId);
            
            if (!string.IsNullOrEmpty(validatedTimeZone))
            {
                _logger.LogDebug("Using timezone from header: {TimeZone}", validatedTimeZone);
                return validatedTimeZone;
            }

            _logger.LogWarning("Invalid timezone received from header: {TimeZone}, using default", timeZoneId);
        }
        else
        {
            _logger.LogDebug("No timezone header found, using default timezone");
        }

        return TimeZoneHelper.DefaultTimeZoneId;
    }

    /// <summary>
    /// Gets the TimeZoneInfo from the current HTTP request header with fallback to default
    /// </summary>
    /// <returns>TimeZoneInfo object for the detected timezone</returns>
    public TimeZoneInfo GetTimeZoneInfoFromHeader()
    {
        var timeZoneId = GetTimeZoneFromHeader();
        return TimeZoneHelper.GetTimeZoneInfo(timeZoneId);
    }

    /// <summary>
    /// Validates and sanitizes a timezone ID from HTTP header
    /// </summary>
    /// <param name="timeZoneId">Raw timezone ID from header</param>
    /// <returns>Validated timezone ID or null if invalid</returns>
    public string? ValidateTimeZoneFromHeader(string? timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            return null;
        }

        try
        {
            // Sanitize the input to prevent injection attacks
            var sanitized = TimeZoneHelper.SanitizeTimeZoneId(timeZoneId);
            
            if (string.IsNullOrEmpty(sanitized))
            {
                _logger.LogWarning("Timezone header value failed sanitization: {TimeZone}", timeZoneId);
                return null;
            }

            // Validate the timezone ID
            if (!TimeZoneHelper.IsValidTimeZoneId(sanitized))
            {
                _logger.LogWarning("Invalid timezone ID received from header: {TimeZone}", sanitized);
                return null;
            }

            return sanitized;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating timezone from header: {TimeZone}", timeZoneId);
            return null;
        }
    }
}
