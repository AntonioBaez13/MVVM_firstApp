using System.Collections.Generic;

namespace MVVM_firstApp
{
    public class FullTicketData
    {
        public string DayOfWeek { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int TicketNo { get; set; }

        public string LoteriaName { get; set; }

        public IEnumerable<Combination> Combinations { get; set; }

    }
}
