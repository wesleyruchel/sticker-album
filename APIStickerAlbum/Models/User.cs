using APIStickerAlbum.Models;
using System.Text.Json.Serialization;

namespace APIStickerAlbum.Models;
public class User
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public string? Name { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime Created { get; set; }

    [JsonIgnore]
    public virtual ICollection<EducatorsAlbum> EducatorsAlbums { get; set; } = new List<EducatorsAlbum>();

    [JsonIgnore]
    public virtual ICollection<LearnersAlbum> LearnersAlbums { get; set; } = new List<LearnersAlbum>();

    [JsonIgnore]
    public virtual ICollection<LearnersSticker> LearnersStickers { get; set; } = new List<LearnersSticker>();
}
