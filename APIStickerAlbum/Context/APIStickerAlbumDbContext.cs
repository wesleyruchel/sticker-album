using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIStickerAlbum.Context;

public class APIStickerAlbumDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public APIStickerAlbumDbContext(DbContextOptions<APIStickerAlbumDbContext> options) : base(options)
    { }

    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<AlbumShare> AlbumsShares { get; set; }
    public virtual DbSet<EducatorsAlbum> EducatorsAlbums { get; set; }
    public virtual DbSet<LearnersAlbum> LearnersAlbums { get; set; }
    public virtual DbSet<LearnersSticker> LearnersStickers { get; set; }
    public virtual DbSet<Sticker> Stickers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AlbumsId");

            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime");
        });

        builder.Entity<AlbumShare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AlbumsSharesId");

            entity.Property(e => e.ShareAt)
                .HasColumnType("datetime");

        });

        builder.Entity<EducatorsAlbum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EducatorsAlbumsId");

            entity.HasIndex(e => new { e.AlbumId, e.UserId }, "UK_EducatorsAlbums_UserAlbum").IsUnique();

            entity.HasOne(d => d.Album).WithMany(p => p.EducatorsAlbums)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EducatorsAlbums_AlbumsId");

            entity.HasOne(d => d.User).WithMany(p => p.EducatorsAlbums)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EducatorsAlbums_UsersId");
        });

        builder.Entity<LearnersAlbum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LearnersAlbumsId");

            entity.HasIndex(e => new { e.AlbumId, e.UserId }, "UK_LearnersAlbums_UserAlbum").IsUnique();

            entity.HasOne(d => d.Album).WithMany(p => p.LearnersAlbums)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LearnersAlbums_AlbumsId");

            entity.HasOne(d => d.User).WithMany(p => p.LearnersAlbums)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LearnersAlbums_UsersId");
        });

        builder.Entity<LearnersSticker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LearnersStickersId");

            entity.HasIndex(e => new { e.UserId, e.StickerId }, "UK_LearnersStickers_UserSticker").IsUnique();

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Sticker).WithMany(p => p.LearnersStickers)
                .HasForeignKey(d => d.StickerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LearnersStickers_StickersId");

            entity.HasOne(d => d.User).WithMany(p => p.LearnersStickers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LearnersStickers_UsersId");
        });

        builder.Entity<Sticker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_StickersId");

            entity.Property(e => e.Description)
                .IsUnicode(false);

            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Album).WithMany(p => p.Stickers)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_stickers_albumsid");
        });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "Users");

            entity.HasKey(e => e.Id).HasName("PK_UsersId");

            entity.Property(e => e.BornDate)
                .HasColumnType("datetime");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime");

            entity.Property(e => e.Type).HasMaxLength(50).IsUnicode(false);

            entity.Property(e => e.FirstName).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(100).IsUnicode(false);
        });
    }
}
