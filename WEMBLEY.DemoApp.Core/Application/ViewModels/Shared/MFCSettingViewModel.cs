using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Shared
{
    public class MFCSettingViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ReferenceStore _referenceStore;
        private readonly HomeDataStore _homeDataStore;
        //Doi lai la Device
        private readonly DeviceStore _deviceStore;
        public ObservableCollection<string> DeviceIds => _deviceStore.DeviceIds;
        //
        private readonly DeviceSelectedStore _deviceSelectedStore;
        public string SeletedDeviceId => _deviceSelectedStore.SeletedDeviceId;
        public string DeviceId { get; set; } = "";
        //
        public ObservableCollection<MFCDto> MFCEntries { get; set; } = new();

        public ICommand LoadMFCSettingViewCommand { get; set; }
        public ICommand UpdateMFCCommand { get; set; }
        public ICommand LoadApiCommand { get; set; }
        public string HomeRefName => _homeDataStore.HomeRefName;
        public int HomeRefId { get; set; } = 0;
        public MFCSettingViewModel(IApiService apiService, ReferenceStore referenceStore, DeviceStore deviceStore, HomeDataStore homeDataStore, DeviceSelectedStore deviceSelectedStore)
        {
            _apiService = apiService;
            _referenceStore = referenceStore;
            _deviceStore = deviceStore;
            _homeDataStore = homeDataStore;
            _deviceSelectedStore = deviceSelectedStore;

            LoadMFCSettingViewCommand = new RelayCommand(LoadMFCSettingViewAsync);
            UpdateMFCCommand = new RelayCommand(UpdateMFCAsync);
            LoadApiCommand = new RelayCommand(LoadApi);
        }

        private void LoadMFCSettingViewAsync()
        {
            DeviceId = SeletedDeviceId;
            LoadApi();
        }
        private async void LoadApi()
        {
            try
            {
                if (!string.IsNullOrEmpty(HomeRefName))
                {
                    HomeRefId = _referenceStore.References.First(i => i.RefName == HomeRefName).Id;
                    var dtos = await _apiService.GetDeviceReferenceMFCAsync(HomeRefId, DeviceId);
                    if(dtos.Count() != 0)
                    {
                        var viewModels = dtos.Last().MFCs;
                        MFCEntries = new(viewModels);
                    }
                    else
                    {
                        MFCEntries = new();
                    }
                }
                else { }

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        public async void UpdateMFCAsync()
        {
            var fixDto = MFCEntries;
            if (MessageBox.Show("Xác nhận sửa MFC", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.FixMFCAsync(HomeRefId, DeviceId, fixDto);
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMFCSettingViewAsync();
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: " + ex.Message);
                }
            }
            else { }


        }
    }
}
