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
    public class LoGoesController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public LoGoesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/LoGoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoGo>>> GetloGos()
        {
            return await _context.loGos.ToListAsync();
        }

        // GET: api/LoGoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoGo>> GetLoGo(int id)
        {
            var loGo = await _context.loGos.FindAsync(id);

            if (loGo == null)
            {
                return NotFound();
            }

            return loGo;
        }

        // PUT: api/LoGoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoGo(int id, LoGo loGo)
        {
            if (id != loGo.ID)
            {
                return BadRequest();
            }

            _context.Entry(loGo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoGoExists(id))
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

        // POST: api/LoGoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoGo>> PostLoGo(LoGo loGo)
        {
            _context.loGos.Add(loGo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoGo", new { id = loGo.ID }, loGo);
        }

        // DELETE: api/LoGoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoGo(int id)
        {
            var loGo = await _context.loGos.FindAsync(id);
            if (loGo == null)
            {
                return NotFound();
            }

            _context.loGos.Remove(loGo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoGoExists(int id)
        {
            return _context.loGos.Any(e => e.ID == id);
        }
    }
}
