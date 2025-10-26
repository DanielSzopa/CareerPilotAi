using System.Security.Claims;
using CareerPilotAi.Application.Exceptions;
using CareerPilotAi.Application.Services;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CareerPilotAi.Tests.Services;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserServiceTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _userService = new UserService(_httpContextAccessor);
    }

    [Fact]
    public void GetUserIdOrThrowException_ShouldReturnUserId_WhenUserIsAuthenticated()
    {
        var userId = "test-user-id";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = Substitute.For<HttpContext>();
        httpContext.User.Returns(principal);

        _httpContextAccessor.HttpContext.Returns(httpContext);

        var result = _userService.GetUserIdOrThrowException();

        result.ShouldBe(userId);
    }

    [Fact]
    public void GetUserIdOrThrowException_ShouldThrowUserIdDoesNotExist_WhenHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext.Returns((HttpContext?)null);

        var exception = Should.Throw<UserIdDoesNotExist>(() =>
            _userService.GetUserIdOrThrowException());

        exception.Message.ShouldBe("UserId doesn't exist. It's null or empty.");
    }

    [Fact]
    public void GetUserIdOrThrowException_ShouldThrowUserIdDoesNotExist_WhenUserIsNull()
    {
        var httpContext = Substitute.For<HttpContext>();
        httpContext.User.Returns((ClaimsPrincipal?)null);

        _httpContextAccessor.HttpContext.Returns(httpContext);

        var exception = Should.Throw<UserIdDoesNotExist>(() =>
            _userService.GetUserIdOrThrowException());

        exception.Message.ShouldBe("UserId doesn't exist. It's null or empty.");
    }

    [Fact]
    public void GetUserIdOrThrowException_ShouldThrowUserIdDoesNotExist_WhenNameIdentifierClaimIsMissing()
    {
        var claims = new[] { new Claim(ClaimTypes.Email, "test@example.com") };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = Substitute.For<HttpContext>();
        httpContext.User.Returns(principal);

        _httpContextAccessor.HttpContext.Returns(httpContext);

        var exception = Should.Throw<UserIdDoesNotExist>(() =>
            _userService.GetUserIdOrThrowException());

        exception.Message.ShouldBe("UserId doesn't exist. It's null or empty.");
    }

    [Fact]
    public void GetUserIdOrThrowException_ShouldThrowUserIdDoesNotExist_WhenNameIdentifierIsEmpty()
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "") };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = Substitute.For<HttpContext>();
        httpContext.User.Returns(principal);

        _httpContextAccessor.HttpContext.Returns(httpContext);

        var exception = Should.Throw<UserIdDoesNotExist>(() =>
            _userService.GetUserIdOrThrowException());

        exception.Message.ShouldBe("UserId doesn't exist. It's null or empty.");
    }
}
