using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace APIStickerAlbum.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string Type { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BornDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    [JsonIgnore]
    public virtual ICollection<EducatorsAlbum> EducatorsAlbums { get; set; } = new List<EducatorsAlbum>();

    [JsonIgnore]
    public virtual ICollection<LearnersAlbum> LearnersAlbums { get; set; } = new List<LearnersAlbum>();

    [JsonIgnore]
    public virtual ICollection<LearnersSticker> LearnersStickers { get; set; } = new List<LearnersSticker>();
}
