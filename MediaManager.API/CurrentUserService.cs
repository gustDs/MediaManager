using MediaManager.Application.Interfaces;

namespace MediaManager.API;

public class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get; }

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        var claim = accessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                 ?? accessor.HttpContext?.User.FindFirst("sub");
        UserId = claim is not null ? Guid.Parse(claim.Value) : Guid.Empty;
    }
}
