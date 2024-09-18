namespace APIStickerAlbum.DTOs;

public class ApplicationUserProfileDTO
{
    public string? Type { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BornDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
