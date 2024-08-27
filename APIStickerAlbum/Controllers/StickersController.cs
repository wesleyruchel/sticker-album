using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIStickerAlbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StickersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StickersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sticker>>> Get()
        {
            var stickers = await _unitOfWork.StickerRepository.GetAllAsync();

            return Ok(stickers);
        }

        [HttpGet("{id}", Name = "GetStickerById")]
        public ActionResult<Sticker> Get(int id)
        {
            var sticker = _unitOfWork.StickerRepository.Get(s => s.Id == id);

            if (sticker is null)
                return NotFound("Nenhuma figurinha encontrada com o parâmetro informado.");

            return Ok(sticker);
        }

        [HttpPost]
        public ActionResult Post(Sticker sticker)
        {
            if (sticker is null)
                return BadRequest("Dados inválidos.");

            var created = _unitOfWork.StickerRepository.Create(sticker);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("GetStickerById", new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Sticker sticker)
        {
            if (id != sticker.Id)
                return BadRequest("Dados inválidos.");

            _unitOfWork.StickerRepository.Update(sticker);
            _unitOfWork.Commit();

            return Ok(sticker);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var sticker = _unitOfWork.StickerRepository.Get(s => s.Id == id);

            if (sticker is null)
                return BadRequest("Dados inválidos.");

            var deleted = _unitOfWork.StickerRepository.Delete(sticker);
            _unitOfWork.Commit();

            return Ok(deleted);
        }
    }
}
