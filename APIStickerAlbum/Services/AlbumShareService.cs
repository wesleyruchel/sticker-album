using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;

namespace APIStickerAlbum.Services;

public class AlbumShareService : IAlbumShareService
{
    private readonly IUnitOfWork _unitOfWork;

    public AlbumShareService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private string GenerateShareCode()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 10);
    }

    public string ShareAlbum(int albumId, int sharedByUserId)
    {
        var album = _unitOfWork.AlbumRepository.Get(a => a.Id == albumId);

        if (album is null)
            throw new Exception("Álbum não encontrado");

        if (!album.Shared)
        {
            album.Shared = true;
            _unitOfWork.AlbumRepository.Update(album);
        }

        var shareCode = GenerateShareCode();
        var albumShare = new AlbumShare
        {
            AlbumId = albumId,
            SharedByUserId = sharedByUserId,
            ShareCode = shareCode,
            ShareAt = DateTime.UtcNow,
        };

        var created = _unitOfWork.AlbumShareRepository.Create(albumShare);
        _unitOfWork.Commit();

        return created.ShareCode;
    }
}
