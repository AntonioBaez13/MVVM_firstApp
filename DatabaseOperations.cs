using MVVM_firstApp.Models;
using MVVM_firstApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVM_firstApp
{
    public static class DatabaseOperations
    {
        private static readonly LotoContext db = new();

        public static (string trackPin, int trackTicketId) AddToDabataseAndPrint(IEnumerable<Combination> combinations, LoteriaViewModel selectedLoteria)
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
                        Date = DateTime.UtcNow.Date,
                    };
                    db.Jugada.Add(jugada);
                } //TODO: add extra else-if statement if the sum exceeds 5 (max) or is a combination of 2 numbers do not add to db
                else
                {
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

        public static string CreateRandomPin(int length)
        {
            Random random = new();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
            {
                s = string.Concat(s, random.Next(10).ToString());
            }
            return s;
        }

        public static IEnumerable<LoteriaViewModel> GetAllLoterias()
        {
            return db.Loteria.Select(x => new LoteriaViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public static IEnumerable<Combination> GetCombinations(int ticketId)
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

        public static DateTimeOffset GetDateCreated(int ticketId)
        {
            return db.Ticket.Where(t => t.Id == ticketId).Select(x => x.DateCreated).FirstOrDefault();
        }

        public static string GetLoteriaName(int ticketId)
        {
            return db.TicketJugada.Where(t => t.TicketId == ticketId).Select(X => X.Jugada.Loteria.Name).FirstOrDefault();
        }

        public static int GetSumOfValuesRepeated(DateTime date)
        {
            return db.Jugada.Where(j => j.Date == date).Select(x => x.Repeated).Sum();
        }

        public static List<TicketJugadaViewModel> GetTicketJugadaViewModel(DateTime date, string number, int loteriaId)
        {
            int jugadaId = db.Jugada.Where(j => j.Date == date && j.Number == number && j.LoteriaId == loteriaId)
                .Select(x => x.Id).FirstOrDefault();
            List<TicketJugadaViewModel> ticketJugadaViewModel = db.TicketJugada.Where(tj => tj.JugadaId == jugadaId)
                .Select(x => new TicketJugadaViewModel()
                {
                    TicketId = x.TicketId,
                    Points = x.Points
                })
                .ToList();

            return ticketJugadaViewModel;
        }
    }
}
