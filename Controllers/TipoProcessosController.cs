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
    public class TipoProcessosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoProcessosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoProcessos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposProcessos.ToListAsync());
        }

        // GET: TipoProcessos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoProcesso = await _context.TiposProcessos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoProcesso == null)
            {
                return NotFound();
            }

            return View(tipoProcesso);
        }

        // GET: TipoProcessos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoProcessos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] TipoProcesso tipoProcesso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoProcesso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoProcesso);
        }

        // GET: TipoProcessos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoProcesso = await _context.TiposProcessos.FindAsync(id);
            if (tipoProcesso == null)
            {
                return NotFound();
            }
            return View(tipoProcesso);
        }

        // POST: TipoProcessos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] TipoProcesso tipoProcesso)
        {
            if (id != tipoProcesso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoProcesso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoProcessoExists(tipoProcesso.Id))
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
            return View(tipoProcesso);
        }

        // GET: TipoProcessos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoProcesso = await _context.TiposProcessos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoProcesso == null)
            {
                return NotFound();
            }

            return View(tipoProcesso);
        }

        // POST: TipoProcessos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoProcesso = await _context.TiposProcessos.FindAsync(id);
            if (tipoProcesso != null)
            {
                _context.TiposProcessos.Remove(tipoProcesso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoProcessoExists(int id)
        {
            return _context.TiposProcessos.Any(e => e.Id == id);
        }
    }
}
