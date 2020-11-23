using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Models;

namespace ProvaTecnica.Data
{
    public class ProvaTecnicaContext : DbContext
    {
        public ProvaTecnicaContext(DbContextOptions<ProvaTecnicaContext> options)
            : base(options)
        {
        }

        public DbSet<RegistrosPontos> RegistrosPontos { get; set; }
    }
}
