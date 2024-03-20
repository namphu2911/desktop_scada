namespace WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports
{
    public class ShotDto
    {
        public double ExecutionTime { get; set; }
        public double CycleTime { get; set; }
        public DateTime TimeStamp { get; set; }
        public double A { get; set; }
        public double Q { get; set; }
        public double P { get; set; }
        public double OEE { get; set; }
        public ShotDto(double executionTime, double cycleTime, DateTime timeStamp, double a, double q, double p, double oEE)
        {
            ExecutionTime = executionTime;
            CycleTime = cycleTime;
            TimeStamp = timeStamp;
            A = a;
            Q = q;
            P = p;
            OEE = oEE;
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
