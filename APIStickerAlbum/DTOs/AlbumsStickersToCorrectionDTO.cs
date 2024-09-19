namespace APIStickerAlbum.DTOs;

public class AlbumsStickersToCorrectionDTO
{
    public string? UserName { get; set; }
    public string? UserFirstName { get; set; }
    public int AlbumId { get; set; }
    public string? AlbumTitle { get; set; }
    public int UserStickerId { get; set; }
    public string? StickerTitle { get; set; }
    public string? StickerDescription { get; set; }
    public string? Status { get; set; }
    public string? ImageUrl { get; set; }
}
