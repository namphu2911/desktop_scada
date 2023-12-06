using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Models;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class MachineStatusDto
    {
        public string DeviceId { get; set; }
        public EMachineStatus Status { get; set; }
        public DateTime Date { get; set; }
        public int ShiftNumber { get; set; }
        public DateTime Timestamp { get; set; }

        public MachineStatusDto(string deviceId, EMachineStatus status, DateTime date, int shiftNumber, DateTime timestamp)
        {
            DeviceId = deviceId;
            Status = status;
            Date = date;
            ShiftNumber = shiftNumber;
            Timestamp = timestamp;
        }
    }
}
