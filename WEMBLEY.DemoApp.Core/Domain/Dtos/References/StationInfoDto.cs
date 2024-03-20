using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Employees;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.References
{
    public class StationInfoDto
    {
        public string StationId { get; set; }
        public List<EmployeeWorkingDto> Employees { get; set; }
        public List<MFCDto> MFCs { get; set; }
        public StationInfoDto(string stationId, List<EmployeeWorkingDto> employees, List<MFCDto> mFCs)
        {
            StationId = stationId;
            Employees = employees;
            MFCs = mFCs;
        }
    }
}
