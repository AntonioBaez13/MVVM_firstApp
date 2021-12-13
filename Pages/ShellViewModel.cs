using MVVM_firstApp.Models;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MVVM_firstApp.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly GenericRepository<Loteria> LoteriaRepository;
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
            LoteriaRepository = new GenericRepository<Loteria>(new LotoContext());
            Combinations = new ObservableCollection<Combination>();
            var queryable = this.LoteriaRepository.GetAll().;
        }

        public void AddToCollection(int puntos, string jugada)
        {
            Combinations.Add(new Combination() { Puntos = puntos, Jugada = jugada });
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
            throw new NotImplementedException();
            //TODO: Add all the items on the collection to a database, and if suscesfull print a receipt
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
