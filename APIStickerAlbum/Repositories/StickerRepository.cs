using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;

namespace APIStickerAlbum.Repositories
{
    public class StickerRepository : Repository<Sticker>, IStickerRepository
    {
        public StickerRepository(APIStickerAlbumDbContext context) : base(context)
        {
            
        }
    }
}
