namespace APIStickerAlbum.DTOs;

public class AlbumDetailsDTO
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool Community { get; set; } = false;
    public bool Shared { get; set; } = false;
    public bool Blocked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
