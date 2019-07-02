using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace deadrat22
{
    public class CityBeeCalculator
    {
        public List<Journey> Journeys { get; private set; }

        private CsvReader<Journey> _reader;

        public void LoadJourneys(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                _reader = new CsvReader<Journey>(streamReader, ";");
                Journeys = _reader.ReadAll()
                    .ToList();
            }
        }

        public Report GenerateReport()
        {
            if (Journeys == null)
                return null;
            var monthly = Journeys
                .GroupBy(j => j.StartTime.Month)
                .Select(g => new MonthlyReport(g))
                .ToArray();
            return new Report(monthly);
        }
    }
}
