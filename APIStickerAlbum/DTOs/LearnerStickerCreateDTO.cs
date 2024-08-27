using System.ComponentModel.DataAnnotations;

namespace APIStickerAlbum.DTOs;

public class LearnerStickerCreateDTO
{
    [Required]
    public string? ImageBase64 { get; set; }
    [Required]
    public int StickerId { get; set; }
}
