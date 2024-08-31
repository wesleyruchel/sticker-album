using APIStickerAlbum.DTOs;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIStickerAlbum.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole<int>> roleManager,
                            IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("sign-in")]
    public async Task<IActionResult> Login(LoginModelDTO modelDTO)
    {
        var user = await _userManager.FindByNameAsync(modelDTO.Username!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, modelDTO.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.FirstName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityMinutes"], out int RefreshTokenValidityMinutes);

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(RefreshTokenValidityMinutes);
            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }

    [HttpPost]
    [Route("sign-up")]
    public async Task<IActionResult> Register(RegisterModelDTO modelDTO)
    {
        var userExists = await _userManager.FindByNameAsync(modelDTO.Username!);

        if (userExists is not null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseDTO { Status = "Erro", Message = "Usuário já cadastrado" });
        }

        ApplicationUser user = new()
        {
            Type = modelDTO.Type!,
            FirstName = modelDTO.FirstName!,
            LastName = modelDTO.LastName!,
            BornDate = modelDTO.BornDate,
            Email = modelDTO.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDTO.Username
        };

        var result = await _userManager.CreateAsync(user, modelDTO.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseDTO { Status = "Erro", Message = "Ocorreu um erro ao criar o usuário", Details = result.ToString()});
        }

        return Ok(new ResponseDTO { Status = "Sucesso", Message = "Usuário criado com sucesso" });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModelDTO modelDTO)
    {
        if (modelDTO is null)
        {
            return BadRequest("Requisição cliente inválida");
        }

        string? acessToken = modelDTO.AcessToken ?? throw new ArgumentNullException(nameof(modelDTO));
        string? refreshToken = modelDTO.RefreshToken ?? throw new ArgumentNullException(nameof(modelDTO));

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);

        if (principal is null)
        {
            return BadRequest("Token de acesso inválido");
        }

        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username!);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Token de acesso inválido");
        }

        var newAcessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
            refreshToken = newRefreshToken
        });
    }
}
