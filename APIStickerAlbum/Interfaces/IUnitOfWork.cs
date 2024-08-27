namespace APIStickerAlbum.Interfaces;

public interface IUnitOfWork
{
    IAlbumRepository AlbumRepository { get; }
    IAlbumShareRepository AlbumShareRepository { get; }
    IStickerRepository StickerRepository { get; }
    ILearnersAlbumRepository LearnersAlbumRepository { get; }
    ILearnersStickerRepository LearnersStickerRepository { get; }
    void Commit();
}
