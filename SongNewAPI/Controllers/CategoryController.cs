using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongNewApi.Data;
using SongNewApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SongNewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Categories.Where(c=> !c.isRowDeleted).AsNoTracking().ToListAsync());
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)     
        {
            var category = await _context.Categories.Where(c=>!c.isRowDeleted).FirstOrDefaultAsync(x => x.CategoryId == id);

            if (category == null)
                return NotFound();
            else
                return Ok(category);
        }


        [HttpGet("ByName/{name}")]

        public async Task<IActionResult> GetByName(string name)

        {

            var category = await _context.Categories.Where(c => !c.isRowDeleted && c.CategoryName.Contains(name)).ToListAsync();

            if (category == null)

                return NotFound();

            else

                return Ok(category);

        }


        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            var categoryFromDb = await _context.Categories.AddAsync(category);

            if (categoryFromDb == null)
                return NotFound();
            else
            {
                await _context.SaveChangesAsync();
                return Ok("Category Added");
            }
        }


        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            var categoryFromDb = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (categoryFromDb == null)
                return NotFound();
            else
            {
                categoryFromDb.CategoryName = category.CategoryName;
                _context.Categories.Update(categoryFromDb);
                await _context.SaveChangesAsync();
                return Ok("Category Updated");
            }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryFromDb = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (categoryFromDb == null)
                return NotFound();
            else
            {
                categoryFromDb.isRowDeleted= true;
                _context.Categories.Update(categoryFromDb);
                await _context.SaveChangesAsync();
                return Ok("Category Deleted");
            }
        }

    }
}
