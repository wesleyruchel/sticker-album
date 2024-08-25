using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;

namespace APIStickerAlbum.Repositories;

public class AlbumShareRepository : Repository<AlbumShare>, IAlbumShareRepository
{
    public AlbumShareRepository(APIStickerAlbumDbContext context) : base(context)
    {

    }
}
