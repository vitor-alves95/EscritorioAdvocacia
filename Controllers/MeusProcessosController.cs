using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EscritorioAdvocacia.Controllers
{
   
    [Authorize(Roles = "Advogado")]
    public class MeusProcessosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

       
        public MeusProcessosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        private async Task<Advogado> GetAdvogadoProfileAsync()
        {
            
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null) return null;

            
            var advogadoProfile = await _context.Advogados
                .FirstOrDefaultAsync(a => a.ApplicationUserId == appUser.Id);

            return advogadoProfile;
        }


        // GET: /MeusProcessos/
        public async Task<IActionResult> Index()
        {
            var advogado = await GetAdvogadoProfileAsync();
            if (advogado == null)
            {
                
                return View("AcessoNegado");
            }

            
            var processos = await _context.Processos
                .Where(p => p.AdvogadoId == advogado.Id)
                .Include(p => p.Cliente) 
                .Include(p => p.TipoProcesso) 
                .OrderByDescending(p => p.DataAbertura)
                .ToListAsync();

            return View(processos);
        }

        // GET: /MeusProcessos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var advogado = await GetAdvogadoProfileAsync();
            if (advogado == null) return View("AcessoNegado");

            
            var processo = await _context.Processos
                .Include(p => p.Cliente)
                .Include(p => p.TipoProcesso)
                .Include(p => p.VaraOrigem)
                .Include(p => p.AdvogadoResponsavel)
                .FirstOrDefaultAsync(p => p.Id == id && p.AdvogadoId == advogado.Id);

            if (processo == null)
            {
                
                return NotFound();
            }

            return View(processo);
        }

        // GET: /MeusProcessos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var advogado = await GetAdvogadoProfileAsync();
            if (advogado == null) return View("AcessoNegado");

            
            var processo = await _context.Processos
                .FirstOrDefaultAsync(p => p.Id == id && p.AdvogadoId == advogado.Id);

            if (processo == null) return NotFound();

            return View(processo);
        }

        // POST: /MeusProcessos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StatusAndamento,DescricaoAndamento")] Processo processoInput)
        {
            if (id != processoInput.Id) return NotFound();

            var advogado = await GetAdvogadoProfileAsync();
            if (advogado == null) return View("AcessoNegado");

            
            var processoToUpdate = await _context.Processos
                .FirstOrDefaultAsync(p => p.Id == id && p.AdvogadoId == advogado.Id);

            if (processoToUpdate == null) return NotFound();

            if (ModelState.IsValid)
            {
                
                processoToUpdate.StatusAndamento = processoInput.StatusAndamento;
                processoToUpdate.DescricaoAndamento = processoInput.DescricaoAndamento;

                try
                {
                    _context.Update(processoToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Processos.Any(e => e.Id == processoToUpdate.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            
            return View(processoToUpdate);
        }
    }
}