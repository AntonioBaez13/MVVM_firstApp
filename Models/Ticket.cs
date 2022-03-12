using System;
using System.Collections.Generic;

namespace MVVM_firstApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string PIN { get; set; }
        public bool Cancelled { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public virtual ICollection<TicketJugada> TicketJugada { get; set; }
    }
}
