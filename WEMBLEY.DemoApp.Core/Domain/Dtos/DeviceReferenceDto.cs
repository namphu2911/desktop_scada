using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class DeviceReferenceDto
    {
        public string DeviceId { get; set; }
        public List<MFCDto> MFCs { get; set; }
        public DeviceReferenceDto(string deviceId, List<MFCDto> mFCs)
        {
            DeviceId = deviceId;
            MFCs = mFCs;
        }
    }
}
