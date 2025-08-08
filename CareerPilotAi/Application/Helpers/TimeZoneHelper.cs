using System.Globalization;

namespace CareerPilotAi.Application.Helpers;

/// <summary>
/// Provides helper methods for timezone operations and validations
/// </summary>
public static class TimeZoneHelper
{
    private static readonly TimeZoneInfo PolandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");

    /// <summary>
    /// Default fallback timezone (Poland - Europe/Warsaw)
    /// </summary>
    public static string DefaultTimeZoneId => PolandTimeZone.Id;

    /// <summary>
    /// Validates if the provided timezone identifier is valid
    /// </summary>
    /// <param name="timeZoneId">The timezone identifier to validate</param>
    /// <returns>True if the timezone is valid, false otherwise</returns>
    public static bool IsValidTimeZoneId(string? timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
            return false;

        try
        {
            // Use TimeZoneInfo.FindSystemTimeZoneById to validate
            // This supports both Windows and IANA timezone IDs
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
        catch (InvalidTimeZoneException)
        {
            return false;
        }
    }

    /// <summary>
    /// Gets a TimeZoneInfo object for the specified timezone ID with fallback to Poland timezone
    /// </summary>
    /// <param name="timeZoneId">The timezone identifier</param>
    /// <returns>TimeZoneInfo object for the specified timezone or Poland timezone as fallback</returns>
    public static TimeZoneInfo GetTimeZoneInfo(string? timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId) || !IsValidTimeZoneId(timeZoneId))
        {
            return PolandTimeZone;
        }

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            return PolandTimeZone;
        }
        catch (InvalidTimeZoneException)
        {
            return PolandTimeZone;
        }
    }

    /// <summary>
    /// Converts a UTC DateTime to the specified timezone
    /// </summary>
    /// <param name="utcDateTime">UTC DateTime to convert</param>
    /// <param name="timeZoneId">Target timezone identifier</param>
    /// <returns>DateTime converted to the specified timezone</returns>
    public static DateTime ConvertToTimeZone(DateTime utcDateTime, string? timeZoneId)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC format", nameof(utcDateTime));
        }

        var targetTimeZone = GetTimeZoneInfo(timeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, targetTimeZone);
    }

    /// <summary>
    /// Converts a UTC DateTime to the specified timezone using TimeZoneInfo object
    /// </summary>
    /// <param name="utcDateTime">UTC DateTime to convert</param>
    /// <param name="timeZone">Target timezone info</param>
    /// <returns>DateTime converted to the specified timezone</returns>
    public static DateTime ConvertToTimeZone(DateTime utcDateTime, TimeZoneInfo timeZone)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC format", nameof(utcDateTime));
        }

        if (timeZone == null)
        {
            throw new ArgumentNullException(nameof(timeZone));
        }

        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
    }

    /// <summary>
    /// Gets the display name for a timezone
    /// </summary>
    /// <param name="timeZoneId">The timezone identifier</param>
    /// <returns>Display name of the timezone</returns>
    public static string GetTimeZoneDisplayName(string? timeZoneId)
    {
        var timeZone = GetTimeZoneInfo(timeZoneId);
        return timeZone.DisplayName;
    }

    /// <summary>
    /// Sanitizes and validates a timezone identifier from client input
    /// </summary>
    /// <param name="clientTimeZoneId">Timezone ID received from client</param>
    /// <returns>Sanitized and validated timezone ID or null if invalid</returns>
    public static string? SanitizeTimeZoneId(string? clientTimeZoneId)
    {
        if (string.IsNullOrWhiteSpace(clientTimeZoneId))
            return null;

        // Remove any potential dangerous characters and trim
        var sanitized = clientTimeZoneId.Trim();
        
        // Basic validation to prevent injection attacks
        if (sanitized.Length > 100 || sanitized.Contains(".."))
            return null;

        // Final validation - check if it's a valid timezone ID
        return IsValidTimeZoneId(sanitized) ? sanitized : null;
    }
}
