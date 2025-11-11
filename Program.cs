using EscritorioAdvocacia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexao")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages(); // Adiciona suporte para as p�ginas de login/registro

var app = builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        // Pega os gerenciadores de Pap�is e Usu�rios
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Nomes dos pap�is
        string[] roleNames = { "Administrador", "Advogado", "Cliente" };

        // Cria os pap�is se eles n�o existirem
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // --- Cria o usu�rio Admin padr�o ---
        var adminUserEmail = "admin@admin.com";
        var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

        if (adminUser == null)
        {
            ApplicationUser newAdmin = new ApplicationUser
            {
                UserName = adminUserEmail,
                Email = adminUserEmail,
                EmailConfirmed = true // Confirma o email direto
            };

            var result = await userManager.CreateAsync(newAdmin, "admin");

            if (result.Succeeded)
            {
                // Associa o usu�rio ao papel "Administrador"
                await userManager.AddToRoleAsync(newAdmin, "Administrador");
            }
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Ocorreu um erro ao semear o banco de dados.");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // <-- Adiciona o "Quem � voc�?"
app.UseAuthorization();  // <-- Adiciona o "O que voc� pode fazer?"

app.MapRazorPages(); // Mapeia as rotas de login (ex: /Account/Login)

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
