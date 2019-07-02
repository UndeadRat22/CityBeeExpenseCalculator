using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace deadrat22
{
    public class Report
    {
        public  IEnumerable<MonthlyReport> MonthlyReports { get; private set; }

        public double AvgPrice { get => MonthlyReports.Average(report => report.TotalPrice); }
        public double MaxPrice { get => MonthlyReports.Max(report => report.TotalPrice); }
        public double MinPrice { get => MonthlyReports.Min(report => report.TotalPrice); }

        public Report(IEnumerable<MonthlyReport> reports)
        {
            MonthlyReports = reports;
        }
    }
}
