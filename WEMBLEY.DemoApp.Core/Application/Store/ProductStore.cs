using System.Collections.ObjectModel;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Products;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class ProductStore : BaseViewModel
    {
        public List<ProductDto> Products { get; private set; }
        public List<ProductSimpleDto> ProductSimples { get; private set; }
        public ObservableCollection<string> ProductIds { get; private set; }
        public ObservableCollection<string> ProductNames { get; private set; }
        public ObservableCollection<string> LineIds { get; private set; }
        public ObservableCollection<string> LineNames { get; private set; }
        public ProductStore()
        {
            Products = new List<ProductDto>();
            ProductSimples = new List<ProductSimpleDto>();

            ProductIds = new ObservableCollection<string>();
            ProductNames = new ObservableCollection<string>();
            LineIds = new ObservableCollection<string>();
            LineNames = new ObservableCollection<string>();
        }

        public void SetProduct(IEnumerable<ProductDto> products)
        {
            Products = products.ToList();
            ProductSimples = products.SelectMany(i => i.UsableLines.Select(x => new ProductSimpleDto(
                i.ProductId,
                i.ProductName,
                x.LineId,
                x.LineName,
                x.LineType))).ToList();

            ProductIds = new ObservableCollection<string>(ProductSimples.Select(i => i.ProductId).Distinct().OrderBy(s => s));
            ProductNames = new ObservableCollection<string>(ProductSimples.Select(i => i.ProductName).Distinct().OrderBy(s => s));

            LineIds = new ObservableCollection<string>(ProductSimples.Select(i => i.LineId).Distinct().OrderBy(s => s));
            LineNames = new ObservableCollection<string>(ProductSimples.Select(i => i.LineName).Distinct().OrderBy(s => s));
        }
    }
}
