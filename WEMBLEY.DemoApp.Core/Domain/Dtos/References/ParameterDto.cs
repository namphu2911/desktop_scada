using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.References
{
    public class ParameterDto
    {
        public string DeviceType { get; set; }
        public string ProductName { get; set; }
        public int RefId { get; set; }
        public string RefName { get; set; }
        public string LotId { get; set; }
        public int LotSize { get; set; }
        public List<DeviceInfoDto> Devices { get; set; }
        public ParameterDto(string deviceType, string productName, int refId, string refName, string lotId, int lotSize, List<DeviceInfoDto> devices)
        {
            DeviceType = deviceType;
            ProductName = productName;
            RefId = refId;
            RefName = refName;
            LotId = lotId;
            LotSize = lotSize;
            Devices = devices;
        }
    }
}
