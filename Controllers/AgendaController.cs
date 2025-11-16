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
    [Authorize(Roles = "Controladoria")]
    public class AgendaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Agenda
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Compromissos
                .Include(c => c.Advogado)
                .Include(c => c.Processo)
                .OrderBy(c => c.DataHoraInicio); // Ordena por data
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Agenda/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compromisso = await _context.Compromissos
                .Include(c => c.Advogado)
                .Include(c => c.Processo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (compromisso == null)
            {
                return NotFound();
            }

            return View(compromisso);
        }

        // GET: Agenda/Create
        public IActionResult Create()
        {
            ViewData["AdvogadoId"] = new SelectList(_context.Advogados, "Id", "Email");
            ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "NumeroUnificado");
            PopulateDropdowns();
            return View();
        }

        // POST: Agenda/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,DataHoraInicio,DataHoraFim,Local,AdvogadoId,ProcessoId")] Compromisso compromisso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compromisso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdvogadoId"] = new SelectList(_context.Advogados, "Id", "Email", compromisso.AdvogadoId);
            ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "NumeroUnificado", compromisso.ProcessoId);
            PopulateDropdowns(compromisso.ProcessoId, compromisso.AdvogadoId);
            return View(compromisso);
        }

        // GET: Agenda/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compromisso = await _context.Compromissos.FindAsync(id);
            if (compromisso == null)
            {
                return NotFound();
            }
            ViewData["AdvogadoId"] = new SelectList(_context.Advogados, "Id", "Email", compromisso.AdvogadoId);
            ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "NumeroUnificado", compromisso.ProcessoId);
            PopulateDropdowns(compromisso.ProcessoId, compromisso.AdvogadoId);
            return View(compromisso);
        }

        // POST: Agenda/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,DataHoraInicio,DataHoraFim,Local,AdvogadoId,ProcessoId")] Compromisso compromisso)
        {
            if (id != compromisso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compromisso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompromissoExists(compromisso.Id))
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
            ViewData["AdvogadoId"] = new SelectList(_context.Advogados, "Id", "Email", compromisso.AdvogadoId);
            ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "NumeroUnificado", compromisso.ProcessoId);
            PopulateDropdowns(compromisso.ProcessoId, compromisso.AdvogadoId);
            return View(compromisso);
        }

        // GET: Agenda/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compromisso = await _context.Compromissos
                .Include(c => c.Advogado)
                .Include(c => c.Processo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compromisso == null)
            {
                return NotFound();
            }

            return View(compromisso);
        }

        // POST: Agenda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compromisso = await _context.Compromissos.FindAsync(id);
            if (compromisso != null)
            {
                _context.Compromissos.Remove(compromisso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompromissoExists(int id)
        {
            return _context.Compromissos.Any(e => e.Id == id);
        }

        private void PopulateDropdowns(int? processoId = null, int? advogadoId = null)
        {
            ViewData["AdvogadoId"] = new SelectList(_context.Advogados.OrderBy(a => a.Nome), "Id", "Nome", advogadoId);

            ViewData["ProcessoId"] = new SelectList(_context.Processos.OrderBy(p => p.Titulo), "Id", "Titulo", processoId);
        }
    }
}
