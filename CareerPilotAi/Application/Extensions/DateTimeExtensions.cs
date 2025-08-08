using CareerPilotAi.Application.Helpers;

namespace CareerPilotAi.Application.Extensions;

/// <summary>
/// Extension methods for DateTime to support timezone conversions
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Converts a UTC DateTime to the specified user timezone
    /// </summary>
    /// <param name="utcDateTime">The UTC DateTime to convert</param>
    /// <param name="timeZoneId">The target timezone identifier</param>
    /// <returns>DateTime converted to the user's timezone</returns>
    /// <exception cref="ArgumentException">Thrown when DateTime is not in UTC format</exception>
    public static DateTime ToUserTimeZone(this DateTime utcDateTime, string? timeZoneId)
    {
        if (utcDateTime == default)
        {
            throw new ArgumentException("DateTime cannot be default value.", nameof(utcDateTime));
        }

        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC format for timezone conversion.", nameof(utcDateTime));
        }

        try
        {
            return TimeZoneHelper.ConvertToTimeZone(utcDateTime, timeZoneId);
        }
        catch (Exception)
        {
            // If conversion fails, fallback to default timezone
            return TimeZoneHelper.ConvertToTimeZone(utcDateTime, TimeZoneHelper.DefaultTimeZoneId);
        }
    }

    /// <summary>
    /// Converts a UTC DateTime to the specified user timezone using TimeZoneInfo
    /// </summary>
    /// <param name="utcDateTime">The UTC DateTime to convert</param>
    /// <param name="timeZone">The target timezone info</param>
    /// <returns>DateTime converted to the user's timezone</returns>
    /// <exception cref="ArgumentException">Thrown when DateTime is not in UTC format</exception>
    /// <exception cref="ArgumentNullException">Thrown when timeZone is null</exception>
    public static DateTime ToUserTimeZone(this DateTime utcDateTime, TimeZoneInfo timeZone)
    {
        if (utcDateTime == default)
        {
            throw new ArgumentException("DateTime cannot be default value.", nameof(utcDateTime));
        }

        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC format for timezone conversion.", nameof(utcDateTime));
        }

        if (timeZone == null)
        {
            throw new ArgumentNullException(nameof(timeZone));
        }

        try
        {
            return TimeZoneHelper.ConvertToTimeZone(utcDateTime, timeZone);
        }
        catch (Exception)
        {
            // If conversion fails, fallback to default timezone
            var defaultTimeZone = TimeZoneHelper.GetTimeZoneInfo(TimeZoneHelper.DefaultTimeZoneId);
            return TimeZoneHelper.ConvertToTimeZone(utcDateTime, defaultTimeZone);
        }
    }

    /// <summary>
    /// Formats a DateTime for display with timezone indication
    /// </summary>
    /// <param name="dateTime">The DateTime to format</param>
    /// <param name="format">Optional custom format string</param>
    /// <returns>Formatted string with timezone indication</returns>
    public static string ToDisplayString(this DateTime dateTime, string format = "dd.MM.yyyy HH:mm")
    {
        return dateTime.ToString(format);
    }

    /// <summary>
    /// Checks if a DateTime is in UTC format
    /// </summary>
    /// <param name="dateTime">The DateTime to check</param>
    /// <returns>True if the DateTime is in UTC, false otherwise</returns>
    public static bool IsUtc(this DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc;
    }
}
