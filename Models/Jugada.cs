using System;
using System.Collections.Generic;

namespace MVVM_firstApp.Models
{
    public class Jugada
    {
        public int JugadaId { get; set; }
        public string Number { get; set; }
        public int Repeated { get; set; }
        public int LoteriaId { get; set; }
        public DateTime? Date { get; set; }

        public Loteria Loteria { get; set; }
        public virtual ICollection<TicketJugada> TicketJugada { get; set; }

    }
}
