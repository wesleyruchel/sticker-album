namespace APIStickerAlbum.Interfaces;

public interface IUnitOfWork
{
    IAlbumRepository AlbumRepository { get; }
    IStickerRepository StickerRepository { get; }

    void Commit();
}
