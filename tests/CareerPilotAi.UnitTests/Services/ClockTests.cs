using CareerPilotAi.Application.Services;
using Shouldly;
using Xunit;

namespace CareerPilotAi.Tests.Services;

public class ClockTests
{
    private readonly IClock _clock = new Clock();

    [Theory]
    [InlineData(2024, 1, 15, 12, 0, 0, 12, 0)]
    [InlineData(2024, 7, 15, 12, 0, 0, 12, 0)]
    public void GetDateTimeAdjustedToTimeZone_ShouldReturnSameDateTime_ForUtcTimeZone(
        int year, int month, int day, int hour, int minute, int second, int expectedHour, int expectedMinute)
    {
        var utcDateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

        var result = _clock.GetDateTimeAdjustedToTimeZone(utcDateTime, "UTC");

        result.Year.ShouldBe(year);
        result.Month.ShouldBe(month);
        result.Day.ShouldBe(day);
        result.Hour.ShouldBe(expectedHour);
        result.Minute.ShouldBe(expectedMinute);
    }

    [Theory]
    [InlineData("Europe/Warsaw", 2024, 1, 15, 12, 0, 0, 13, 0)]
    [InlineData("Europe/London", 2024, 1, 15, 12, 0, 0, 12, 0)]
    [InlineData("America/New_York", 2024, 1, 15, 12, 0, 0, 7, 0)]
    [InlineData("Asia/Tokyo", 2024, 1, 15, 12, 0, 0, 21, 0)]
    [InlineData("Europe/Warsaw", 2024, 7, 15, 12, 0, 0, 14, 0)]
    [InlineData("Europe/London", 2024, 7, 15, 12, 0, 0, 13, 0)]
    [InlineData("America/New_York", 2024, 7, 15, 12, 0, 0, 8, 0)]
    [InlineData("Asia/Tokyo", 2024, 7, 15, 12, 0, 0, 21, 0)]
    public void GetDateTimeAdjustedToTimeZone_ShouldReturnCorrectlyAdjustedDateTime(
        string timeZoneId, int year, int month, int day, int hour, int minute, int second, int expectedHour, int expectedMinute)
    {
        var utcDateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

        var result = _clock.GetDateTimeAdjustedToTimeZone(utcDateTime, timeZoneId);

        result.Year.ShouldBe(year);
        result.Month.ShouldBe(month);
        result.Day.ShouldBe(day);
        result.Hour.ShouldBe(expectedHour);
        result.Minute.ShouldBe(expectedMinute);
    }

    [Theory]
    [InlineData("Invalid/TimeZone")]
    [InlineData("Not/A/Zone")]
    [InlineData("Fake/Location")]
    public void GetDateTimeAdjustedToTimeZone_ShouldThrowException_ForInvalidTimeZone(string invalidTimeZoneId)
    {
        var utcDateTime = new DateTime(2024, 10, 26, 12, 0, 0, DateTimeKind.Utc);

        var exception = Should.Throw<TimeZoneNotFoundException>(() =>
            _clock.GetDateTimeAdjustedToTimeZone(utcDateTime, invalidTimeZoneId));

        exception.Message.ShouldContain(invalidTimeZoneId);
    }
}
