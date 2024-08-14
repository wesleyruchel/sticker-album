using APIStickerAlbum.Context;
using APIStickerAlbum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APIStickerAlbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly APIStickerAlbumDbContext _context;

        public AlbumsController(APIStickerAlbumDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> Get()
        {
            var albums = await _context.Albums
                .AsNoTracking()
                .Include(a => a.Stickers)
                .ToListAsync();

            if (albums.IsNullOrEmpty())
                return NotFound("Nenhum álbum encontrado.");

            return albums;
        }

        [HttpGet("{id}", Name = "GetAlbumById")]
        public ActionResult<Album> Get(int id)
        {
            var album = _context.Albums
                .AsNoTracking()
                .Include(a => a.Stickers)
                .FirstOrDefault(a => a.Id == id);

            if (album is null)
                return NotFound("Nenhum álbum encontrado com o parâmetro informado.");

            return album;
        }

        [HttpPost]
        public ActionResult Post(Album album)
        {
            if (album is null)
                return BadRequest("Dados inválidos.");

            _context.Albums.Add(album);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetAlbumById", new { id = album.Id }, album);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Album album)
        {
            if (id != album.Id)
                return BadRequest("Dados inválidos.");

            _context.Entry(album).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(album);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var album = _context.Albums
                .FirstOrDefault(a => a.Id == id);

            if (album is null)
                return BadRequest("Dados inválidos.");

            _context.Albums.Remove(album);
            _context.SaveChanges();

            return Ok(album);
        }
    }
}
