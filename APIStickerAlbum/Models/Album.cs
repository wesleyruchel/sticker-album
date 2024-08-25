using System.Text.Json.Serialization;

namespace APIStickerAlbum.Models;

public class Album
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Community { get; set; } = false;
    public bool Shared { get; set; } = false;
    public bool Blocked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   
    [JsonIgnore]
    public virtual ICollection<EducatorsAlbum> EducatorsAlbums { get; set; } = new List<EducatorsAlbum>();
    
    [JsonIgnore]
    public virtual ICollection<LearnersAlbum> LearnersAlbums { get; set; } = new List<LearnersAlbum>();

    [JsonIgnore]
    public virtual ICollection<Sticker> Stickers { get; set; } = new List<Sticker>();
}
