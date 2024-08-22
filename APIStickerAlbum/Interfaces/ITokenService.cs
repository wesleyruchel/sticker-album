using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIStickerAlbum.Interfaces;

public interface ITokenService
{
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
    string GenerateRefreshToken();
}
