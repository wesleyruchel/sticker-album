using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;

namespace APIStickerAlbum.Repositories;

public class LearnersStickerRepository : Repository<LearnersSticker>, ILearnersStickerRepository
{
    public LearnersStickerRepository(APIStickerAlbumDbContext context) : base(context)
    {

    }
}
