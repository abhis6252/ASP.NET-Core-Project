using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongNewApi.Data;
using SongNewApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SongNewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<ArtistController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Artists.Where(a=> !a.isRowDeleted).AsNoTracking().ToListAsync());
        }

        // GET api/<ArtistController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var artist = await _context.Artists.Where(a=> !a.isRowDeleted).AsNoTracking().FirstOrDefaultAsync(x => x.ArtistId == id);
            
            if(artist == null)
                return NotFound();
            else
                return Ok(artist);
        }
        [HttpGet("ByName/{name}")]

        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var song = await _context.Songs.Where(a => !a.isRowDeleted && a.ArtistName.Contains(name)).ToListAsync();

            if (song == null)
                return NotFound();
            else
                return Ok(song);
        }

        // POST api/<ArtistController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Artist artist)
        {
            var artistFromDb = await _context.Artists.AddAsync(artist);

            if (artistFromDb == null)
                return NotFound();
            else
            {
                await _context.SaveChangesAsync();
                return Ok("Artist Added");
            }
        }

        // PUT api/<ArtistController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Artist artist)
        {
            var artistFromDb = await _context.Artists.FirstOrDefaultAsync(x => x.ArtistId == id);

            if (artistFromDb == null)
                return NotFound();
            else
            {
                artistFromDb.ArtistName = artist.ArtistName;
                _context.Artists.Update(artistFromDb);
                await _context.SaveChangesAsync();
                return Ok("Artist Updated");
            }
        }

        // DELETE api/<ArtistController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var artistFromDb = await _context.Artists.FirstOrDefaultAsync(x => x.ArtistId == id);

            if (artistFromDb == null)
                return NotFound();
            else
            {
                artistFromDb.isRowDeleted = true;
                _context.Artists.Update(artistFromDb);
                await _context.SaveChangesAsync();
                return Ok("Artist Deleted");
            }
        }
    }
}
