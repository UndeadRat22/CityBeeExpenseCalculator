using CsvHelper;
using CsvHelper.Configuration;
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

        public void LoadJourneysRetarded(string fileName)
        {
            if (Journeys == null)
                Journeys = new List<Journey>();

            using (var reader = new StreamReader(fileName))
            {
                var header = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(';');

                    var priceString = Regex.Match(line[8], @"(\d+\.\d\d).*").Groups[1].Value;

                    var journey = new Journey
                    {
                        ID = int.Parse(line[0]),
                        Vehicle = line[1],
                        StartAddress = line[2],
                        StartTime = DateTime.Parse(line[3]),
                        EndAddress = line[4],
                        EndTime = DateTime.Parse(line[5]),
                        Distance = int.Parse(line[6]),
                        Duration = line[7],
                        Price = double.Parse(priceString)
                    };
                    Journeys.Add(journey);
                }
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
