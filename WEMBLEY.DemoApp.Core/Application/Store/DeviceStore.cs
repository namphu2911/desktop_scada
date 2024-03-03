using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Devices;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class DeviceStore : BaseViewModel
    {
        public List<DeviceDto> Devices { get; private set; }
        public ObservableCollection<string> DeviceIds { get; private set; }
        public ObservableCollection<string> DeviceNames { get; private set; }
        public ObservableCollection<string> DeviceTypes { get; private set; }
        public DeviceStore()
        {
            Devices = new List<DeviceDto>();
            DeviceIds = new ObservableCollection<string>();
            DeviceNames = new ObservableCollection<string>();
            DeviceTypes = new ObservableCollection<string>();
        }

        public void SetDevice(IEnumerable<DeviceDto> devices)
        {
            Devices = devices.ToList();
            DeviceIds = new ObservableCollection<string>(Devices.Select(i => i.DeviceId).OrderBy(s => s));
            DeviceNames = new ObservableCollection<string>(Devices.Select(i => i.DeviceName).OrderBy(s => s));
            DeviceTypes = new ObservableCollection<string>(Devices.Select(i => i.DeviceType).Distinct().OrderBy(s => s));
        }

    }
}
