using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using Microsoft.AspNetCore.Authorization;

namespace AppView.Areas.Admin.Controllers
{

	[Area("Admin")]
    public class WelComesController : Controller
    {
        private readonly HotelDbContext _context;

        public WelComesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: WelComes
        public async Task<IActionResult> Index()
        {
            return View(await _context.welComes.ToListAsync());
        }

        // GET: WelComes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var welCome = await _context.welComes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (welCome == null)
            {
                return NotFound();
            }

            return View(welCome);
        }

        // GET: WelComes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WelComes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Wel,NoiDung,TrangThai")] WelCome welCome)
        {
            if (ModelState.IsValid)
            {
                _context.Add(welCome);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(welCome);
        }

        // GET: WelComes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var welCome = await _context.welComes.FindAsync(id);
            if (welCome == null)
            {
                return NotFound();
            }
            return View(welCome);
        }

        // POST: WelComes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Wel,NoiDung,TrangThai")] WelCome welCome)
        {
            if (id != welCome.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(welCome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WelComeExists(welCome.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(welCome);
        }

        // GET: WelComes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var welCome = await _context.welComes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (welCome == null)
            {
                return NotFound();
            }

            return View(welCome);
        }

        // POST: WelComes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var welCome = await _context.welComes.FindAsync(id);
            if (welCome != null)
            {
                _context.welComes.Remove(welCome);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WelComeExists(int id)
        {
            return _context.welComes.Any(e => e.ID == id);
        }
    }
}
