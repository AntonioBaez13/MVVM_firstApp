using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MVVM_firstApp.Models
{
    public class LotoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO implement the relationships between tables here 
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Loteria> Loteria { get; set; }
        public DbSet<Jugada> Jugada { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketJugada> TicketJugada { get; set; }

    }
}
