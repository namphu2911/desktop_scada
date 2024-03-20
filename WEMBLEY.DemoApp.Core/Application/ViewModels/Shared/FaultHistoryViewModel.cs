using AutoMapper;
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
using WEMBLEY.DemoApp.Core.Domain.Dtos.ErrorInformations;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Shared
{
    public class FaultHistoryViewModel : BaseViewModel
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
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-65).Date;
        public DateTime EndDate { get; set; } = DateTime.Today.Date;
        public ICommand LoadMachineFaultHistoryCommand { get; set; }
        public ICommand LoadApiCommand { get; set; }
        public ObservableCollection<ErrorStatusDto> ErrorsHistoryEntries { get; set; } = new();
        public FaultHistoryViewModel(IApiService apiService, StationStore deviceStore, DeviceSelectedStore deviceSelectedStore)
        {
            _apiService = apiService;
            _deviceStore = deviceStore;
            _deviceSelectedStore = deviceSelectedStore;

            LoadMachineFaultHistoryCommand = new RelayCommand(LoadMachineFaultHistory);
            LoadApiCommand = new RelayCommand(LoadApi);
        }

        private void LoadMachineFaultHistory()
        {
            DeviceId = SeletedDeviceId;
            LoadApi();
        }

        private async void LoadApi()
        {
            try
            {
                var dtos = await _apiService.GetErrorsHistoryAsync(DeviceId, StartDate, EndDate);
                ErrorsHistoryEntries = new(dtos);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
