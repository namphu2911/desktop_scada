using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class DeviceSelectedStore
    {
        public string SeletedDeviceId { get; private set; } = "";
        public void SetSeletedDevice(string seletedDeviceId)
        {
            SeletedDeviceId = seletedDeviceId;
        }
    }
}
