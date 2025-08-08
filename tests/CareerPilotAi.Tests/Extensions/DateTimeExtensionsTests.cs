using CareerPilotAi.Application.Extensions;
using Xunit;

namespace CareerPilotAi.Tests.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void ToUserTimeZone_ValidUtcDateTime_ConvertsToSpecifiedTimeZone()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        const string timeZoneId = "UTC";

        // Act
        var result = utcDateTime.ToUserTimeZone(timeZoneId);

        // Assert
        Assert.Equal(utcDateTime, result);
    }

    [Fact]
    public void ToUserTimeZone_NonUtcDateTime_ThrowsArgumentException()
    {
        // Arrange
        var localDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Local);
        const string timeZoneId = "UTC";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => localDateTime.ToUserTimeZone(timeZoneId));
    }

    [Fact]
    public void ToUserTimeZone_DefaultDateTime_ThrowsArgumentException()
    {
        // Arrange
        var defaultDateTime = default(DateTime);
        const string timeZoneId = "UTC";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => defaultDateTime.ToUserTimeZone(timeZoneId));
    }

    [Fact]
    public void ToUserTimeZone_WithTimeZoneInfo_ConvertsCorrectly()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        var timeZone = TimeZoneInfo.Utc;

        // Act
        var result = utcDateTime.ToUserTimeZone(timeZone);

        // Assert
        Assert.Equal(utcDateTime, result);
    }

    [Fact]
    public void ToUserTimeZone_WithNullTimeZoneInfo_ThrowsArgumentNullException()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => utcDateTime.ToUserTimeZone((TimeZoneInfo)null!));
    }

    [Fact]
    public void ToUserTimeZone_WithInvalidTimeZone_FallsBackToDefault()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        const string invalidTimeZoneId = "Invalid/TimeZone";

        // Act
        var result = utcDateTime.ToUserTimeZone(invalidTimeZoneId);

        // Assert
        // Should not throw exception and should return some valid DateTime
        Assert.NotEqual(default(DateTime), result);
    }

    [Fact]
    public void ToDisplayString_ValidDateTime_ReturnsFormattedString()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 15, 14, 30, 0);

        // Act
        var result = dateTime.ToDisplayString();

        // Assert
        Assert.Equal("15.01.2025 14:30", result);
    }

    [Fact]
    public void ToDisplayString_WithCustomFormat_ReturnsCustomFormattedString()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 15, 14, 30, 0);
        const string customFormat = "yyyy-MM-dd HH:mm";

        // Act
        var result = dateTime.ToDisplayString(customFormat);

        // Assert
        Assert.Equal("2025-01-15 14:30", result);
    }

    [Fact]
    public void IsUtc_UtcDateTime_ReturnsTrue()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var result = utcDateTime.IsUtc();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUtc_LocalDateTime_ReturnsFalse()
    {
        // Arrange
        var localDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Local);

        // Act
        var result = localDateTime.IsUtc();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsUtc_UnspecifiedDateTime_ReturnsFalse()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Unspecified);

        // Act
        var result = unspecifiedDateTime.IsUtc();

        // Assert
        Assert.False(result);
    }
}
