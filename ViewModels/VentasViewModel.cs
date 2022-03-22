﻿using MVVM_firstApp.Models;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MVVM_firstApp.ViewModels
{
    public class VentasViewModel : Screen
    {
        DatabaseOperations databaseOperations = new DatabaseOperations();

        public IEnumerable<Loteria> Loterias { get; set; }

        private Loteria _selectedLoteria;
        public Loteria SelectedLoteria
        {
            get => _selectedLoteria;
            set => SetAndNotify(ref _selectedLoteria, value);
        }

        public ObservableCollection<int> TicketsFromToday { get; set; }
        private int _selectedTicket;
        public int SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                if (SetAndNotify(ref _selectedTicket, value))
                {
                    ExtracTicketInfo();
                }
            }
        }

        public FullTicketData _completeTicketInfo;
        public FullTicketData CompleteTicketInfo
        {
            get => _completeTicketInfo;
            set => SetAndNotify(ref _completeTicketInfo, value);
        }

        public ObservableCollection<Combination> Combinations { get; set; }

        private Combination _selectedCombination;
        public Combination SelectedCombination
        {
            get => _selectedCombination;
            set => SetAndNotify(ref _selectedCombination, value);
        }

        private string _puntosTotalSum;
        public string PuntosTotalSum
        {
            get => _puntosTotalSum;
            set => SetAndNotify(ref _puntosTotalSum, value);
        }

        private int _ticketToCopy;
        public int TicketToCopy
        {
            get => _ticketToCopy;
            set => SetAndNotify(ref _ticketToCopy, value);
        }
        private double TotalSum { get; set; }

        public VentasViewModel()
        {
            this.DisplayName = "Ventas";
            Combinations = new ObservableCollection<Combination>();
            TicketsFromToday = new ObservableCollection<int>();
            Loterias = databaseOperations.GetAllLoterias();
        }

        public void AddToCollection(int puntos, string jugada)
        {
            //if the jugada is already in the collection, only update the puntos
            if (Combinations.Any(x => x.Jugada == jugada))
            {
                var record = Combinations.FirstOrDefault(x => x.Jugada == jugada);
                if ((record.Puntos + puntos) <= 5 && jugada.Length < 4) record.Puntos += puntos;
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

        //Text box to retrieve the sum of property puntos value
        public void SumPuntos()
        {
            TotalSum = Combinations.Sum(x => x.Puntos);
            PuntosTotalSum = TotalSum.ToString("C", CultureInfo.GetCultureInfo("es-ES"));
        }

        public void AddValuesAndPrint()
        {
            if (Combinations.Count > 0 && SelectedLoteria.Name != null)
            {
                var sucess = databaseOperations.AddToDabataseAndPrint(Combinations, SelectedLoteria);
                if (sucess.trackTicketId > 0)
                {
                    RemoveAllItems();
                    AddTicketsFromToday(sucess.trackTicketId);
                    //TODO: Complete the printing behaviour
                    //PrintBehaviour print = new PrintBehaviour(Combinations, sucess.trackPin, success.trackTicketId, SelectedLoteria.Name);
                    //print.PrintTicket();
                }
            }
        }

        public void CopyTicket()
        {
            if (TicketToCopy > 0 && Combinations.Count == 0)
            {
                IEnumerable<Combination> copiedTicket = databaseOperations.GetCombinations(TicketToCopy);
                foreach (Combination comb in copiedTicket)
                {
                    Combinations.Add(comb);
                }
            }
        }

        public void ExtracTicketInfo()
        {
            IEnumerable<Combination> ticketCombinations = databaseOperations.GetCombinations(SelectedTicket);
            string loteriaName = databaseOperations.GetLoteriaName(SelectedTicket);
            DateTimeOffset dateTime = databaseOperations.GetDateCreated(SelectedTicket);
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