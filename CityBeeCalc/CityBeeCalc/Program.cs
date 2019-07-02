using System;
using System.IO;
using System.Linq;

namespace deadrat22
{
    public class Program
    {
        public static string fn = @"C:\Users\Valdas\Desktop\report.txt";
        public static void Main(string[] args)
        {
            using (var reader = new StreamReader(fn))
            {
                CsvReader<Journey> csvReader = new CsvReader<Journey>(reader, ";");
                Journey result = csvReader.Read();
                Console.WriteLine();
            }
               /*var calc = new CityBeeCalculator();

            calc.LoadJourneysRetarded(@"C:\Users\Valdas\Desktop\trips.csv");

            var report = calc.GenerateReport();

            using (var writer = new StreamWriter(@"C:\Users\Valdas\Desktop\report.txt"))
            {
                foreach (var monthly in report.MonthlyReports.Reverse())
                {
                    writer.WriteLine($@"mo{monthly.month.Month.ToString()}: {monthly.TotalPrice.ToString()}");
                }
                writer.WriteLine($"Min: {report.MinPrice}");
                writer.WriteLine($"Max: {report.MaxPrice}");
                writer.WriteLine($"Avg: {report.AvgPrice}");
            }*/
        }
    }
}
