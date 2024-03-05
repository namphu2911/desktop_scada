using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report
{
    public class ShiftReportEntry
    {
        public double OEE { get; set; }
        public double A { get; set; }
        public double P { get; set; }
        public double Q { get; set; }
        public DateTime Date { get; set; }
        public int ShiftNumber { get; set; }
        public int ProductCount { get; set; }
        public int DefectCount { get; set; }

        public ShiftReportEntry(double oEE, double a, double p, double q, DateTime date, int shiftNumber, int productCount, int defectCount)
        {
            OEE = oEE;
            A = a;
            P = p;
            Q = q;
            Date = date;
            ShiftNumber = shiftNumber;
            ProductCount = productCount;
            DefectCount = defectCount;
        }
    }
}
