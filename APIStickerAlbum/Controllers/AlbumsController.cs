using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIStickerAlbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AlbumsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public ActionResult Post(Album album)
        {
            if (album is null)
                return BadRequest("Dados inválidos.");

            var created = _unitOfWork.AlbumRepository.Create(album);
            
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("GetAlbumById", new { id = created.Id }, created);
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
