using APIStickerAlbum.DTOs;
using APIStickerAlbum.DTOs.Mappings;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
    [Route("profile")]
    public async Task<ActionResult<ApplicationUserProfileDTO>> GetProfile()
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        return Ok(user.ToAppUserProfileDTO());
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
    public async Task<ActionResult> Share(int id)
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        var shareCode = _albumShareService.ShareAlbum(id, user.Id);

        return Ok(new { ShareCode = shareCode, Message = "Compartilhado com sucesso" });
    }

    [HttpPost]
    [Route("albums/shared/{shareCode}")]
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

    [HttpGet]
    [Route("albums/shared/correction/stickers/{allStickers:bool=false}")]
    public async Task<ActionResult<IEnumerable<AlbumsStickersToCorrectionDTO>>> GetSharedAlbumsWithStickersForCorrecation(bool allStickers)
    {
        var user = await _currentUserService.GetCurrentUserAsync();

        if (user is null || !user.Type.Equals("educador", StringComparison.CurrentCultureIgnoreCase))
            return Unauthorized();

        var result = await _unitOfWork.AlbumShareRepository.GetAlbumsStickersToCorrectionAsync(user.Id, allStickers);

        if (result.IsNullOrEmpty())
            return new List<AlbumsStickersToCorrectionDTO>();

        return Ok(result);
    }

    [HttpGet]
    [Route("albums/{id}/stickers")]
    public async Task<ActionResult<IEnumerable<LearnersStickersDTO>>> GetStickerAlbum(int id)
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        var learnersStickers = _unitOfWork.LearnersStickerRepository.GetStickersAlbumByAlbumId(user.Id, id);
        return Ok(learnersStickers.ToLearnersStickersDTOList());
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
            var fileName = $"{Guid.NewGuid()}.jpg";

            learnerSticker.ImageUrl = await _storageService.UploadFileAsync(learnerStickerCreateDTO.ImageBase64, fileName);
        }

        var exists = _unitOfWork.LearnersStickerRepository.Get(ls => ls.UserId == learnerSticker.UserId && ls.StickerId == learnerSticker.StickerId);

        if (exists is not null)
        {
            exists.ImageUrl = learnerSticker.ImageUrl;
            exists.Status = null;
            var result = _unitOfWork.LearnersStickerRepository.Update(exists);
            _unitOfWork.Commit();
            return Ok(result);
        }
        else
        {
            var result = _unitOfWork.LearnersStickerRepository.Create(learnerSticker);
            _unitOfWork.Commit();
            return Ok(result);
        }
    }

    [HttpPatch]
    [Route("albums/stickers/{id}")]
    public ActionResult UpdateStatusLearnerSticker(int id, JsonPatchDocument<LearnerStickerUpdateStatusDTO> patchStickerUpdateStatusDTO)
    {
        if (patchStickerUpdateStatusDTO is null || id < 1)
            return BadRequest();

        var learnerSticker = _unitOfWork.LearnersStickerRepository.Get(ls => ls.Id == id);

        if (learnerSticker is null)
            return NotFound();

        var learnerStickerUpdateStatus = new LearnerStickerUpdateStatusDTO
        {
            Status = learnerSticker.Status,
        };

        patchStickerUpdateStatusDTO.ApplyTo(learnerStickerUpdateStatus, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(learnerStickerUpdateStatus))
            return BadRequest(ModelState);

        learnerSticker.Status = learnerStickerUpdateStatus.Status;

        _unitOfWork.LearnersStickerRepository.Update(learnerSticker);
        _unitOfWork.Commit();

        return Ok();
    }
}
