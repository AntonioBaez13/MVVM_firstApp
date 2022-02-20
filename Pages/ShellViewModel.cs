﻿using MVVM_firstApp.Models;
using Stylet;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MVVM_firstApp.Pages
{
    public class ShellViewModel : Screen
    {
        public DatabaseOperations databaseOperations = new DatabaseOperations(); 

        private readonly LotoContext db;
        public IEnumerable<Loteria> Loterias { get; set; }

        private Loteria _selectedLoteria;
        public Loteria SelectedLoteria
        {
            get => _selectedLoteria;
            set => SetAndNotify(ref _selectedLoteria, value);
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

        private double TotalSum { get; set; }

        public ShellViewModel()
        {
            db = new LotoContext();
            Combinations = new ObservableCollection<Combination>();
            Loterias = this.db.Loteria.ToList();
        }

        public void AddToCollection(int puntos, string jugada)
        {
            //if the jugada is already in the collection, only update the puntos
            if (Combinations.Any(x => x.Jugada == jugada))
            {
                var record = Combinations.FirstOrDefault(x => x.Jugada == jugada);
                if((record.Puntos + puntos) <= 5 && jugada.Length < 4) record.Puntos += puntos;
            }
            else
            {
                Combinations.Add(new Combination() { Puntos = puntos, Jugada = jugada });
            }
                
            SumPuntos();
            SelectedCombination = Combinations.Last();
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
                bool sucess = databaseOperations.AddToDabataseAndPrint(Combinations, SelectedLoteria);
                if (sucess)
                {
                    RemoveAllItems();
                    //TODO: Complete the printing behaviour
                    //PrintBehaviour print = new PrintBehaviour(Combinations,1, 2, SelectedLoteria.Name);
                    //print.PrintTicket();
                }
            }
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
