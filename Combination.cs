using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_firstApp
{
    public class Combination : PropertyChangedBase
    {
        private int _puntos;
        public int Puntos
        {
            get { return _puntos; }
            set { SetAndNotify(ref _puntos, value); }
        }

        private string _jugada;
        public string Jugada
        {
            get { return _jugada; }
            set { SetAndNotify(ref _jugada, value); }
        }

    }
}
