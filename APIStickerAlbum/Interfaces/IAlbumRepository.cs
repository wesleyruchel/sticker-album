using APIStickerAlbum.Models;

namespace APIStickerAlbum.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    IEnumerable<Album> GetAlbumsByAuthenticatedUser(int userId, string userType);
    IEnumerable<Sticker> GetStickersAlbumByAlbumId(int albumId);
}
