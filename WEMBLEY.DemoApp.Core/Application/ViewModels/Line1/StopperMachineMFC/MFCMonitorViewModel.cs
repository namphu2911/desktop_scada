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
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineMFC
{
    public class MFCMonitorViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ReferenceStore _referenceStore;
        public HomeViewModel _homeViewModel { get; set; }

        public ObservableCollection<MFCDto> MFCEntries { get; set; } = new();

        public ICommand LoadMFCMonitorViewCommand { get; set; }
        public string HomeRef { get; set; } = "";

        public MFCMonitorViewModel(IApiService apiService, ReferenceStore referenceStore, HomeViewModel homeViewModel)
        {
            _apiService = apiService;   
            _referenceStore = referenceStore;
            _homeViewModel = homeViewModel;

            LoadMFCMonitorViewCommand = new RelayCommand(LoadMFCMonitorViewAsync);
        }

        private async void LoadMFCMonitorViewAsync()
        {
            LoadLotSettingAsync();
            try
            {
                var dtos = await _apiService.GetAllLotDeviceReferenceAsync();
                var dto = dtos.First(i => i.RefName == HomeRef);
                var viewModels = dto.Devices.First(i => i.DeviceId == "HC001").MFCs;
                MFCEntries = new(viewModels);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceTypeAsync("HerapinCap");
                var dto = dtos.First(i => i.DeviceType == "HerapinCap");
                HomeRef = dto.RefName;
                OnPropertyChanged(nameof(HomeRef));
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
