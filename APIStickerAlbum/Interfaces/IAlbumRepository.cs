using APIStickerAlbum.Models;

namespace APIStickerAlbum.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    Album? GetAlbumByAuthenticatedUser(int albumId, int userId, string userType);
    IEnumerable<Album> GetAlbumsByAuthenticatedUser(int userId, string userType);
    IEnumerable<Sticker> GetStickersAlbumByAlbumId(int albumId);
}
