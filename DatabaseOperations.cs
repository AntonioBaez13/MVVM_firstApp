using MVVM_firstApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVM_firstApp
{
    public class DatabaseOperations
    {
        private readonly LotoContext db = new LotoContext();
        public (string trackPin, int trackTicketId) AddToDabataseAndPrint(IEnumerable<Combination> combinations, Loteria selectedLoteria)
        {
            string pin = CreateRandomPin(8);

            Ticket ticket = new()
            {
                PIN = pin,
                Cancelled = false,
                DateCreated = DateTimeOffset.Now
            };
            db.Ticket.Add(ticket);

            foreach (Combination combination in combinations)
            {
                Jugada jugada = db.Jugada.Where(j => j.Number == combination.Jugada
                                        && j.LoteriaId == selectedLoteria.Id
                                        && j.Date == DateTime.UtcNow.Date).FirstOrDefault();

                if (jugada == null)
                {
                    //if jugada doesn't exist on the db add new one
                    jugada = new()
                    {
                        Number = combination.Jugada,
                        Repeated = combination.Puntos,
                        LoteriaId = selectedLoteria.Id,
                        Date = DateTime.Now,
                    };
                    db.Jugada.Add(jugada);
                } //TODO: add extra else-if statement if the sum exceeds 5 (max) or is a combination of 2 numbers do not add to db
                else
                {
                    //update only the repeated column
                    jugada.Repeated += combination.Puntos;
                }

                TicketJugada ticketJugada = new()
                {
                    Ticket = ticket,
                    Jugada = jugada,
                    Points = combination.Puntos
                };

                db.TicketJugada.Add(ticketJugada);
            }

            db.SaveChanges();
            return (pin, ticket.Id);
        }

        public string CreateRandomPin(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
            {
                s = String.Concat(s, random.Next(10).ToString());
            }
            return s;
        }

        public IEnumerable<Combination> GetCombinations (int ticketId)
        {
            List<Combination> copiOfTicket = db.TicketJugada
                .Where(t => t.TicketId == ticketId)
                .Select(x => new Combination()
                { 
                    Jugada = x.Jugada.Number, 
                    Puntos = x.Points 
                })
                .ToList();

            return copiOfTicket;
        }

        public DateTimeOffset GetDateCreated(int ticketId)
        {
            return db.Ticket.Where(t => t.Id == ticketId).Select(x => x.DateCreated).FirstOrDefault();
        }

        public string GetLoteriaName(int ticketId)
        {
            return db.TicketJugada.Where(t => t.TicketId == ticketId).Select(X => X.Jugada.Loteria.Name).FirstOrDefault();
        }
    }
}
