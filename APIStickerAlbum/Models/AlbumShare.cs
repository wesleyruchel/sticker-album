namespace APIStickerAlbum.Models;

public class AlbumShare
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public int SharedByUserId { get; set; }
    public string ShareCode { get; set; } = string.Empty;
    public DateTime ShareAt { get; set; }
}
