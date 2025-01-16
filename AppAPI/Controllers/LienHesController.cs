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
    public class LienHesController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public LienHesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/LienHes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LienHe>>> GetlienHes()
        {
            return await _context.lienHes.ToListAsync();
        }

        // GET: api/LienHes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LienHe>> GetLienHe(int id)
        {
            var lienHe = await _context.lienHes.FindAsync(id);

            if (lienHe == null)
            {
                return NotFound();
            }

            return lienHe;
        }

        // PUT: api/LienHes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLienHe(int id, LienHe lienHe)
        {
            if (id != lienHe.ID)
            {
                return BadRequest();
            }

            _context.Entry(lienHe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LienHeExists(id))
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

        // POST: api/LienHes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LienHe>> PostLienHe(LienHe lienHe)
        {
            _context.lienHes.Add(lienHe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLienHe", new { id = lienHe.ID }, lienHe);
        }

        // DELETE: api/LienHes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLienHe(int id)
        {
            var lienHe = await _context.lienHes.FindAsync(id);
            if (lienHe == null)
            {
                return NotFound();
            }

            _context.lienHes.Remove(lienHe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LienHeExists(int id)
        {
            return _context.lienHes.Any(e => e.ID == id);
        }
    }
}
