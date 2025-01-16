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
    public class MenuItemsController : Controller
    {
        private readonly HotelDbContext _context;

        public MenuItemsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.Menu.ToListAsync());
        }

        // GET: MenuItems/Details/5
      /*  public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.Menu
                .FirstOrDefaultAsync(m => m.ID == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }*/

        // GET: MenuItems/Create
      /*  public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Item,Url,TenController,TrangThai,OrderIndex")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuItem);
        }*/

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.Menu.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Item,Url,TenController,TrangThai,OrderIndex")] MenuItem menuItem)
        {
            if (id != menuItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid) 
            {
                try
                {
                    // Chỉ cập nhật tên và trạng thái
                    var existingMenuItem = await _context.Menu.FindAsync(id);
                    if (existingMenuItem == null)
                    {
                        return NotFound();
                    }

                    existingMenuItem.Item = menuItem.Item;
                    existingMenuItem.TrangThai = menuItem.TrangThai;

                    _context.Update(existingMenuItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.ID))
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
            return View(menuItem);
        }


        // GET: MenuItems/Delete/5
       /* public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.Menu
                .FirstOrDefaultAsync(m => m.ID == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.Menu.FindAsync(id);
            if (menuItem != null)
            {
                _context.Menu.Remove(menuItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
*/
        private bool MenuItemExists(int id)
        {
            return _context.Menu.Any(e => e.ID == id);
        }
    }
}
