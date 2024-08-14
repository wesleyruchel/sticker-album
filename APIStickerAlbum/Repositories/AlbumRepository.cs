using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;

namespace APIStickerAlbum.Repositories;

public class AlbumRepository : Repository<Album>, IAlbumRepository
{
    public AlbumRepository(APIStickerAlbumDbContext context) : base(context)
    {

    }
}
