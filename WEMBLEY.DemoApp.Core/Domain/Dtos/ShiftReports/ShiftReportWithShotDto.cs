namespace WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports
{
    public class ShiftReportWithShotDto
    {
        public double OEE { get; set; }
        public double A { get; set; }
        public double P { get; set; }
        public double Q { get; set; }
        public DateTime Date { get; set; }
        public int ShiftNumber { get; set; }
        public string DeviceId { get; set; }
        public int ProductCount { get; set; }
        public int DefectCount { get; set; }
        public List<ShotDto> Shots { get; set; }
        public ShiftReportWithShotDto(double oEE, double a, double p, double q, DateTime date, int shiftNumber, string deviceId, int productCount, int defectCount, List<ShotDto> shots)
        {
            OEE = oEE;
            A = a;
            P = p;
            Q = q;
            Date = date;
            ShiftNumber = shiftNumber;
            DeviceId = deviceId;
            ProductCount = productCount;
            DefectCount = defectCount;
            Shots = shots;
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
