using CareerPilotAi.Models.JobApplication;
using Xunit;

namespace CareerPilotAi.Tests.Models.JobApplication;

public class CardDateTests
{
    [Fact]
    public void CardDate_ValidUtcDateTime_CreatesCardDateWithPolandTimeZone()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var cardDate = new CardDate(utcDateTime);

        // Assert
        Assert.NotNull(cardDate.Value);
        Assert.NotEmpty(cardDate.Value);
    }

    [Fact]
    public void CardDate_DefaultDateTime_ThrowsArgumentException()
    {
        // Arrange
        var defaultDateTime = default(DateTime);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CardDate(defaultDateTime));
    }

    [Fact]
    public void CardDate_WithTimeZoneId_CreatesCardDateWithSpecifiedTimeZone()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        const string timeZoneId = "UTC";

        // Act
        var cardDate = new CardDate(utcDateTime, timeZoneId);

        // Assert
        Assert.NotNull(cardDate.Value);
        Assert.NotEmpty(cardDate.Value);
        Assert.Equal("15.01.2025 12:00", cardDate.Value);
    }

    [Fact]
    public void CardDate_WithTimeZoneInfo_CreatesCardDateWithSpecifiedTimeZone()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        var timeZone = TimeZoneInfo.Utc;

        // Act
        var cardDate = new CardDate(utcDateTime, timeZone);

        // Assert
        Assert.NotNull(cardDate.Value);
        Assert.NotEmpty(cardDate.Value);
        Assert.Equal("15.01.2025 12:00", cardDate.Value);
    }

    [Fact]
    public void CardDate_WithNullTimeZoneInfo_ThrowsArgumentNullException()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CardDate(utcDateTime, (TimeZoneInfo)null!));
    }

    [Fact]
    public void CardDate_WithInvalidTimeZoneId_HandleGracefully()
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        const string invalidTimeZoneId = "Invalid/TimeZone";

        // Act
        var cardDate = new CardDate(utcDateTime, invalidTimeZoneId);

        // Assert
        Assert.NotNull(cardDate.Value);
        Assert.NotEmpty(cardDate.Value);
        // Should fallback to some valid display format
    }

    [Fact]
    public void CardDate_WithUnspecifiedDateTime_ConvertToUtc()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Unspecified);
        const string timeZoneId = "UTC";

        // Act
        var cardDate = new CardDate(unspecifiedDateTime, timeZoneId);

        // Assert
        Assert.NotNull(cardDate.Value);
        Assert.NotEmpty(cardDate.Value);
    }

    [Fact]
    public void CardDate_WithLocalDateTime_ThrowsArgumentException()
    {
        // Arrange
        var localDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Local);
        const string timeZoneId = "UTC";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CardDate(localDateTime, timeZoneId));
    }

    [Theory]
    [InlineData("Europe/Warsaw")]
    [InlineData("America/New_York")]
    [InlineData("Asia/Tokyo")]
    public void CardDate_WithVariousTimeZones_CreatesValidCardDate(string timeZoneId)
    {
        // Arrange
        var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var cardDate = new CardDate(utcDateTime, timeZoneId);

        // Assert
        Assert.NotNull(cardDate.Value);
        Assert.NotEmpty(cardDate.Value);
        Assert.Contains("15.01.2025", cardDate.Value);
    }
}
