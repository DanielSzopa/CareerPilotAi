using CareerPilotAi.Application.Extensions;

namespace CareerPilotAi.Models.JobApplication;

public class JobApplicationCardViewModel
{
    public Guid JobApplicationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public CardDate? CardDate { get; set; }

    public string Status { get; set; } = string.Empty;
}

public class CardDate
{
    public string Value { get; }

    /// <summary>
    /// Creates a CardDate with UTC DateTime (legacy constructor for backward compatibility)
    /// </summary>
    /// <param name="dateTime">UTC DateTime</param>
    public CardDate(DateTime dateTime)
    {
        if(dateTime == default)
        {
            throw new ArgumentException("DateTime cannot be default value.", nameof(dateTime));
        }

        // Assume UTC if not specified (for backward compatibility)
        if (dateTime.Kind == DateTimeKind.Unspecified)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        // If it's UTC, convert to Poland timezone as default
        if (dateTime.Kind == DateTimeKind.Utc)
        {
            var localDateTime = dateTime.ToUserTimeZone("Europe/Warsaw");
            Value = localDateTime.ToDisplayString();
        }
        else
        {
            Value = dateTime.ToDisplayString();
        }
    }

    /// <summary>
    /// Creates a CardDate with timezone conversion
    /// </summary>
    /// <param name="utcDateTime">UTC DateTime to convert</param>
    /// <param name="timeZoneId">Target timezone identifier</param>
    public CardDate(DateTime utcDateTime, string? timeZoneId)
    {
        if(utcDateTime == default)
        {
            throw new ArgumentException("DateTime cannot be default value.", nameof(utcDateTime));
        }

        // Ensure the DateTime is marked as UTC
        if (utcDateTime.Kind == DateTimeKind.Unspecified)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC format for timezone conversion.", nameof(utcDateTime));
        }

        try
        {
            var localDateTime = utcDateTime.ToUserTimeZone(timeZoneId);
            Value = localDateTime.ToDisplayString();
        }
        catch (Exception)
        {
            // Fallback to default display if conversion fails
            Value = utcDateTime.ToDisplayString();
        }
    }

    /// <summary>
    /// Creates a CardDate with timezone conversion using TimeZoneInfo
    /// </summary>
    /// <param name="utcDateTime">UTC DateTime to convert</param>
    /// <param name="timeZone">Target timezone info</param>
    public CardDate(DateTime utcDateTime, TimeZoneInfo timeZone)
    {
        if(utcDateTime == default)
        {
            throw new ArgumentException("DateTime cannot be default value.", nameof(utcDateTime));
        }

        if (timeZone == null)
        {
            throw new ArgumentNullException(nameof(timeZone));
        }

        // Ensure the DateTime is marked as UTC
        if (utcDateTime.Kind == DateTimeKind.Unspecified)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC format for timezone conversion.", nameof(utcDateTime));
        }

        try
        {
            var localDateTime = utcDateTime.ToUserTimeZone(timeZone);
            Value = localDateTime.ToDisplayString();
        }
        catch (Exception)
        {
            // Fallback to default display if conversion fails
            Value = utcDateTime.ToDisplayString();
        }
    }
}