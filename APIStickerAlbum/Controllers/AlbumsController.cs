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
        private readonly IAlbumShareService _albumShareService;

        public AlbumsController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService,
            IAlbumShareService albumShareService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _albumShareService = albumShareService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> Get()
        {
            var albums = await _unitOfWork.AlbumRepository.GetAllAsync();

            return Ok(albums);
        }

        [HttpGet("{id}", Name = "GetAlbumById")]
        public ActionResult<Album> Get(int id)
        {
            var album = _unitOfWork.AlbumRepository.Get(a => a.Id == id);

            if (album is null)
                return NotFound("Nenhum álbum encontrado com o parâmetro informado.");

            return Ok(album);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Album album)
        {
            if (album is null)
                return BadRequest("Dados inválidos.");

            var user = await _currentUserService.GetCurrentUserAsync();

            // TO-DO :: Abstrair regra de negócio
            if (user is null || user.Type.ToLower() != "educador")
                return BadRequest("Usuário não encontrado ou autenticação inválida.");

            album.EducatorsAlbums = new List<EducatorsAlbum>
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

        [HttpPost]
        [Route("{id}/share")]
        public async Task<IActionResult> Share(int id)
        {
            var user = await _currentUserService.GetCurrentUserAsync();
            var shareCode = _albumShareService.ShareAlbum(id, user.Id);

            return Ok(new { ShareCode = shareCode, Message = "Compartilhado com sucesso" });
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Album album)
        {
            if (id != album.Id)
                return BadRequest("Dados inválidos.");

            _unitOfWork.AlbumRepository.Update(album);
            _unitOfWork.Commit();

            return Ok(album);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var album = _unitOfWork.AlbumRepository.Get(a => a.Id == id);

            if (album is null)
                return BadRequest("Dados inválidos.");

            var deleted = _unitOfWork.AlbumRepository.Delete(album);
            _unitOfWork.Commit();

            return Ok(deleted);
        }
    }
}
