using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace MVVM_firstApp
{
    public class PrintBehaviour
    {
        private Font font, boldFont;
        private Graphics graphics;
        private Brush brush;

        private readonly IEnumerable<Combination> combination;
        private readonly string pin;
        private readonly int ticketId;
        private readonly string loteriaName;

        public PrintBehaviour(IEnumerable<Combination> combination, string pin, int ticketId, string loteriaName)
        {
            this.combination = combination;
            this.pin = pin;
            this.ticketId = ticketId;
            this.loteriaName = loteriaName;
        }

        public void PrintTicket()
        {
            PrintDocument printDocument = new();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            printDocument.Print();
        }

        private void PrintDocument_PrintPage(object render, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            font = new("Courier New", 12);
            boldFont = new("Courier New", 15, FontStyle.Bold);
            brush = Brushes.Black;
            
            float fontHeigh = font.GetHeight();
            float xPos = 10;
            float yPos = 30;

            graphics.DrawString(loteriaName, boldFont, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString(("Ticket: " + ticketId), font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString(("Pin: " + pin), font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString((DateTime.Now.ToString("dddd, dd MMMM yyyy")), font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString((DateTime.Now.ToString("HH:mm:ss")), font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString("-------------------------", font, brush, xPos, yPos);
            yPos += fontHeigh;

            foreach(Combination c in combination)
            {
                graphics.DrawString((c.Jugada.PadRight(10) + c.Puntos + ".00 € "), boldFont, brush, xPos, yPos);
                yPos += fontHeigh;
            }

            int totalSum = combination.Sum(x => x.Puntos);
            graphics.DrawString("-------------------------", font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString(("Total " + totalSum + ".00 € "), boldFont, brush, xPos, yPos);
        }
    }
}
