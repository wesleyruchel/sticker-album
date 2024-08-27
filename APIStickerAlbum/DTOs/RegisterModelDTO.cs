using System.ComponentModel.DataAnnotations;

namespace APIStickerAlbum.DTOs;

public class RegisterModelDTO
{

    [Required(ErrorMessage = "O tipo do usuário é obrigatório")]
    public string? Type { get; set; }

    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    [Required(ErrorMessage = "O nome de usuário é obrigatório")]
    public string? Username {  get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "A confirmação da  senha é obrigatória")]
    [Compare("Password", ErrorMessage = "As senhas não conferem")]
    public string? ConfirmPassword { get; set; }
}
