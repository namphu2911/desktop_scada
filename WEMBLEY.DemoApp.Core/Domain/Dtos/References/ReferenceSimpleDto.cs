using WEMBLEY.DemoApp.Core.Domain.Models;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.References
{
    public class ReferenceSimpleDto
    {
        public string ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string ProductName { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public ELineType LineType { get; set; }
        public ReferenceSimpleDto(string referenceId, string referenceName, string productName, string lineId, string lineName, ELineType lineType)
        {
            ReferenceId = referenceId;
            ReferenceName = referenceName;
            ProductName = productName;
            LineId = lineId;
            LineName = lineName;
            LineType = lineType;
        }
    }
}
