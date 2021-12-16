namespace MVVM_firstApp.Models
{
    public class TicketJugada
    {
        public int TicketId { get; set; }
        public int JugadaId { get; set; }
        public int Points { get; set; }

        public Ticket Ticket { get; set; }
        public Jugada Jugada { get; set; }
    }
}
