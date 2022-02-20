using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace MVVM_firstApp
{
    public class PrintBehaviour
    {
        private IEnumerable<Combination> combination;
        private int pin;
        private int ticketId;
        private string loteriaName;

        public PrintBehaviour(IEnumerable<Combination> combination, int pin, int ticketId, string loteriaName)
        {
            this.combination = combination;
            this.pin = pin;
            this.ticketId = ticketId;
            this.loteriaName = loteriaName;
        }

        public void PrintTicket()
        {

        }

        private void PrintDocument_PrintPage(object render, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Font font = new Font("Courier New", 12);
            Font bigFont = new Font("Courier New", 18);
            Brush brush = new SolidBrush(Color.Black);
            float fontHeigh = font.GetHeight();
            float bigFontHeigh = bigFont.GetHeight();


        }
    }
}
