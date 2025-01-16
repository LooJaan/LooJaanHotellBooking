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
    public class TaiKhoannsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public TaiKhoannsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/TaiKhoanns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoann>>> GetTaiKhoans()
        {
            return await _context.TaiKhoans.ToListAsync();
        }

        // GET: api/TaiKhoanns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaiKhoann>> GetTaiKhoann(int id)
        {
            var taiKhoann = await _context.TaiKhoans.FindAsync(id);

            if (taiKhoann == null)
            {
                return NotFound();
            }

            return taiKhoann;
        }

        // PUT: api/TaiKhoanns/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaiKhoann(int id, TaiKhoann taiKhoann)
        {
            if (id != taiKhoann.ID)
            {
                return BadRequest();
            }

            _context.Entry(taiKhoann).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoannExists(id))
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

        // POST: api/TaiKhoanns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaiKhoann>> PostTaiKhoann(TaiKhoann taiKhoann)
        {
            _context.TaiKhoans.Add(taiKhoann);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaiKhoann", new { id = taiKhoann.ID }, taiKhoann);
        }

        // DELETE: api/TaiKhoanns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaiKhoann(int id)
        {
            var taiKhoann = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoann == null)
            {
                return NotFound();
            }

            _context.TaiKhoans.Remove(taiKhoann);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaiKhoannExists(int id)
        {
            return _context.TaiKhoans.Any(e => e.ID == id);
        }
    }
}
