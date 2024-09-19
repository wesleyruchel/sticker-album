using APIStickerAlbum.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace APIStickerAlbum.DTOs;

public class LearnerStickerUpdateStatusDTO
{
    [Required]
    [EnumDataType(typeof(StickerStatusEnum), ErrorMessage = "O status deve ser Aprovada ou Reprovada")]
    public string? Status { get; set; }
}
