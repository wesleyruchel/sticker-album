using APIStickerAlbum.Models;

namespace APIStickerAlbum.Interfaces;

public interface ILearnersAlbumRepository : IRepository<LearnersAlbum>
{
    bool Exists(LearnersAlbum learnerAlbum);
}
