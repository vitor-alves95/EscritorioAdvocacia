using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Administrador")]
public class GerenciadorUsuariosController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public GerenciadorUsuariosController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

}