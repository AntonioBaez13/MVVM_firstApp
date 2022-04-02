﻿using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MVVM_firstApp.ViewModels
{
    public class VentasViewModel : Screen
    {
        private readonly IDatabaseOperations _databaseOperations;
        private LoteriaViewModel _selectedLoteria;
        private int _selectedTicket;
        private FullTicketData _completeTicketInfo;
        private Combination _selectedCombination;
        private string _puntosTotalSum;
        private int _ticketToCopy;

        private double TotalSum { get; set; }
        public IEnumerable<LoteriaViewModel> Loterias { get; set; }
        public ObservableCollection<int> TicketsFromToday { get; set; }
        public ObservableCollection<Combination> Combinations { get; set; }

        public LoteriaViewModel SelectedLoteria
        {
            get => _selectedLoteria;
            set => SetAndNotify(ref _selectedLoteria, value);
        }
        public int SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                if (SetAndNotify(ref _selectedTicket, value))
                {
                    RetrieveTicketInfo();
                }
            }
        }
        public FullTicketData CompleteTicketInfo
        {
            get => _completeTicketInfo;
            set => SetAndNotify(ref _completeTicketInfo, value);
        }
        public Combination SelectedCombination
        {
            get => _selectedCombination;
            set => SetAndNotify(ref _selectedCombination, value);
        }
        public string PuntosTotalSum
        {
            get => _puntosTotalSum;
            set => SetAndNotify(ref _puntosTotalSum, value);
        }
        public int TicketToCopy
        {
            get => _ticketToCopy;
            set => SetAndNotify(ref _ticketToCopy, value);
        }

        public VentasViewModel(IDatabaseOperations databaseOperations)
        {
            this.DisplayName = "Ventas";
            _databaseOperations = databaseOperations;
            Combinations = new ObservableCollection<Combination>();
            TicketsFromToday = new ObservableCollection<int>();
            Loterias = _databaseOperations.GetAllLoterias();
        }

        public void AddToCollection(int puntos, string jugada)
        {
            if (Combinations.Any(x => x.Jugada == jugada))
            {
                var record = Combinations.FirstOrDefault(x => x.Jugada == jugada);
                if ((record.Puntos + puntos) <= 5 && jugada.Length < 4)
                {
                    record.Puntos += puntos;
                }
            }
            else
            {
                Combinations.Add(new Combination() { Puntos = puntos, Jugada = jugada });
            }

            SumPuntos();
            SelectedCombination = Combinations.Last();
        }

        public void AddTicketsFromToday(int value)
        {
            TicketsFromToday.Add(value);
            SelectedTicket = TicketsFromToday.Last();
        }

        public void SumPuntos()
        {
            TotalSum = Combinations.Sum(x => x.Puntos);
            PuntosTotalSum = TotalSum.ToString("C", CultureInfo.GetCultureInfo("es-ES"));
        }

        public void AddValuesAndPrint()
        {
            if (Combinations.Count > 0
                && SelectedLoteria != null)
            {
                (string trackPin, int trackTicketId) = _databaseOperations.AddToDabataseAndPrint(Combinations, SelectedLoteria);
                if (trackTicketId > 0)
                {
                    PrintBehaviour print = new PrintBehaviour(Combinations, trackPin, trackTicketId, SelectedLoteria.Name);
                    print.PrintTicket();
                    RemoveAllItems();
                    AddTicketsFromToday(trackTicketId);
                    //TODO: Complete the printing behaviour
                    //the CompleteTicketInfo contains Combinations, TicketNo, LoteriaName (only the Pin would be missing)
                    //Which means I could make a class that only takes one parameter (two with the PIN as optional)
                    //And like that we can also handle what we print when we want to rePrint a ticket (which In this case would not have PIN) 
                }
            }
        }

        public void CopyTicket()
        {
            if (TicketToCopy > 0 && Combinations.Count == 0)
            {
                IEnumerable<Combination> copiedTicket = _databaseOperations.GetCombinations(TicketToCopy);
                foreach (Combination comb in copiedTicket)
                {
                    Combinations.Add(comb);
                }
            }
        }

        public void RetrieveTicketInfo()
        {
            IEnumerable<Combination> ticketCombinations = _databaseOperations.GetCombinations(SelectedTicket);
            string loteriaName = _databaseOperations.GetLoteriaName(SelectedTicket);
            DateTimeOffset dateTime = _databaseOperations.GetDateCreated(SelectedTicket);
            string dayOfWeek = dateTime.DayOfWeek.ToString();
            string date = dateTime.Date.ToString("D");
            string time = dateTime.TimeOfDay.ToString(@"hh\:mm");
            CompleteTicketInfo = new()
            {
                DayOfWeek = dayOfWeek,
                Date = date,
                Time = time,
                TicketNo = SelectedTicket,
                LoteriaName = loteriaName,
                Combinations = ticketCombinations
            };
        }

        public void RemoveSelectedItem()
        {
            Combinations.Remove(SelectedCombination);
            SumPuntos();
        }

        public void RemoveAllItems()
        {
            Combinations.Clear();
            SumPuntos();
        }
    }
}