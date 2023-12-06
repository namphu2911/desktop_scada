using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class LotDeviceReferenceDto
    {
        public string DeviceType { get; set; }
        public string ProductName { get; set; }
        public string RefName { get; set; }
        public string LotId { get; set; }
        public int LotSize { get; set; }
        public List<DeviceReferenceDto> Devices { get; set; }
        public LotDeviceReferenceDto(string deviceType, string productName, string refName, string lotId, int lotSize, List<DeviceReferenceDto> devices)
        {
            DeviceType = deviceType;
            ProductName = productName;
            RefName = refName;
            LotId = lotId;
            LotSize = lotSize;
            Devices = devices;
        }
    }
}
