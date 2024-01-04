using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class ComparedMFC
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double? RealValue { get; set; }
        public bool IsAlarmed => Value != RealValue;
        public ComparedMFC(string name, double value, double? realValue)
        {
            Name = name;
            Value = value;
            RealValue = realValue;
        }
    }
}
