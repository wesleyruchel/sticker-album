using APIStickerAlbum.Models;

namespace APIStickerAlbum.DTOs.Mappings;

public static class ApplicationUserProfileDTOMappingExtensions
{
    public static ApplicationUser? ToAppUserProfile(this ApplicationUserProfileDTO applicationUserProfileDTO)
    {
        if(applicationUserProfileDTO is null)
            return null;

        #pragma warning disable CS8601
        return new ApplicationUser
        {
            Type = applicationUserProfileDTO.Type,
            UserName = applicationUserProfileDTO.Username,
            Email = applicationUserProfileDTO.Email,
            FirstName = applicationUserProfileDTO.FirstName,
            LastName = applicationUserProfileDTO.LastName,
            BornDate = applicationUserProfileDTO.BornDate,
            CreatedAt = applicationUserProfileDTO.CreatedAt
        };
        #pragma warning restore CS8601
    }
    
    public static ApplicationUserProfileDTO? ToAppUserProfileDTO(this ApplicationUser applicationUser)
    {
        if (applicationUser is null)
            return null;

        return new ApplicationUserProfileDTO
        {
            Type = applicationUser.Type,
            Username = applicationUser.UserName,
            Email = applicationUser.Email,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            BornDate = applicationUser.BornDate,
            CreatedAt = applicationUser.CreatedAt
        };
    }
}