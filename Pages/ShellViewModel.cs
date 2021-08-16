using Stylet;
using System.Collections.ObjectModel;
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

        public ShellViewModel()
        {
            Combinations = new ObservableCollection<Combination>();
        }

        public void AddToCollection(int puntos, string jugada)
        {
            Combinations.Add(new Combination() { Puntos = puntos, Jugada = jugada });
            SelectedCombination = Combinations.Last();
        }

    }
}
