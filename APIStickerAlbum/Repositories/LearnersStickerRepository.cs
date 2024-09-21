using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.EntityFrameworkCore;

namespace APIStickerAlbum.Repositories;

public class LearnersStickerRepository : Repository<LearnersSticker>, ILearnersStickerRepository
{
    public LearnersStickerRepository(APIStickerAlbumDbContext context) : base(context)
    {

    }

    public IEnumerable<LearnersSticker> GetStickersAlbumByAlbumId(int userId, int albumId)
    {
        return _context.LearnersStickers
            .Where(ls => ls.UserId == userId && ls.Sticker.AlbumId == albumId)
            .ToList();
    }
}
