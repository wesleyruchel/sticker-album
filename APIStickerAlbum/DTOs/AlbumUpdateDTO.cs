namespace APIStickerAlbum.DTOs;

public class AlbumUpdateDTO
{
    public int Id { get; set; }
    public string? ImageBase64 { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool Community { get; set; } = false;
    public bool Shared { get; set; } = false;
    public bool Blocked { get; set; } = false;
}
