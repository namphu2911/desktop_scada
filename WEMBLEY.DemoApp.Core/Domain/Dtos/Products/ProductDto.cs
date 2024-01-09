using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string DeviceType { get; set; }
        public ProductDto(int id, string productName, string deviceType)
        {
            Id = id;
            ProductName = productName;
            DeviceType = deviceType;
        }
    }
}
