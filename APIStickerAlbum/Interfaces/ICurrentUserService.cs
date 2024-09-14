using APIStickerAlbum.Models;

namespace APIStickerAlbum.Interfaces;

public interface ICurrentUserService
{
    string GetUserId();
    string GetUserType();
    Task<ApplicationUser> GetCurrentUserAsync();
}
