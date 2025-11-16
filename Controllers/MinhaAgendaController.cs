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
    public class MinhaAgendaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MinhaAgendaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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


        // GET: /MinhaAgenda/
        public async Task<IActionResult> Index()
        {
            var advogado = await GetAdvogadoProfileAsync();
            if (advogado == null)
            {
                return View("AcessoNegado");
            }

            var compromissos = await _context.Compromissos
                .Where(c => c.AdvogadoId == advogado.Id)
                .Include(c => c.Processo)
                .OrderBy(c => c.DataHoraInicio)
                .ToListAsync();

            return View(compromissos);
        }

        // GET: /MinhaAgenda/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var advogado = await GetAdvogadoProfileAsync();
            if (advogado == null) return View("AcessoNegado");

            var compromisso = await _context.Compromissos
                .Include(c => c.Advogado)
                .Include(c => c.Processo)
                .FirstOrDefaultAsync(c => c.Id == id && c.AdvogadoId == advogado.Id);

            if (compromisso == null)
            {
                return NotFound();
            }

            return View(compromisso);
        }
    }
}