using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Models
{
    public class DataPoint
    {
        public double OEE { get; set; }
        public DateTime TimeStamp { get; set; }
        public DataPoint(double oee, DateTime timeStamp)
        {
            OEE = oee;
            TimeStamp = timeStamp;
        }
    }
}
