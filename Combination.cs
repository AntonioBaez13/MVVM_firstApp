using Stylet;

namespace MVVM_firstApp
{
    public class Combination : PropertyChangedBase
    {
        private int _puntos;
        public int Puntos
        {
            get => _puntos;
            set => SetAndNotify(ref _puntos, value);
        }

        private string _jugada;
        public string Jugada
        {
            get => _jugada;
            set => SetAndNotify(ref _jugada, value);
        }

    }
}
