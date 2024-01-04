using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class ShiftReportDto
    {
        public int Id { get; set; }
        public double OEE { get; set; }
        public double A { get; set; }
        public double P { get; set; }
        public double Q { get; set; }
        public DateTime Date { get; set; }
        public int ShiftNumber { get; set; }
        public string DeviceId { get; set; }
        public int ProductCount { get; set; }
        public int DefectCount { get; set; }
        public ShiftReportDto(int id, double oEE, double a, double p, double q, DateTime date, int shiftNumber, string deviceId, int productCount, int defectCount)
        {
            Id = id;
            OEE = oEE;
            A = a;
            P = p;
            Q = q;
            Date = date;
            ShiftNumber = shiftNumber;
            DeviceId = deviceId;
            ProductCount = productCount;
            DefectCount = defectCount;
        }

        public void UpdateOEE(double oEE, double a, double p, double q)
        {
            OEE = oEE * 100;
            A = a * 100;
            P = p * 100;
            Q = q * 100;
        }
    }
}
