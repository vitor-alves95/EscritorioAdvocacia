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

    }
}