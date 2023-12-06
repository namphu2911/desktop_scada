using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class MFCDto
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public MFCDto(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
