using WEMBLEY.DemoApp.Core.Domain.Dtos.Stations;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Employees
{
    public class WorkRecord
    {
        public StationDto Station { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public WorkRecord(StationDto station, DateTime startTime, DateTime endTime)
        {
            Station = station;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
