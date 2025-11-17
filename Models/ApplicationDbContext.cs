using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EscritorioAdvocacia.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Advogado> Advogados { get; set; }
        public DbSet<Processo> Processos { get; set; }
        public DbSet<TipoProcesso> TiposProcessos { get; set; }
        public DbSet<VaraOrigem> VarasOrigem { get; set; }
        public DbSet<Compromisso> Compromissos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VaraOrigem>().HasData(
                new VaraOrigem { Id = 1, Nome = "1ª Vara Cível", Comarca = "São Paulo"},
                new VaraOrigem { Id = 2, Nome = "Vara de Família e Sucessões", Comarca = "Campinas"},
                new VaraOrigem { Id = 3, Nome = "Vara do Trabalho", Comarca = "Rio de Janeiro"},
                new VaraOrigem { Id = 4, Nome = "Juizado Especial Cível", Comarca = "Belo Horizonte" }
            );

        }
    }
}