using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.EntityFrameworkCore;

namespace APIStickerAlbum.Repositories;

public class AlbumRepository : Repository<Album>, IAlbumRepository
{
    public AlbumRepository(APIStickerAlbumDbContext context) : base(context)
    {

    }

    public Album? GetAlbumByAuthenticatedUser(int albumId, int userId, string userType)
    {
        if (userType.ToLower() == "educador")
            return _context.EducatorsAlbums
                    .Where(ea => ea.AlbumId == albumId && ea.UserId == userId)
                    .Select(ea => ea.Album).FirstOrDefault();

        return _context.LearnersAlbums
                   .Where(ea => ea.AlbumId == albumId && ea.UserId == userId)
                   .Select(ea => ea.Album).FirstOrDefault();
    }

    public IEnumerable<Album> GetAlbumsByAuthenticatedUser(int userId, string userType)
    {
        if (userType.ToLower() == "educador")
            return _context.EducatorsAlbums
                .Where(ea => ea.UserId == userId)
                .Select(ea => ea.Album)
                .ToList();

        return _context.LearnersAlbums
                .Where(ea => ea.UserId == userId)
                .Select(ea => ea.Album)
                .ToList();
    }

    public IEnumerable<Sticker> GetStickersAlbumByAlbumId(int albumId)
    {
        return _context.Stickers
            .Where(s => s.AlbumId == albumId)
            .ToList();
    }
}
