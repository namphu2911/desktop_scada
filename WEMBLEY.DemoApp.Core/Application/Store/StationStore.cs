using System.Collections.ObjectModel;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Stations;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class StationStore : BaseViewModel
    {
        public List<StationReferenceInfoDto> Stations { get; private set; }
        public ObservableCollection<string> StationIds { get; private set; }
        public ObservableCollection<string> StationNames { get; private set; }
        public StationStore()
        {
            Stations = new List<StationReferenceInfoDto>();
            StationIds = new ObservableCollection<string>();
            StationNames = new ObservableCollection<string>();
        }

        public void SetStation(IEnumerable<StationReferenceInfoDto> stations)
        {
            Stations = stations.ToList();
            StationIds = new ObservableCollection<string>(Stations.Select(i => i.StationId).Distinct().OrderBy(s => s));
            StationNames = new ObservableCollection<string>(Stations.Select(i => i.StationName).Distinct().OrderBy(s => s));
        }

    }
}
