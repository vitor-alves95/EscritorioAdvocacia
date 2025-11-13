using Microsoft.AspNetCore.Identity;
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
    public class AdvogadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdvogadosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Advogados
        public async Task<IActionResult> Index()
        {
            var advogados = await _context.Advogados
                                        .Include(a => a.ApplicationUser)
                                        .ToListAsync();
            return View(advogados);
        }

        // GET: Advogados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advogado = await _context.Advogados
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advogado == null)
            {
                return NotFound();
            }

            return View(advogado);
        }

        // GET: Advogados/Create
        public IActionResult Create()
        {
            PopulateUsersDropdown();
            return View();
        }

        // POST: Advogados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,OAB,Email,ApplicationUserId")] Advogado advogado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advogado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateUsersDropdown();
            return View(advogado);
        }

        // GET: Advogados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advogado = await _context.Advogados.FindAsync(id);
            if (advogado == null)
            {
                return NotFound();
            }
            PopulateUsersDropdown();
            return View(advogado);
        }

        // POST: Advogados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,OAB,Email,ApplicationUserId")] Advogado advogado)
        {
            if (id != advogado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advogado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvogadoExists(advogado.Id))
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
            PopulateUsersDropdown();
            return View(advogado);
        }

        // GET: Advogados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advogado = await _context.Advogados
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advogado == null)
            {
                return NotFound();
            }

            return View(advogado);
        }

        // POST: Advogados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advogado = await _context.Advogados.FindAsync(id);
            if (advogado != null)
            {
                _context.Advogados.Remove(advogado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvogadoExists(int id)
        {
            return _context.Advogados.Any(e => e.Id == id);
        }

        private void PopulateUsersDropdown()
        {
            var users = _userManager.Users.OrderBy(u => u.UserName).ToList();
            ViewData["ApplicationUserId"] = new SelectList(users, "Id", "UserName");
        }
    } 
} 