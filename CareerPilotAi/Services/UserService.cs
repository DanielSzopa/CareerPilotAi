using CareerPilotAi.Application.Exceptions;
using System.Security.Claims;

namespace CareerPilotAi.Services;

internal class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserIdOrThrowException()
    {
        var userId = _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            throw new UserIdDoesNotExist();

        return userId;
    }
}
