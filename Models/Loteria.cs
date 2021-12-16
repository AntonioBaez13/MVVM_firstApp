using System.Collections.Generic;

namespace MVVM_firstApp.Models
{
    public class Loteria
    {
        public int LoteriaId { get; set; }
        public string Name { get; set; }

        public ICollection<Jugada> Jugadas { get; set; }
    }
}