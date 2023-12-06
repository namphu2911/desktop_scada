using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class LineInitialSettingEntry
    {
        public string DeviceType { get; set; }
        public string ProductName { get; set; }
        public string RefName { get; set; }
        public string LotId { get; set; }
        public string LotSize { get; set; }
        public LineInitialSettingEntry(string deviceType, string productName, string refName, string lotId, string lotSize)
        {
            DeviceType = deviceType;
            ProductName = productName;
            RefName = refName;
            LotId = lotId;
            LotSize = lotSize;
        }
    }
}
