using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnhChiTietsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public AnhChiTietsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/AnhChiTiets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnhChiTiet>>> GetAnhChiTiets()
        {
            return await _context.AnhChiTiets.ToListAsync();
        }

        // GET: api/AnhChiTiets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnhChiTiet>> GetAnhChiTiet(int id)
        {
            var anhChiTiet = await _context.AnhChiTiets.FindAsync(id);

            if (anhChiTiet == null)
            {
                return NotFound();
            }

            return anhChiTiet;
        }

        // PUT: api/AnhChiTiets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnhChiTiet(int id, AnhChiTiet anhChiTiet)
        {
            if (id != anhChiTiet.Id)
            {
                return BadRequest();
            }

            _context.Entry(anhChiTiet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnhChiTietExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AnhChiTiets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AnhChiTiet>> PostAnhChiTiet(AnhChiTiet anhChiTiet)
        {
            _context.AnhChiTiets.Add(anhChiTiet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnhChiTiet", new { id = anhChiTiet.Id }, anhChiTiet);
        }

        // DELETE: api/AnhChiTiets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnhChiTiet(int id)
        {
            var anhChiTiet = await _context.AnhChiTiets.FindAsync(id);
            if (anhChiTiet == null)
            {
                return NotFound();
            }

            _context.AnhChiTiets.Remove(anhChiTiet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnhChiTietExists(int id)
        {
            return _context.AnhChiTiets.Any(e => e.Id == id);
        }
    }
}
