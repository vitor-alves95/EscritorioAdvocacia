using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Authorization;

namespace EscritorioAdvocacia.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProcessosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Processos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Processos
                .Include(p => p.AdvogadoResponsavel)
                .Include(p => p.Cliente)
                .Include(p => p.TipoProcesso)
                .Include(p => p.VaraOrigem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Processos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processos
                .Include(p => p.AdvogadoResponsavel)
                .Include(p => p.Cliente)
                .Include(p => p.TipoProcesso)
                .Include(p => p.VaraOrigem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // GET: Processos/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Processos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroUnificado,Titulo,StatusAndamento,DescricaoAndamento,DataAbertura,ClienteId,AdvogadoId,TipoProcessoId,VaraOrigemId")] Processo processo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns();
            return View(processo);
        }

        // GET: Processos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processos.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }
            PopulateDropdowns();
            return View(processo);
        }

        // POST: Processos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroUnificado,Titulo,StatusAndamento,DescricaoAndamento,DataAbertura,ClienteId,AdvogadoId,TipoProcessoId,VaraOrigemId")] Processo processo)
        {
            if (id != processo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoExists(processo.Id))
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
            PopulateDropdowns();
            return View(processo);
        }

        // GET: Processos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processos
                .Include(p => p.AdvogadoResponsavel)
                .Include(p => p.Cliente)
                .Include(p => p.TipoProcesso)
                .Include(p => p.VaraOrigem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // POST: Processos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var processo = await _context.Processos.FindAsync(id);
            if (processo != null)
            {
                _context.Processos.Remove(processo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessoExists(int id)
        {
            return _context.Processos.Any(e => e.Id == id);
        }

        
        private void PopulateDropdowns()
        {
            
            ViewData["AdvogadoId"] = new SelectList(_context.Advogados.OrderBy(a => a.Nome), "Id", "Nome");
            ViewData["ClienteId"] = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "Id", "Nome");
            ViewData["TipoProcessoId"] = new SelectList(_context.TiposProcessos.OrderBy(t => t.Nome), "Id", "Nome");
            ViewData["VaraOrigemId"] = new SelectList(_context.VarasOrigem.OrderBy(v => v.Nome), "Id", "Nome");
        }
    } 
} 