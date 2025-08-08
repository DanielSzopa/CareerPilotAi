using CareerPilotAi.Application.Helpers;
using Xunit;

namespace CareerPilotAi.Tests.Helpers;

public class TimeZoneHelperTests
{
    [Fact]
    public void IsValidTimeZoneId_ValidTimeZone_ReturnsTrue()
    {
        // Arrange
        const string validTimeZoneId = "Europe/Warsaw";

        // Act
        var result = TimeZoneHelper.IsValidTimeZoneId(validTimeZoneId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidTimeZoneId_InvalidTimeZone_ReturnsFalse()
    {
        // Arrange
        const string invalidTimeZoneId = "Invalid/TimeZone";

        // Act
        var result = TimeZoneHelper.IsValidTimeZoneId(invalidTimeZoneId);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValidTimeZoneId_NullOrEmpty_ReturnsFalse(string? timeZoneId)
    {
        // Act
        var result = TimeZoneHelper.IsValidTimeZoneId(timeZoneId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetTimeZoneInfo_ValidTimeZone_ReturnsCorrectTimeZone()
    {
        // Arrange
        const string timeZoneId = "UTC";

        // Act
        var result = TimeZoneHelper.GetTimeZoneInfo(timeZoneId);

        // Assert
        Assert.Equal("UTC", result.Id);
    }

    [Fact]
    public void GetTimeZoneInfo_InvalidTimeZone_ReturnsPolandTimeZone()
    {
        // Arrange
        const string invalidTimeZoneId = "Invalid/TimeZone";

        // Act
        var result = TimeZoneHelper.GetTimeZoneInfo(invalidTimeZoneId);

        // Assert
        Assert.Equal("Europe/Warsaw", result.Id);
    }

    [Fact]
    public void GetTimeZoneInfo_NullTimeZone_ReturnsPolandTimeZone()
    {
        // Act
        var result = TimeZoneHelper.GetTimeZoneInfo(null);

        // Assert
        Assert.Equal("Europe/Warsaw", result.Id);
    }

    [Fact]
    public void ConvertToTimeZone_ValidUtcDateTime_ConvertsCorrectly()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        const string timeZoneId = "UTC";

        // Act
        var result = TimeZoneHelper.ConvertToTimeZone(utcDateTime, timeZoneId);

        // Assert
        Assert.Equal(utcDateTime, result);
    }

    [Fact]
    public void ConvertToTimeZone_NonUtcDateTime_ThrowsArgumentException()
    {
        // Arrange
        var localDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Local);
        const string timeZoneId = "UTC";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => TimeZoneHelper.ConvertToTimeZone(localDateTime, timeZoneId));
    }

    [Fact]
    public void ConvertToTimeZone_WithTimeZoneInfo_ConvertsCorrectly()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        var timeZone = TimeZoneInfo.Utc;

        // Act
        var result = TimeZoneHelper.ConvertToTimeZone(utcDateTime, timeZone);

        // Assert
        Assert.Equal(utcDateTime, result);
    }

    [Fact]
    public void ConvertToTimeZone_WithNullTimeZoneInfo_ThrowsArgumentNullException()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TimeZoneHelper.ConvertToTimeZone(utcDateTime, (TimeZoneInfo)null!));
    }

    [Fact]
    public void DefaultTimeZoneId_ReturnsPolandTimeZone()
    {
        // Act
        var result = TimeZoneHelper.DefaultTimeZoneId;

        // Assert
        Assert.Equal("Europe/Warsaw", result);
    }

    [Fact]
    public void GetTimeZoneDisplayName_ValidTimeZone_ReturnsDisplayName()
    {
        // Arrange
        const string timeZoneId = "UTC";

        // Act
        var result = TimeZoneHelper.GetTimeZoneDisplayName(timeZoneId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData("Europe/Warsaw")]
    [InlineData("America/New_York")]
    [InlineData("Asia/Tokyo")]
    public void SanitizeTimeZoneId_ValidTimeZone_ReturnsTimeZone(string timeZoneId)
    {
        // Act
        var result = TimeZoneHelper.SanitizeTimeZoneId(timeZoneId);

        // Assert
        Assert.Equal(timeZoneId, result);
    }

    [Theory]
    [InlineData("../../../etc/passwd")]
    [InlineData("Invalid..TimeZone")]
    [InlineData("")]
    [InlineData("   ")]
    public void SanitizeTimeZoneId_InvalidTimeZone_ReturnsNull(string timeZoneId)
    {
        // Act
        var result = TimeZoneHelper.SanitizeTimeZoneId(timeZoneId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void SanitizeTimeZoneId_NullTimeZone_ReturnsNull()
    {
        // Act
        var result = TimeZoneHelper.SanitizeTimeZoneId(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void SanitizeTimeZoneId_TooLongTimeZone_ReturnsNull()
    {
        // Arrange
        var longTimeZoneId = new string('A', 101); // 101 characters

        // Act
        var result = TimeZoneHelper.SanitizeTimeZoneId(longTimeZoneId);

        // Assert
        Assert.Null(result);
    }
}
