namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Stations
{
    public class StationDto
    {
        public string StationId { get; set; }
        public string StationName { get; set; }
        public string LineId { get; set; }

        public StationDto(string stationId, string stationName, string lineId)
        {
            StationId = stationId;
            StationName = stationName;
            LineId = lineId;
        }
    }
}
