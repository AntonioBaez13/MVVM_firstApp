using MVVM_firstApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVM_firstApp
{
    public class DatabaseOperations
    {
        private readonly LotoContext db = new();

        public bool AddToDabataseAndPrint(ObservableCollection<Combination> combinations, Loteria selectedLoteria)
        {
            string pin = CreateRandomPin(8);

            using var transaction = db.Database.BeginTransaction();

            try
            {
                Ticket ticket = new()
                {
                    PIN = pin,
                    Cancelled = false,
                    DateCreated = DateTime.Now
                };
                db.Ticket.Add(ticket);
                db.SaveChanges();

                foreach (Combination combination in combinations)
                {
                    Jugada jugada = new();

                    var checkJugada = db.Jugada.Where(j => j.Number == combination.Jugada
                                            && j.LoteriaId == selectedLoteria.LoteriaId
                                            && j.Date == DateTime.UtcNow.Date).FirstOrDefault();

                    if (checkJugada == null)
                    {
                        //if jugada doesn't exist on the db add new one, else 
                        jugada = new()
                        {
                            Number = combination.Jugada,
                            Repeated = combination.Puntos,
                            LoteriaId = selectedLoteria.LoteriaId,
                            Date = DateTime.Now,
                        };
                        db.Jugada.Add(jugada);
                    }
                    else if (checkJugada.Repeated + combination.Puntos > 5 || combination.Jugada.Length == 4)
                    {
                        //if the sum exceeds 5 (max) or is a combination of 2 numbers
                        throw new Exception();
                    }
                    else
                    {
                        //update only the repeated column
                        checkJugada.Repeated += combination.Puntos;
                    }

                    db.SaveChanges();

                    TicketJugada ticketJugada = new()
                    {
                        TicketId = ticket.TicketId,
                        JugadaId = jugada.JugadaId.ToString() != null ? jugada.JugadaId : checkJugada.JugadaId,
                        Points = combination.Puntos
                    };

                    db.TicketJugada.Add(ticketJugada);
                }
                //TODO:
                db.SaveChanges();
                //Print the receipt
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public string CreateRandomPin(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i <length; i++)
            {
                s = String.Concat(s, random.Next(10).ToString());
            }
            return s;
        }
    }
}
