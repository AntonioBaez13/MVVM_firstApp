
using System;
using System.Text.RegularExpressions;

namespace MVVM_firstApp
{
    public class CleanTextBoxes
    {
        public string CleanJugadaInput(string jugada)
        {
            int size = jugada.Length;

            switch (size)
            {
                case 1:
                    jugada = jugada.PadLeft(2, '0');
                    return jugada;
                case 2:
                    return jugada;
                case 3:
                    jugada = jugada.PadLeft(4, '0');
                    return OrderCombination(jugada);
                case 4:
                    return OrderCombination(jugada);
                default:
                    break;
            }

            throw new ArgumentException("Jugada No Aceptada");
        }

        public int CleanPuntosInput(string text)
        {
            if (int.TryParse(text, out int puntos))
            {
                return puntos;
            }
            else
            {
                throw new ArgumentException("Puntos no Aceptados ");
            }
        }
        private string OrderCombination(string jugada)
        {
            //this regex splits a 3 or 4 char string, into two strings 
            string[] test = Regex.Split(jugada, "(?<=\\G..)");
            string a = test[0];
            string b = test[1];
            int.TryParse(a, out int firstPart);
            int.TryParse(b, out int lastPart);

            jugada = firstPart > lastPart ? b + a : a + b;

            return jugada;
        }
    }
}