using System.ComponentModel.DataAnnotations;

namespace APIStickerAlbum.DTOs;

public class AlbumCreateDTO
{
    public string? ImageBase64 { get; set; }

    [Required]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool Community { get; set; }
}
