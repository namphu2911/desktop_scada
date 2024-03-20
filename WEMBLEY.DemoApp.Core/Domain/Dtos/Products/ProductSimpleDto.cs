using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Models;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Products
{
    public class ProductSimpleDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public ELineType LineType { get; set; }
        public ProductSimpleDto(string productId, string productName, string lineId, string lineName, ELineType lineType)
        {
            ProductId = productId;
            ProductName = productName;
            LineId = lineId;
            LineName = lineName;
            LineType = lineType;
        }
    }
}
