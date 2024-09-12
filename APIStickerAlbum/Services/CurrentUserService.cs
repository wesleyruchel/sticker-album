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
        #pragma warning disable CS8602 
        return _contextAccessor
                .HttpContext
                .User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;
        #pragma warning restore CS8602 
    }

    public string GetUserRole()
    {
        #pragma warning disable CS8602
        return _contextAccessor
                .HttpContext
                .User
                .FindFirst(ClaimTypes.Role)
                .Value;
        #pragma warning restore CS8602
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        #pragma warning disable CS8603
        return await _userManager.FindByIdAsync(GetUserId());
        #pragma warning restore CS8603
    }
}
