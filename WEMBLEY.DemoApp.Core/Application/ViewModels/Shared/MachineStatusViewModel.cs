using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.MachineStatus;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Shared
{
    public class MachineStatusViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        //Doi lai la Device
        private readonly StationStore _deviceStore;
        public ObservableCollection<string> DeviceIds => _deviceStore.StationIds;
        //
        private readonly DeviceSelectedStore _deviceSelectedStore;
        public string SeletedDeviceId => _deviceSelectedStore.SeletedDeviceId;
        public string DeviceId { get; set; } = "";
        //
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7).Date;
        public DateTime EndDate { get; set; } = DateTime.Today.Date;
        public ICommand LoadMachineStatusHistoryCommand { get; set; }
        public ICommand LoadApiCommand { get; set; }
        public ObservableCollection<MachineStatusDto> StatusHistoryEntries { get; set; } = new();
        public MachineStatusViewModel(IApiService apiService, StationStore deviceStore, DeviceSelectedStore deviceSelectedStore)
        {
            _apiService = apiService;
            _deviceStore = deviceStore;
            _deviceSelectedStore = deviceSelectedStore;
            LoadMachineStatusHistoryCommand = new RelayCommand(LoadMachineStatusHistory);
            LoadApiCommand = new RelayCommand(LoadApi);
        }

        private void LoadMachineStatusHistory()
        {
            DeviceId = SeletedDeviceId;
            LoadApi();
        }

        private async void LoadApi()
        {
            try
            {
                var dtos = await _apiService.GetStatusHistoryAsync(DeviceId, StartDate, EndDate);
                StatusHistoryEntries = new(dtos);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
