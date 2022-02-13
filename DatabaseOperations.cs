using MVVM_firstApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVM_firstApp
{
    public class DatabaseOperations
    {
        private readonly LotoContext db;

        public DatabaseOperations()
        {
            db = new LotoContext();
        }

        public bool AddToDabataseAndPrint(IEnumerable<Combination> combinations, Loteria selectedLoteria)
        {
            string pin = CreateRandomPin(8);
            using (db)
            {

                Ticket ticket = new()
                {
                    PIN = pin,
                    Cancelled = false,
                    DateCreated = DateTime.Now
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
                    }
                    else if (jugada.Repeated + combination.Puntos > 5 || combination.Jugada.Length == 4)
                    {
                        //if the sum exceeds 5 (max) or is a combination of 2 numbers do not add to db
                        db.ChangeTracker.Clear();
                        return false;
                    }
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
                return true;
            }
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
    }
}
