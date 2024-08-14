using APIStickerAlbum.Models;
using System.Text.Json.Serialization;

namespace APIStickerAlbum.Models;

public class Sticker
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int AlbumId { get; set; }

    [JsonIgnore]
    public virtual Album Album { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<LearnersSticker> LearnersStickers { get; set; } = new List<LearnersSticker>();
}
