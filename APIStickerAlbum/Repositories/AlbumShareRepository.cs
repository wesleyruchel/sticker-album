using APIStickerAlbum.Context;
using APIStickerAlbum.DTOs;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.EntityFrameworkCore;

namespace APIStickerAlbum.Repositories;

public class AlbumShareRepository : Repository<AlbumShare>, IAlbumShareRepository
{
    public AlbumShareRepository(APIStickerAlbumDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<AlbumsStickersToCorrectionDTO>> GetAlbumsStickersToCorrectionAsync(int sharedByUserId)
    {
        return await _context.AlbumsShares
            .Where(tp => tp.SharedByUserId == sharedByUserId)
            .Join(_context.EducatorsAlbums,
                  albumsShares => albumsShares.AlbumId,
                  educatorsAlbums => educatorsAlbums.AlbumId,
                  (albumsShares, educatorsAlbums) => new
                  {
                      albumsShares,
                      educatorsAlbums
                  })
            .Join(_context.Albums,
                resultSelector => resultSelector.educatorsAlbums.AlbumId,
                albums => albums.Id,
                (resultSelector, albums) => new
                {
                    resultSelector.albumsShares,
                    resultSelector.educatorsAlbums,
                    albums
                })
            .Join(_context.Stickers,
                  resultSelector => resultSelector.albums.Id,
                  stickers => stickers.AlbumId,
                  (resultSelector, stickers) => new
                  {
                      resultSelector.albumsShares,
                      resultSelector.educatorsAlbums,
                      resultSelector.albums,
                      stickers
                  })
            .Join(_context.LearnersStickers,
                  resultSelector => resultSelector.stickers.Id,
                  learnersStickers => learnersStickers.StickerId,
                  (resultSelector, learnersStickers) => new
                  {
                      resultSelector.albumsShares,
                      resultSelector.educatorsAlbums,
                      resultSelector.albums,
                      resultSelector.stickers,
                      learnersStickers
                  })
            .Join(_context.Users,
                  resultSelector => resultSelector.learnersStickers.UserId,
                  users => users.Id,
                  (resultSelector, users) => new AlbumsStickersToCorrectionDTO
                  {
                      UserName = users.UserName,
                      UserFirstName = users.FirstName,
                      AlbumId = resultSelector.albums.Id,
                      AlbumTitle = resultSelector.albums.Title,
                      UserStickerId = resultSelector.learnersStickers.Id,
                      StickerTitle = resultSelector.stickers.Title,
                      StickerDescription = resultSelector.stickers.Description,
                      Status = resultSelector.learnersStickers.Status,
                      ImageUrl = resultSelector.learnersStickers.ImageUrl,
                  })
            .ToListAsync();
    }
}
