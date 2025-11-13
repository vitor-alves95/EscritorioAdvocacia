using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscritorioAdvocacia.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class VarasOrigemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VarasOrigemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VarasOrigem
        public async Task<IActionResult> Index()
        {
            return View(await _context.VarasOrigem.ToListAsync());
        }

        // GET: VarasOrigem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varaOrigem = await _context.VarasOrigem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varaOrigem == null)
            {
                return NotFound();
            }

            return View(varaOrigem);
        }

        // GET: VarasOrigem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VarasOrigem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Comarca")] VaraOrigem varaOrigem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(varaOrigem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(varaOrigem);
        }

        // GET: VarasOrigem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varaOrigem = await _context.VarasOrigem.FindAsync(id);
            if (varaOrigem == null)
            {
                return NotFound();
            }
            return View(varaOrigem);
        }

        // POST: VarasOrigem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Comarca")] VaraOrigem varaOrigem)
        {
            if (id != varaOrigem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(varaOrigem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaraOrigemExists(varaOrigem.Id))
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
            return View(varaOrigem);
        }

        // GET: VarasOrigem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varaOrigem = await _context.VarasOrigem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varaOrigem == null)
            {
                return NotFound();
            }

            return View(varaOrigem);
        }

        // POST: VarasOrigem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var varaOrigem = await _context.VarasOrigem.FindAsync(id);
            if (varaOrigem != null)
            {
                _context.VarasOrigem.Remove(varaOrigem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VaraOrigemExists(int id)
        {
            return _context.VarasOrigem.Any(e => e.Id == id);
        }
    }
}
