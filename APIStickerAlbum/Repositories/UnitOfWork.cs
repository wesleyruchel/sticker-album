using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;

namespace APIStickerAlbum.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IAlbumRepository? _albumRepository;
    public APIStickerAlbumDbContext _context;

    public UnitOfWork(APIStickerAlbumDbContext context)
    {
        _context = context;
    }

    // Lazy Loading
    public IAlbumRepository AlbumRepository
    {
        get { return _albumRepository = _albumRepository ?? new AlbumRepository(_context); }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose() 
    {
        _context.Dispose();
    }
}
