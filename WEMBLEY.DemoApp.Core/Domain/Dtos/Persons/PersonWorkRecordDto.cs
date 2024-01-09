using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Devices;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Persons
{
    public class PersonWorkRecordDto
    {
        public DeviceDto Device { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public PersonWorkRecordDto(DeviceDto device, DateTime startTime, DateTime endTime)
        {
            Device = device;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
