using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexao")));

// 2. Configuração do Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Regras de senha (para desenvolvimento)
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3; // <-- ADICIONEI ISSO: Permite senhas curtas se você quiser
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// --- ÁREA DE SEED (CRIAÇÃO DE DADOS INICIAIS) ---
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // 1. Criar Papéis
        string[] roleNames = { "Administrador", "Advogado", "Cliente" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. Criar Admin
        var adminUserEmail = "admin@admin.com";
        var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

        if (adminUser == null)
        {
            ApplicationUser newAdmin = new ApplicationUser
            {
                UserName = adminUserEmail,
                Email = adminUserEmail,
                EmailConfirmed = true
            };

            
            var result = await userManager.CreateAsync(newAdmin, "admin123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Administrador");
                Console.WriteLine("USUARIO ADMIN CRIADO COM SUCESSO!"); // Mensagem no console
            }
            else
            {
                // Loga o erro exato se falhar
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"ERRO AO CRIAR ADMIN: {error.Description}");
                }
            }
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Ocorreu um erro crítico ao semear o banco de dados.");
}
// --- FIM DO SEED ---


// --- PIPELINE (A ordem aqui importa muito) ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Arquivos estáticos (CSS, JS) vêm antes do Roteamento

app.UseRouting(); // Roteamento UMA vez só

app.UseAuthentication(); // Quem é você?
app.UseAuthorization();  // O que você pode fazer?

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();