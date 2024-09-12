using APIStickerAlbum.Models;

namespace APIStickerAlbum.Interfaces
{
    public interface ILearnersStickerRepository : IRepository<LearnersSticker> 
    {
        IEnumerable<LearnersSticker> GetStickersAlbumByAlbumId(int albumId);
    }
}
