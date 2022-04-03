using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace MVVM_firstApp
{
    public class PrintBehaviour : IPrintBehaviour
    {
        private Font font, boldFont;
        private Graphics graphics;
        private Brush brush;

        private IEnumerable<Combination> combination;
        private string pin;
        private int ticketId;
        private string loteriaName;
        private string date;
        private string time;

        public void PrintTicket(FullTicketData completeTicketInfo)
        {
            combination = completeTicketInfo.Combinations;
            pin = completeTicketInfo.Pin;
            ticketId = completeTicketInfo.TicketNo;
            loteriaName = completeTicketInfo.LoteriaName;
            date = completeTicketInfo.CompleteDayandDate;
            time = completeTicketInfo.Time;
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
            graphics.DrawString("Ticket: " + ticketId, font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString("Pin: " + pin, font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString(date, font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString(time, font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString("-------------------------", font, brush, xPos, yPos);
            yPos += fontHeigh;

            foreach (Combination c in combination)
            {
                graphics.DrawString(c.Jugada.PadRight(10) + c.Puntos + ".00 € ", boldFont, brush, xPos, yPos);
                yPos += fontHeigh;
            }

            int totalSum = combination.Sum(x => x.Puntos);
            graphics.DrawString("-------------------------", font, brush, xPos, yPos);
            yPos += fontHeigh;
            graphics.DrawString("Total " + totalSum + ".00 € ", boldFont, brush, xPos, yPos);
        }
    }
}
