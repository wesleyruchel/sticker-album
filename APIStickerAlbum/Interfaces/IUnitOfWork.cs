namespace APIStickerAlbum.Interfaces;

public interface IUnitOfWork
{
    IAlbumRepository AlbumRepository { get; }

    void Commit();
}
