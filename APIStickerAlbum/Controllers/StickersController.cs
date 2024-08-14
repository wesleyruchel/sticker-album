using APIStickerAlbum.Context;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIStickerAlbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StickersController : ControllerBase
    {
        private readonly APIStickerAlbumDbContext _context;

        public StickersController(APIStickerAlbumDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sticker>>> Get()
        {
            var stickers = await _context.Stickers
                .AsNoTracking()
                .ToListAsync();

            if (stickers is null)
                return NotFound("Nenhuma figurinha encontrada.");

            return stickers;
        }

        [HttpGet("{id}", Name = "GetStickerById")]
        public ActionResult<Sticker> Get(int id)
        {
            var sticker = _context.Stickers
                .AsNoTracking()
                .FirstOrDefault(a => a.Id == id);

            if (sticker is null)
                return NotFound("Nenhuma figurinha encontrada com o parâmetro informado.");

            return sticker;
        }

        [HttpPost]
        public ActionResult Post(Sticker sticker)
        {
            if (sticker is null)
                return BadRequest("Dados inválidos.");

            _context.Stickers.Add(sticker);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetStickerById", new { id = sticker.Id }, sticker);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Sticker sticker)
        {
            if (id != sticker.Id)
                return BadRequest("Dados inválidos.");

            _context.Entry(sticker).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(sticker);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var sticker = _context.Stickers
                .FirstOrDefault(s => s.Id == id);

            if (sticker is null)
                return BadRequest("Dados inválidos.");

            _context.Stickers.Remove(sticker);
            _context.SaveChanges();

            return Ok(sticker);
        }
    }
}
