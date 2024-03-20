using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences
{
    public class StationReferenceInfoDto
    {
        public string ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string StationId { get; set; }
        public string StationName { get; set; }
        public StationReferenceInfoDto(string referenceId, string referenceName, string stationId, string stationName)
        {
            ReferenceId = referenceId;
            ReferenceName = referenceName;
            StationId = stationId;
            StationName = stationName;
        }
    }
}
