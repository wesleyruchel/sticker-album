using APIStickerAlbum.DTOs;
using APIStickerAlbum.Models;

namespace APIStickerAlbum.Interfaces;

public interface IAlbumShareRepository : IRepository<AlbumShare>
{
    Task<IEnumerable<AlbumsStickersToCorrectionDTO>> GetAlbumsStickersToCorrectionAsync(int sharedByUserId);
}
