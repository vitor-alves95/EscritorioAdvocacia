using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EscritorioAdvocacia.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class PortalDoClienteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PortalDoClienteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<Cliente> GetClienteProfileAsync()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null) return null;

            var clienteProfile = await _context.Clientes
                .FirstOrDefaultAsync(c => c.ApplicationUserId == appUser.Id);

            return clienteProfile;
        }


        // GET: /PortalDoCliente/
        public async Task<IActionResult> Index()
        {
            var cliente = await GetClienteProfileAsync();
            if (cliente == null)
            {
                return View("AcessoNegado");
            }

            var processos = await _context.Processos
                .Where(p => p.ClienteId == cliente.Id)
                .Include(p => p.AdvogadoResponsavel)
                .Include(p => p.TipoProcesso) 
                .OrderByDescending(p => p.DataAbertura)
                .ToListAsync();

            return View(processos);
        }

        // GET: /PortalDoCliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await GetClienteProfileAsync();
            if (cliente == null) return View("AcessoNegado");

            var processo = await _context.Processos
                .Include(p => p.Cliente)
                .Include(p => p.TipoProcesso)
                .Include(p => p.VaraOrigem)
                .Include(p => p.AdvogadoResponsavel)
                .FirstOrDefaultAsync(p => p.Id == id && p.ClienteId == cliente.Id);

            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }
    }
}