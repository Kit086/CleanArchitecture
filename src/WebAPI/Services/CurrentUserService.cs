using System.Security.Claims;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => GetUserId();

    private string? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return id;
    }
}
