using WEMBLEY.DemoApp.Core.Domain.Dtos.Lines;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Products
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public List<UsableLinesDto> UsableLines { get; set; }
        public ProductDto(string productId, string productName, List<UsableLinesDto> usableLines)
        {
            ProductId = productId;
            ProductName = productName;
            UsableLines = usableLines;
        }
    }
}
