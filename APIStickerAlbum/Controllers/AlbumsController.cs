using APIStickerAlbum.DTOs;
using APIStickerAlbum.DTOs.Mappings;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIStickerAlbum.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStorageService _storageService;

        public AlbumsController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService,
            IStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumDetailsDTO>>> Get()
        {
            var albums = await _unitOfWork.AlbumRepository.GetAllAsync();

            return Ok(albums.ToAlbumDetailsDTOList());
        }

        [HttpGet("{id}", Name = "GetAlbumById")]
        public ActionResult<AlbumDetailsDTO> Get(int id)
        {
            var album = _unitOfWork.AlbumRepository.Get(a => a.Id == id);

            if (album is null)
                return NotFound("Nenhum álbum encontrado com o parâmetro informado.");

            return Ok(album.ToAlbumDetailsDTO());
        }

        [HttpGet]
        [Route("{id}/stickers")]
        public ActionResult<Sticker> GetStickers(int id) 
        {
            var stickers = _unitOfWork.AlbumRepository.GetStickersAlbumByAlbumId(id);

            if (stickers is null)
                return NotFound("Nenhuma figurinha encontrada para o álbum informado");

            return Ok(stickers);
        }

        [HttpPost]
        public async Task<ActionResult<AlbumDetailsDTO>> Post(AlbumCreateDTO albumCreateDTO)
        {
            if (albumCreateDTO is null)
                return BadRequest("Dados inválidos.");

            var user = await _currentUserService.GetCurrentUserAsync();
            var album = albumCreateDTO.ToAlbum();

            // TO-DO :: Abstrair regra de negócio
            if (user is null || user.Type.ToLower() != "educador")
                return BadRequest("Usuário não encontrado ou autenticação inválida.");

            if (!string.IsNullOrEmpty(albumCreateDTO.ImageBase64))
            {
                var imageBytes = Convert.FromBase64String(albumCreateDTO.ImageBase64);
                var fileName = $"{Guid.NewGuid()}.jpg";

                album!.ImageUrl = await _storageService.UploadFileAsync(new MemoryStream(imageBytes), fileName, "image/jpeg");
            }

            album!.EducatorsAlbums = new List<EducatorsAlbum>
            {
                new EducatorsAlbum
                {
                    Album = album,
                    User = user
                }
            };

            var created = _unitOfWork.AlbumRepository.Create(album);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("GetAlbumById", new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlbumDetailsDTO>> Put(int id, AlbumUpdateDTO albumUpdateDTO)
        {
            if (id != albumUpdateDTO.Id)
                return BadRequest("Dados inválidos.");

            var album = albumUpdateDTO.ToAlbum();

            if (!string.IsNullOrEmpty(albumUpdateDTO.ImageBase64))
            {
                var imageBytes = Convert.FromBase64String(albumUpdateDTO.ImageBase64);
                var fileName = $"{Guid.NewGuid()}.jpg";

                album!.ImageUrl = await _storageService.UploadFileAsync(new MemoryStream(imageBytes), fileName, "image/jpeg");
            }
            else
            {
                var albumOriginal = _unitOfWork.AlbumRepository.Get(a => a.Id == id);

                album.ImageUrl = albumOriginal.ImageUrl;
            }

            _unitOfWork.AlbumRepository.Update(album);
            _unitOfWork.Commit();

            return Ok(album);
        }

        [HttpDelete("{id}")]
        public ActionResult<AlbumDetailsDTO> Delete(int id)
        {
            var album = _unitOfWork.AlbumRepository.Get(a => a.Id == id);

            if (album is null)
                return BadRequest("Dados inválidos.");

            var deleted = _unitOfWork.AlbumRepository.Delete(album);
            _unitOfWork.Commit();

            return Ok(deleted.ToAlbumDetailsDTO());
        }
    }
}
