using APIStickerAlbum.Models;

namespace APIStickerAlbum.Interfaces;

public interface ICurrentUserService
{
    string GetUserId();
    Task<ApplicationUser> GetCurrentUserAsync();
}
