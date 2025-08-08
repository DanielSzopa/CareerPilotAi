using CareerPilotAi.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace CareerPilotAi.Tests.Services;

public class TimeZoneServiceTests
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TimeZoneService> _logger;
    private readonly TimeZoneService _timeZoneService;

    public TimeZoneServiceTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _logger = Substitute.For<ILogger<TimeZoneService>>();
        _timeZoneService = new TimeZoneService(_httpContextAccessor, _logger);
    }

    [Fact]
    public void GetTimeZoneFromHeader_ValidHeader_ReturnsTimeZone()
    {
        // Arrange
        var timeZoneId = "Europe/Warsaw";
        var context = CreateHttpContextWithHeader("x-timezone", timeZoneId);
        _httpContextAccessor.HttpContext.Returns(context);

        // Act
        var result = _timeZoneService.GetTimeZoneFromHeader();

        // Assert
        Assert.Equal(timeZoneId, result);
    }

    [Fact]
    public void GetTimeZoneFromHeader_InvalidHeader_ReturnsDefaultTimeZone()
    {
        // Arrange
        var invalidTimeZoneId = "Invalid/TimeZone";
        var context = CreateHttpContextWithHeader("x-timezone", invalidTimeZoneId);
        _httpContextAccessor.HttpContext.Returns(context);

        // Act
        var result = _timeZoneService.GetTimeZoneFromHeader();

        // Assert
        Assert.Equal("Europe/Warsaw", result);
    }

    [Fact]
    public void GetTimeZoneFromHeader_NoHeader_ReturnsDefaultTimeZone()
    {
        // Arrange
        var context = CreateHttpContextWithoutHeader();
        _httpContextAccessor.HttpContext.Returns(context);

        // Act
        var result = _timeZoneService.GetTimeZoneFromHeader();

        // Assert
        Assert.Equal("Europe/Warsaw", result);
    }

    [Fact]
    public void GetTimeZoneFromHeader_NullHttpContext_ReturnsDefaultTimeZone()
    {
        // Arrange
        _httpContextAccessor.HttpContext.Returns((HttpContext?)null);

        // Act
        var result = _timeZoneService.GetTimeZoneFromHeader();

        // Assert
        Assert.Equal("Europe/Warsaw", result);
    }

    [Fact]
    public void GetTimeZoneInfoFromHeader_ValidHeader_ReturnsCorrectTimeZoneInfo()
    {
        // Arrange
        var timeZoneId = "UTC";
        var context = CreateHttpContextWithHeader("x-timezone", timeZoneId);
        _httpContextAccessor.HttpContext.Returns(context);

        // Act
        var result = _timeZoneService.GetTimeZoneInfoFromHeader();

        // Assert
        Assert.Equal("UTC", result.Id);
    }

    [Fact]
    public void ValidateTimeZoneFromHeader_ValidTimeZone_ReturnsTimeZone()
    {
        // Arrange
        var timeZoneId = "Europe/Warsaw";

        // Act
        var result = _timeZoneService.ValidateTimeZoneFromHeader(timeZoneId);

        // Assert
        Assert.Equal(timeZoneId, result);
    }

    [Fact]
    public void ValidateTimeZoneFromHeader_InvalidTimeZone_ReturnsNull()
    {
        // Arrange
        var invalidTimeZoneId = "Invalid/TimeZone";

        // Act
        var result = _timeZoneService.ValidateTimeZoneFromHeader(invalidTimeZoneId);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateTimeZoneFromHeader_NullOrEmpty_ReturnsNull(string? timeZoneId)
    {
        // Act
        var result = _timeZoneService.ValidateTimeZoneFromHeader(timeZoneId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateTimeZoneFromHeader_DangerousInput_ReturnsNull()
    {
        // Arrange
        var dangerousInput = "../../../etc/passwd";

        // Act
        var result = _timeZoneService.ValidateTimeZoneFromHeader(dangerousInput);

        // Assert
        Assert.Null(result);
    }

    private static HttpContext CreateHttpContextWithHeader(string headerName, string headerValue)
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[headerName] = headerValue;
        return context;
    }

    private static HttpContext CreateHttpContextWithoutHeader()
    {
        var context = new DefaultHttpContext();
        return context;
    }
}
