using System.Text.Json.Serialization;

namespace APIStickerAlbum.Models;

public class Album
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public bool Community { get; set; }
    public bool Shared { get; set; }
    public bool Blocked { get; set; }
    public DateTime CreatedAt { get; set; }
   
    [JsonIgnore]
    public virtual ICollection<EducatorsAlbum> EducatorsAlbums { get; set; } = new List<EducatorsAlbum>();
    
    [JsonIgnore]
    public virtual ICollection<LearnersAlbum> LearnersAlbums { get; set; } = new List<LearnersAlbum>();

    [JsonIgnore]
    public virtual ICollection<Sticker> Stickers { get; set; } = new List<Sticker>();

    public Album()
    {
        this.CreatedAt = DateTime.UtcNow;
    }
}
