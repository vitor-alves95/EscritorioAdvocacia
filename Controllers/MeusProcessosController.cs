using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EscritorioAdvocacia.Controllers
{
    // 1. Tranca o controlador inteiro apenas para o "Advogado"
    [Authorize(Roles = "Advogado")]
    public class MeusProcessosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // 2. Injete o DbContext e o UserManager
        public MeusProcessosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 3. Método auxiliar para encontrar o Perfil de Advogado do login atual
        private async Task<Advogado> GetAdvogadoProfileAsync()
        {
            // Pega o usuário (login) atual
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null) return null;

            // Encontra o perfil de Advogado (com OAB, etc.) ligado a esse login
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
                // Se o Admin não associou o login a um perfil, ele não pode ver nada.
                return View("AcessoNegado");
            }

            // 4. A consulta mágica: Filtra processos ONDE AdvogadoId == o Id do perfil dele
            var processos = await _context.Processos
                .Where(p => p.AdvogadoId == advogado.Id)
                .Include(p => p.Cliente) // Inclui o nome do cliente
                .Include(p => p.TipoProcesso) // Inclui o tipo
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

            // 5. Segurança: Pega o processo SÓ SE o ID bater E o AdvogadoId for dele
            var processo = await _context.Processos
                .Include(p => p.Cliente)
                .Include(p => p.TipoProcesso)
                .Include(p => p.VaraOrigem)
                .Include(p => p.AdvogadoResponsavel)
                .FirstOrDefaultAsync(p => p.Id == id && p.AdvogadoId == advogado.Id);

            if (processo == null)
            {
                // Se não achou (ou não é dele), retorna NotFound
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

            // 6. Segurança: Busca o processo para edição SÓ SE for dele
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

            // 7. Busca o processo original do banco (para segurança)
            var processoToUpdate = await _context.Processos
                .FirstOrDefaultAsync(p => p.Id == id && p.AdvogadoId == advogado.Id);

            if (processoToUpdate == null) return NotFound();

            if (ModelState.IsValid)
            {
                // 8. Atualiza SÓ OS CAMPOS PERMITIDOS
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

            // Se o modelo for inválido, retorna para a View com o processo (carregado do banco)
            return View(processoToUpdate);
        }
    }
}