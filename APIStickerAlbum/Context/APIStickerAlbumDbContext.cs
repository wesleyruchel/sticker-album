using APIStickerAlbum.Models;
using Microsoft.EntityFrameworkCore;

namespace APIStickerAlbum.Context;

public class APIStickerAlbumDbContext : DbContext
{
    public APIStickerAlbumDbContext(DbContextOptions<APIStickerAlbumDbContext> options) : base(options)
    { }

    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<EducatorsAlbum> EducatorsAlbums { get; set; }
    public virtual DbSet<LearnersAlbum> LearnersAlbums { get; set; }
    public virtual DbSet<LearnersSticker> LearnersStickers { get; set; }
    public virtual DbSet<Sticker> Stickers { get; set; }
    public virtual DbSet<User> Users { get; set; }
}