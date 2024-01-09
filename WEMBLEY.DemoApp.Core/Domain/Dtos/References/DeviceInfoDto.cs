using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Persons;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.References
{
    public class DeviceInfoDto
    {
        public string DeviceId { get; set; }
        public List<PersonWorkingDto> Persons { get; set; }
        public List<MFCDto> MFCs { get; set; }
        public DeviceInfoDto(string deviceId, List<PersonWorkingDto> persons, List<MFCDto> mFCs)
        {
            DeviceId = deviceId;
            Persons = persons;
            MFCs = mFCs;
        }
    }
}
