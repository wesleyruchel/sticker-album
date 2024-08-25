using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace APIStickerAlbum.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public CurrentUserService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public string GetUserId()
    {
        return _contextAccessor
            .HttpContext?
            .User?.FindFirst(ClaimTypes.NameIdentifier)
            .Value!;
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        return await _userManager.FindByIdAsync(GetUserId());
    }
}
