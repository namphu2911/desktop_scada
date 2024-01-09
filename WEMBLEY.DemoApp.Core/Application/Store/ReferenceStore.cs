using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class ReferenceStore : BaseViewModel
    {
        public List<ReferenceDto> References { get; private set; }
        public ObservableCollection<string> DeviceTypes { get; private set; }
        public ObservableCollection<string> ProductNames { get; private set; }
        public ObservableCollection<string> ReferenceNames { get; private set; }

        public ObservableCollection<string> HerapinCapProductNames { get; private set; }
        public ObservableCollection<string> HerapinCapReferenceNames { get; private set; }
        public ReferenceStore()
        {
            References = new List<ReferenceDto>();
            DeviceTypes = new ObservableCollection<string>();
            ProductNames = new ObservableCollection<string>();
            ReferenceNames = new ObservableCollection<string>();

            HerapinCapProductNames = new ObservableCollection<string>();
            HerapinCapReferenceNames = new ObservableCollection<string>();
        }

        public void SetReference(IEnumerable<ReferenceDto> references)
        {
            References = references.ToList();
            DeviceTypes = new ObservableCollection<string>(References.Select(i => i.DeviceType).Distinct().OrderBy(s => s));
            ProductNames = new ObservableCollection<string>(References.Select(i => i.ProductName).Distinct().OrderBy(s => s));
            ReferenceNames = new ObservableCollection<string>(References.Select(i => i.RefName).Distinct().OrderBy(s => s));

            HerapinCapProductNames = new ObservableCollection<string>(References.Where(x => x.DeviceType == "HerapinCap").Select(i => i.ProductName).Distinct().OrderBy(s => s));
            HerapinCapReferenceNames = new ObservableCollection<string>(References.Where(x => x.DeviceType == "HerapinCap").Select(i => i.RefName).OrderBy(s => s));
        }
    }
}
