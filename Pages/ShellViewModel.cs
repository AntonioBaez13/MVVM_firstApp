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
        private readonly IGenericRepository<Loteria> LoteriaRepository;
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
            LoteriaRepository = new GenericRepository<Loteria>(new LotoContext());
            Combinations = new ObservableCollection<Combination>();
            Loterias = this.LoteriaRepository.GetAll();
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
            if (Combinations.Count > 0)
            {
                //TODO:
                //get the selected loteria
                //method in which I will pass the selcted loteria and the combinations
                //if the method can succesfully add the values to the database 
                //RemoveAllItems();
                //else continue
            }
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
