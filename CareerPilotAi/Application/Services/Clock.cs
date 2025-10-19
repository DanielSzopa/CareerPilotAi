namespace CareerPilotAi.Application.Services;

public interface IClock
{
    DateTime GetDateTimeAdjustedToTimeZone(DateTime dateTime, string timeZoneId);
}

public class Clock : IClock
{
    public DateTime GetDateTimeAdjustedToTimeZone(DateTime dateTime, string timeZoneId)
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);
    }
}