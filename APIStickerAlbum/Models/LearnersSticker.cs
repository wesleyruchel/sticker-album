using System.Text.Json.Serialization;

namespace APIStickerAlbum.Models;

public class LearnersSticker
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int StickerId { get; set; }
    public string? Status { get; set; }
    public string? ImageUrl { get; set; }

    [JsonIgnore]
    public virtual Sticker Sticker { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
