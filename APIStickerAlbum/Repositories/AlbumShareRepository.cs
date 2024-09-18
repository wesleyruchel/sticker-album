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
                  tabelaPrincipal => tabelaPrincipal.AlbumId,
                  tabelaB => tabelaB.AlbumId,
                  (tabelaPrincipal, tabelaB) => new { tabelaPrincipal, tabelaB })
            .Join(_context.Stickers,
                  tb => tb.tabelaB.AlbumId,
                  tabelaC => tabelaC.AlbumId,
                  (tb, tabelaC) => new { tb.tabelaPrincipal, tb.tabelaB, tabelaC })
            .Join(_context.LearnersStickers,
                  tc => tc.tabelaC.Id,
                  tabelaA => tabelaA.StickerId,
                  (tc, tabelaA) => new { tc.tabelaPrincipal, tc.tabelaB, tc.tabelaC, tabelaA })
            .Join(_context.Users,
                  ta => ta.tabelaA.UserId,
                  user => user.Id,
                  (ta, user) => new AlbumsStickersToCorrectionDTO
                  {
                      UserName = user.UserName,
                      UserFirstName = user.FirstName,
                      StickerId = ta.tabelaA.StickerId,
                      Status = ta.tabelaA.Status,
                      ImageUrl = ta.tabelaA.ImageUrl,
                  })
            .ToListAsync();
    }
}
