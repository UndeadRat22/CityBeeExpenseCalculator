using System;
using System.Collections.Generic;
using System.Linq;

namespace deadrat22
{
    public class MonthlyReport
    {
        public IEnumerable<Journey> Journeys { get; private set; }
        public double TotalPrice { get => Journeys.Sum(j => j.Price); }
        public int TotalDistance { get => Journeys.Sum(j => j.Distance); }
        public DateTime month { get => Journeys.First().StartTime; }

        public MonthlyReport(IEnumerable<Journey> journeys)
        {
            Journeys = journeys;
        }
    }
}
