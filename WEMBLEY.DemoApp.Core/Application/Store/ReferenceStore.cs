using System.Collections.ObjectModel;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class ReferenceStore : BaseViewModel
    {
        public List<ReferenceDto> References { get; private set; }
        public List<ReferenceSimpleDto> ReferenceSimples { get; private set; }
        public ObservableCollection<string> ReferenceIds { get; private set; }
        public ObservableCollection<string> ReferenceNames { get; private set; }
        public ObservableCollection<string> ProductNames { get; private set; }
        public ObservableCollection<string> LineIds { get; private set; }
        public ObservableCollection<string> LineNames { get; private set; }

        public ReferenceStore()
        {
            References = new List<ReferenceDto>();
            ReferenceSimples = new List<ReferenceSimpleDto>();

            ReferenceIds = new ObservableCollection<string>();
            ReferenceNames = new ObservableCollection<string>();
            ProductNames = new ObservableCollection<string>();

            LineIds = new ObservableCollection<string>();
            LineNames = new ObservableCollection<string>();
        }

        public void SetReference(IEnumerable<ReferenceDto> references)
        {
            References = references.ToList();
            ReferenceSimples = references.SelectMany(i => i.UsableLines.Select(x => new ReferenceSimpleDto(
                i.ReferenceId,
                i.ReferenceName,
                i.ProductName,
                x.LineId,
                x.LineName,
                x.LineType))).ToList();

            ReferenceIds = new ObservableCollection<string>(ReferenceSimples.Select(i => i.ReferenceId).Distinct().OrderBy(s => s));
            ReferenceNames = new ObservableCollection<string>(ReferenceSimples.Select(i => i.ReferenceName).Distinct().OrderBy(s => s));
            ProductNames = new ObservableCollection<string>(ReferenceSimples.Select(i => i.ProductName).Distinct().OrderBy(s => s));

            LineIds = new ObservableCollection<string>(ReferenceSimples.Select(i => i.LineId).Distinct().OrderBy(s => s));
            LineNames = new ObservableCollection<string>(ReferenceSimples.Select(i => i.LineName).Distinct().OrderBy(s => s));
        }
    }
}
