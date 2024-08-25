namespace APIStickerAlbum.Interfaces;

public interface IAlbumShareService
{
    string ShareAlbum(int albumId, int sharedByUserId);
}
