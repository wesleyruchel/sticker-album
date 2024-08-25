using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIStickerAlbum.Controllers;

[Authorize]
[Route("api/me")]
[ApiController]
public class ApplicationUserController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ApplicationUserController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [Route("albums")]
    public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        var albums = _unitOfWork.AlbumRepository.GetAlbumsByAuthenticatedUser(user.Id, user.Type);

        return Ok(albums);
    }

    [HttpGet]
    [Route("add/album/{shareCode}")]
    public async Task<IActionResult> GetAlbumByShareCode(string shareCode) 
    {
        var albumShare = _unitOfWork.AlbumShareRepository.Get(a => a.ShareCode == shareCode);

        if (albumShare is null)
            return NotFound("Código de compartilhamento inválido");

        var album = _unitOfWork.AlbumRepository.Get(a => a.Id == albumShare.AlbumId);

        if (album is null)
            return NotFound("Álbum não encontrado");

        var user = await _currentUserService.GetCurrentUserAsync();
        var learnerAlbum = new LearnersAlbum
        { 
            AlbumId = album.Id,
            UserId = user.Id
        };

        if (!_unitOfWork.LearnersAlbumRepository.Exists(learnerAlbum))
        {
            _unitOfWork.LearnersAlbumRepository.Create(learnerAlbum);
            _unitOfWork.Commit();
        }

        return Ok(album);
    }
}
