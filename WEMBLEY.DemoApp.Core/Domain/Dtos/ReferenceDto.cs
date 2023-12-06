using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class ReferenceDto
    {
        public int Id { get; set; }
        public string RefName { get; set; }
        public string DeviceType { get; set; }
        public string ProductName { get; set; }
        public ReferenceDto(int id, string refName, string deviceType, string productName)
        {
            Id = id;
            RefName = refName;
            DeviceType = deviceType;
            ProductName = productName;
        }
    }
}
