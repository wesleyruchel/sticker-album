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

}
