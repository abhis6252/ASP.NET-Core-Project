using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongNewApi.Data;
using SongNewApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SongNewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<SongController>
        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber=1, int pageSize=20)
        {
            return Ok(await _context.Songs.Skip((pageNumber-1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync());
        }

        // GET api/<SongController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var song = await _context.Songs.Include(s => s.Category).Include(s => s.Artist).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (song == null)
                return NotFound();
            else
                return Ok(song);
        }

        [HttpGet("ByName/{name}")]

        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var song = await _context.Songs.Where(s => s.ArtistName.Contains(name)).ToListAsync();

            if(song == null)
                return NotFound();
            else
                return Ok(song);
        }

        // POST api/<SongController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Song song)
        {
            var songFromDb = await _context.Songs.AddAsync(song);
            if (songFromDb == null)
                return NotFound();
            else
            {
                await _context.SaveChangesAsync();
                return Ok("Song Added");
            }
        }

        [HttpPost("UploadFile")]

        public async Task<IActionResult> UploadFile(int id, IFormFile file)

        {

            var songFromDb = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);

            if (songFromDb == null)

                return NotFound();

            else if (file == null || file.Length == 0)

                return BadRequest("No File Selected");

            else

            {

                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsFolderPath))

                {

                    Directory.CreateDirectory(uploadsFolderPath);

                }

                var fileName = Path.GetFileName(file.FileName);

                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))

                {

                    await file.CopyToAsync(stream);

                }

                songFromDb.FilePath = filePath;

                _context.Songs.Update(songFromDb);

                await _context.SaveChangesAsync();

                return Ok(new { filePath });
            }
        }

        // PUT api/<SongController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Song song)
        {
            var songFromDb = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);
            if (songFromDb == null)
                return NotFound();
            else
            {
                songFromDb.ArtistName = song.ArtistName;
                songFromDb.FilePath = song.FilePath;
                _context.Songs.Update(songFromDb);
                await _context.SaveChangesAsync();
                return Ok("Song Updated");
            }
        }

        // DELETE api/<SongController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var songFromDb = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);
            if (songFromDb == null)
                return NotFound();
            else
            {
                songFromDb.isRowDeleted = true;
                _context.Songs.Update(songFromDb);
                await _context.SaveChangesAsync();
                return Ok("Song Deleted");
            }
        }
    }
}