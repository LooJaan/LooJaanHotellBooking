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
    public class DatPhongsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public DatPhongsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/DatPhongs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatPhong>>> GetDatPhongs()
        {
            return await _context.DatPhongs.ToListAsync();
        }

        // GET: api/DatPhongs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DatPhong>> GetDatPhong(int id)
        {
            var datPhong = await _context.DatPhongs.FindAsync(id);

            if (datPhong == null)
            {
                return NotFound();
            }

            return datPhong;
        }

        // PUT: api/DatPhongs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDatPhong(int id, DatPhong datPhong)
        {
            if (id != datPhong.ID)
            {
                return BadRequest();
            }

            _context.Entry(datPhong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatPhongExists(id))
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

        // POST: api/DatPhongs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DatPhong>> PostDatPhong(DatPhong datPhong)
        {
            _context.DatPhongs.Add(datPhong);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDatPhong", new { id = datPhong.ID }, datPhong);
        }

        // DELETE: api/DatPhongs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDatPhong(int id)
        {
            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong == null)
            {
                return NotFound();
            }

            _context.DatPhongs.Remove(datPhong);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DatPhongExists(int id)
        {
            return _context.DatPhongs.Any(e => e.ID == id);
        }
    }
}
