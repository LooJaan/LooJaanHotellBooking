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
    public class TinTucsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public TinTucsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/TinTucs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TinTuc>>> GettinTucs()
        {
            return await _context.tinTucs.ToListAsync();
        }

        // GET: api/TinTucs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TinTuc>> GetTinTuc(int id)
        {
            var tinTuc = await _context.tinTucs.FindAsync(id);

            if (tinTuc == null)
            {
                return NotFound();
            }

            return tinTuc;
        }

        // PUT: api/TinTucs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTinTuc(int id, TinTuc tinTuc)
        {
            if (id != tinTuc.ID)
            {
                return BadRequest();
            }

            _context.Entry(tinTuc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TinTucExists(id))
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

        // POST: api/TinTucs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TinTuc>> PostTinTuc(TinTuc tinTuc)
        {
            _context.tinTucs.Add(tinTuc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTinTuc", new { id = tinTuc.ID }, tinTuc);
        }

        // DELETE: api/TinTucs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTinTuc(int id)
        {
            var tinTuc = await _context.tinTucs.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            _context.tinTucs.Remove(tinTuc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TinTucExists(int id)
        {
            return _context.tinTucs.Any(e => e.ID == id);
        }
    }
}
