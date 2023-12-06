using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Models
{
    public class DataPoint
    {
        public double TagValue { get; set; }
        public DateTime TimeStamp { get; set; }
        public DataPoint(double tagValue, DateTime timeStamp)
        {
            TagValue = tagValue;
            TimeStamp = timeStamp;
        }
    }
}
