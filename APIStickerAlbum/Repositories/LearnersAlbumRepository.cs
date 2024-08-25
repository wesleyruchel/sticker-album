using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;

namespace APIStickerAlbum.Repositories;

public class LearnersAlbumRepository : Repository<LearnersAlbum>, ILearnersAlbumRepository
{
    public LearnersAlbumRepository(APIStickerAlbumDbContext context) : base(context)
    {

    }

    public bool Exists(LearnersAlbum learnerAlbum)
    {
        return _context.Set<LearnersAlbum>()
            .Any(la => la.UserId == learnerAlbum.UserId && la.AlbumId == learnerAlbum.AlbumId);
    }
}
