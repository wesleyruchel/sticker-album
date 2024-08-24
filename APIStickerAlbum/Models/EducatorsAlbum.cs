using System.Text.Json.Serialization;

namespace APIStickerAlbum.Models;

public class EducatorsAlbum
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AlbumId { get; set; }

    [JsonIgnore]
    public virtual Album Album { get; set; } = null!;

    [JsonIgnore]
    public virtual ApplicationUser User { get; set; } = null!;
}
