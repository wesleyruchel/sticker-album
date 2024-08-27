using APIStickerAlbum.DTOs;
using APIStickerAlbum.DTOs.Mappings;
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
    private readonly IAlbumShareService _albumShareService;
    private readonly IStorageService _storageService;

    public ApplicationUserController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService,
        IAlbumShareService albumShareService, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _albumShareService = albumShareService;
        _storageService = storageService;
    }

    [HttpGet]
    [Route("albums")]
    public async Task<ActionResult<IEnumerable<AlbumDetailsDTO>>> GetAlbums()
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        var albums = _unitOfWork.AlbumRepository.GetAlbumsByAuthenticatedUser(user.Id, user.Type);

        return Ok(albums.ToAlbumDetailsDTOList());
    }

    [HttpPost]
    [Route("albums/share/{id}")]
    public async Task<IActionResult> Share(int id)
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        var shareCode = _albumShareService.ShareAlbum(id, user.Id);

        return Ok(new { ShareCode = shareCode, Message = "Compartilhado com sucesso" });
    }

    [HttpGet]
    [Route("albums/shered/{shareCode}")]
    public async Task<ActionResult<AlbumDetailsDTO>> GetAlbumByShareCode(string shareCode)
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

        return Ok(album.ToAlbumDetailsDTO());
    }

    [HttpPost]
    [Route("albums/stickers")]
    public async Task<ActionResult<Sticker>> PostStickerAlbum(LearnerStickerCreateDTO learnerStickerCreateDTO)
    {
        if (learnerStickerCreateDTO is null)
            return BadRequest("Dados inválidos");

        var user = await _currentUserService.GetCurrentUserAsync();
        var learnerSticker = new LearnersSticker
        { 
            UserId = user.Id,
            StickerId = learnerStickerCreateDTO.StickerId
        };

        if (!string.IsNullOrEmpty(learnerStickerCreateDTO.ImageBase64))
        {
            var imageBytes = Convert.FromBase64String(learnerStickerCreateDTO.ImageBase64);
            var fileName = $"{Guid.NewGuid()}.jpg";

            learnerSticker.ImageUrl = await _storageService.UploadFileAsync(new MemoryStream(imageBytes), fileName, "image/jpeg");
        }

        var created = _unitOfWork.LearnersStickerRepository.Create(learnerSticker);
        _unitOfWork.Commit();

        return Ok(created);
    }
}
