using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        

        public DbSet<Rifa> Rifas { get; set; }  
        public DbSet<Participante> Participantes { get; set; }
        public DbSet<Premio> Premios { get; set; }

        public DbSet<RifaConParticipante> RifasConParticipantes { get; set; }
    }
}
