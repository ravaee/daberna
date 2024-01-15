using Microsoft.AspNetCore.Identity;

namespace Daberna.Services;

public class UserInfoAccessor
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfoAccessor(
        IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<IdentityUser> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

        return user;
    }
}