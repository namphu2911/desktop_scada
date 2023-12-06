using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class ShotDto
    {
        public double ExecutionTime { get; private set; }
        public double CycleTime { get; private set; }
        public DateTime TimeStamp { get; private set; }
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
    }
}
