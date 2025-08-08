namespace CareerPilotAi.Application.Services;

/// <summary>
/// Service interface for handling timezone operations based on HTTP headers
/// </summary>
public interface ITimeZoneService
{
    /// <summary>
    /// Gets the timezone ID from the current HTTP request header with fallback to default
    /// </summary>
    /// <returns>Timezone ID from header or default (Europe/Warsaw)</returns>
    string GetTimeZoneFromHeader();

    /// <summary>
    /// Gets the TimeZoneInfo from the current HTTP request header with fallback to default
    /// </summary>
    /// <returns>TimeZoneInfo object for the detected timezone</returns>
    TimeZoneInfo GetTimeZoneInfoFromHeader();

    /// <summary>
    /// Validates and sanitizes a timezone ID from HTTP header
    /// </summary>
    /// <param name="timeZoneId">Raw timezone ID from header</param>
    /// <returns>Validated timezone ID or null if invalid</returns>
    string? ValidateTimeZoneFromHeader(string? timeZoneId);
}
