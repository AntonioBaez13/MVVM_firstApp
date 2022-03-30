using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MVVM_firstApp.ViewModels
{
    public class ReportesViewModel : Screen
    {
        private readonly IDatabaseOperations _databaseOperations;
        private LoteriaViewModel _loteriaToSearch;
        private DateTime _selectedDate;
        private string _reporteText;

        public ObservableCollection<TicketJugadaViewModel> TicketPuntos { get; set; }
        public IEnumerable<LoteriaViewModel> Loterias { get; set; }
        public string JugadaToSearch { get; set; }
        public LoteriaViewModel LoteriaToSearch
        {
            get => _loteriaToSearch;
            set => SetAndNotify(ref _loteriaToSearch, value);
        }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetAndNotify(ref _selectedDate, value);
        }
        public string ReporteText
        {
            get => _reporteText;
            set => SetAndNotify(ref _reporteText, value);
        }

        public ReportesViewModel(IDatabaseOperations databaseOperations)
        {
            this.DisplayName = "Reportes";
            _databaseOperations = databaseOperations;
            TicketPuntos = new ObservableCollection<TicketJugadaViewModel>();
            SelectedDate = DateTime.Now.Date;
            Loterias = _databaseOperations.GetAllLoterias();
        }

        public void GetReporte()
        {
            int totalSum = _databaseOperations.GetSumOfValuesRepeated(SelectedDate);
            ReporteText = $"El {SelectedDate.DayOfWeek} {SelectedDate:D} se vendio un total de {totalSum} euros";
        }

        public void FindTicketsWithJugada()
        {
            //Check if I can achieve this using Guard Properties
            TicketPuntos.Clear();
            ReporteText = "";
            if (!string.IsNullOrEmpty(JugadaToSearch) && LoteriaToSearch != null)
            {
                List<TicketJugadaViewModel> ticketJugadaRecords = _databaseOperations.GetTicketJugadaViewModel(SelectedDate.Date, JugadaToSearch, LoteriaToSearch.Id);
                if (ticketJugadaRecords.Count == 0)
                {
                    ReporteText = $"El dia {SelectedDate:D} ningun ticket jugo la jugada especificada: ({JugadaToSearch}) en la ({LoteriaToSearch.Name})";
                }
                else
                {
                    foreach (TicketJugadaViewModel ticketPuntos in ticketJugadaRecords)
                    {
                        TicketPuntos.Add(ticketPuntos);
                    }
                }
            }
            else
            {
                ReporteText = "Por favor, asegurese de que ha especificado una fecha, una loteria y una jugada, antes de presionar el boton 'Buscar Tickets con Jugada'";
            }
        }
    }
}
