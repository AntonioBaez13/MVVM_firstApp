using MVVM_firstApp.Models;
using MVVM_firstApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVM_firstApp
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly LotoContext _db;

        public DatabaseOperations(LotoContext db)
        {
            _db = db;
        }

        public (string trackPin, int trackTicketId) AddToDabataseAndPrint(IEnumerable<Combination> combinations, LoteriaViewModel selectedLoteria)
        {
            string pin = CreateRandomPin(8);

            Ticket ticket = new()
            {
                PIN = pin,
                Cancelled = false,
                DateCreated = DateTimeOffset.Now
            };
            _db.Ticket.Add(ticket);

            foreach (Combination combination in combinations)
            {
                Jugada jugada = _db.Jugada.Where(j => j.Number == combination.Jugada
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
                    _db.Jugada.Add(jugada);
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

                _db.TicketJugada.Add(ticketJugada);
            }

            _db.SaveChanges();
            return (pin, ticket.Id);
        }

        public string CreateRandomPin(int length)
        {
            Random random = new();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
            {
                s = string.Concat(s, random.Next(10).ToString());
            }
            return s;
        }

        public IEnumerable<LoteriaViewModel> GetAllLoterias()
        {
            return _db.Loteria.Select(x => new LoteriaViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public IEnumerable<Combination> GetCombinations(int ticketId)
        {
            List<Combination> copiOfTicket = _db.TicketJugada
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
            return _db.Ticket.Where(t => t.Id == ticketId).Select(x => x.DateCreated).FirstOrDefault();
        }

        public string GetLoteriaName(int ticketId)
        {
            return _db.TicketJugada.Where(t => t.TicketId == ticketId).Select(X => X.Jugada.Loteria.Name).FirstOrDefault();
        }

        public int GetSumOfValuesRepeated(DateTime date)
        {
            return _db.Jugada.Where(j => j.Date == date).Select(x => x.Repeated).Sum();
        }

        public List<TicketJugadaViewModel> GetTicketJugadaViewModel(DateTime date, string number, int loteriaId)
        {
            int jugadaId = _db.Jugada.Where(j => j.Date == date && j.Number == number && j.LoteriaId == loteriaId)
                .Select(x => x.Id).FirstOrDefault();
            List<TicketJugadaViewModel> ticketJugadaViewModel = _db.TicketJugada.Where(tj => tj.JugadaId == jugadaId)
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
