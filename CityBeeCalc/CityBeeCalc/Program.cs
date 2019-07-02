using System;
using System.IO;
using System.Linq;

namespace deadrat22
{
    public class Program
    {
        public static string OutputFn = @"C:\Users\Valdas\Desktop\report.txt";
        public static string InputFn = @"C:\Users\Valdas\Desktop\trips.csv";
        public static void Main(string[] args)
        {

            var calc = new CityBeeCalculator();

            calc.LoadJourneys(@"C:\Users\Valdas\Desktop\trips.csv");

            var report = calc.GenerateReport();

            using (var writer = new StreamWriter(@"C:\Users\Valdas\Desktop\report2.txt"))
            {
                foreach (var monthly in report.MonthlyReports.Reverse())
                {
                    writer.WriteLine($@"mo{monthly.month.Month.ToString()}: {monthly.TotalPrice.ToString()}");
                }
                writer.WriteLine($"Min: {report.MinPrice}");
                writer.WriteLine($"Max: {report.MaxPrice}");
                writer.WriteLine($"Avg: {report.AvgPrice}");
            }
        }
    }
}
