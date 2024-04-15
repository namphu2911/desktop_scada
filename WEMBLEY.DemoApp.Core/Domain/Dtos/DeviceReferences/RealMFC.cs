using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences
{
    public class RealMFC
    {
        public string Name { get; set; }
        public object? RealValue { get; set; }
        public RealMFC(string name, object? realValue)
        {
            Name = name;
            RealValue = realValue;
        }
    }
}
