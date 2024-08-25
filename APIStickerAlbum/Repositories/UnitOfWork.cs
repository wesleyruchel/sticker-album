using APIStickerAlbum.Context;
using APIStickerAlbum.Interfaces;

namespace APIStickerAlbum.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IAlbumRepository? _albumRepository;
    private IAlbumShareRepository? _albumShareRepository;
    private IStickerRepository? _stickerRepository;
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

    public IAlbumShareRepository AlbumShareRepository
    {
        get { return _albumShareRepository = _albumShareRepository ?? new AlbumShareRepository(_context); }
    }

    public IStickerRepository StickerRepository
    {
        get { return _stickerRepository = _stickerRepository ?? new StickerRepository(_context); }
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
