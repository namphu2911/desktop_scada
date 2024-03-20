using WEMBLEY.DemoApp.Core.Domain.Dtos.Lines;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.References
{
    public class ReferenceDto
    {
        public string ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string ProductName { get; set; }
        public List<UsableLinesDto> UsableLines { get; set; }
        public ReferenceDto(string referenceId, string referenceName, string productName, List<UsableLinesDto> usableLines)
        {
            ReferenceId = referenceId;
            ReferenceName = referenceName;
            ProductName = productName;
            UsableLines = usableLines;
        }
    }
}
