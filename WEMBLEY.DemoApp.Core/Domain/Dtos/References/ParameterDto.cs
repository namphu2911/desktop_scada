using WEMBLEY.DemoApp.Core.Domain.Dtos.Lines;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.References
{
    public class ParameterDto
    {
        public string ProductName { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string LotCode { get; set; }
        public int LotSize { get; set; }
        public UsableLinesDto Line { get; set; }
        public List<StationInfoDto> Stations { get; set; }
        public ParameterDto(string productName, string referenceId, string referenceName, string lotCode, int lotSize, UsableLinesDto line, List<StationInfoDto> stations)
        {
            ProductName = productName;
            ReferenceId = referenceId;
            ReferenceName = referenceName;
            LotCode = lotCode;
            LotSize = lotSize;
            Line = line;
            Stations = stations;
        }
    }
}
