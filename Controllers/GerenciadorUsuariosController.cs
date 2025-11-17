using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscritorioAdvocacia.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class GerenciadorUsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public GerenciadorUsuariosController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderBy(u => u.UserName).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> EditRoles(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var viewModel = new EditRolesViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Roles = new List<RoleViewModel>()
            };

            var allRoles = await _roleManager.Roles.ToListAsync();

            foreach (var role in allRoles)
            {
                var checkbox = new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,

                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                viewModel.Roles.Add(checkbox);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var oldRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, oldRoles);

            var newRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName);
            await _userManager.AddToRolesAsync(user, newRoles);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            //Não pode deixar o admin deletar a si mesmo
            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            var advogados = _context.Advogados.Where(a => a.ApplicationUserId == user.Id);
            foreach (var adv in advogados)
            {
                adv.ApplicationUserId = null;
            }

            var clientes = _context.Clientes.Where(c => c.ApplicationUserId == user.Id);
            foreach (var cli in clientes)
            {
                cli.ApplicationUserId = null;
            }

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Erro inesperado ao deletar usuário {user.Id}.");
            }

            return RedirectToAction(nameof(Index));
        }

    }
}