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
    public class WelComesController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public WelComesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/WelComes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WelCome>>> GetwelComes()
        {
            return await _context.welComes.ToListAsync();
        }

        // GET: api/WelComes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WelCome>> GetWelCome(int id)
        {
            var welCome = await _context.welComes.FindAsync(id);

            if (welCome == null)
            {
                return NotFound();
            }

            return welCome;
        }

        // PUT: api/WelComes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWelCome(int id, WelCome welCome)
        {
            if (id != welCome.ID)
            {
                return BadRequest();
            }

            _context.Entry(welCome).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WelComeExists(id))
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

        // POST: api/WelComes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WelCome>> PostWelCome(WelCome welCome)
        {
            _context.welComes.Add(welCome);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWelCome", new { id = welCome.ID }, welCome);
        }

        // DELETE: api/WelComes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWelCome(int id)
        {
            var welCome = await _context.welComes.FindAsync(id);
            if (welCome == null)
            {
                return NotFound();
            }

            _context.welComes.Remove(welCome);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WelComeExists(int id)
        {
            return _context.welComes.Any(e => e.ID == id);
        }
    }
}
