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

            modelBuilder.Entity<TicketJugada>()
                .HasKey(tj => new { tj.TicketId, tj.JugadaId });

            modelBuilder.Entity<TicketJugada>()
                .HasOne(t => t.Ticket)
                .WithMany(t => t.TicketJugada)
                .HasForeignKey(t => t.TicketId);

            modelBuilder.Entity<TicketJugada>()
                .HasOne(j => j.Jugada)
                .WithMany(j => j.TicketJugada)
                .HasForeignKey(j => j.JugadaId);

            modelBuilder.Entity<Jugada>()
                .HasOne(l => l.Loteria)
                .WithMany(l => l.Jugadas)
                .HasForeignKey(l => l.LoteriaId);
        }

        public DbSet<Loteria> Loteria { get; set; }
        public DbSet<Jugada> Jugada { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketJugada> TicketJugada { get; set; }

    }
}
