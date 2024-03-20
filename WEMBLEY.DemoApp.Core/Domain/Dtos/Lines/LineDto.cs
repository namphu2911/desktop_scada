using WEMBLEY.DemoApp.Core.Domain.Dtos.Stations;
using WEMBLEY.DemoApp.Core.Domain.Models;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Lines
{
    public class LineDto
    {
        public string LineId { get; set; }
        public string LineName { get; set; }
        public ELineType LineType { get; set; }
        public List<StationDto> Stations { get; set; }

        public LineDto(string lineId, string lineName, ELineType lineType, List<StationDto> stations)
        {
            LineId = lineId;
            LineName = lineName;
            LineType = lineType;
            Stations = stations;
        }
    }
}
