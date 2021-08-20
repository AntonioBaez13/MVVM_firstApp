using Stylet;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MVVM_firstApp.Pages
{
    public class ShellViewModel : Screen
    {
        public ObservableCollection<Combination> Combinations { get; set; }

        private Combination _selectedCombination;

        public Combination SelectedCombination
        {
            get { return _selectedCombination; }
            set { SetAndNotify(ref _selectedCombination, value); }
        }


        private string _puntosTotalSum;

        public string PuntosTotalSum
        {
            get { return _puntosTotalSum; }
            set { SetAndNotify(ref _puntosTotalSum, value); }
        }
        public ShellViewModel()
        {
            Combinations = new ObservableCollection<Combination>();
        }

        public void AddToCollection(int puntos, string jugada)
        {
            Combinations.Add(new Combination() { Puntos = puntos, Jugada = jugada });
            SumPuntos();
            SelectedCombination = Combinations.Last();
        }

        //TODO: Text box to retrieve the sum of property puntos value
        public void SumPuntos()
        {
            double total = Combinations.Sum(x => x.Puntos);
            PuntosTotalSum = total.ToString("C", CultureInfo.GetCultureInfo("es-ES"));
        }
    }
}
