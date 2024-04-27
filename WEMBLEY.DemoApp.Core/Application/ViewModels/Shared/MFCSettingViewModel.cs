﻿using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Services;
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
        private readonly StationStore _deviceStore;

        private readonly RoleEnableStore _roleEnableStore;
        public ObservableCollection<string> DeviceIds => _deviceStore.StationIds;
        public ObservableCollection<string> ReferenceIds => _referenceStore.ReferenceIds;

        public ObservableCollection<string> DeviceIdsFilled { get; set; } = new();
        public ObservableCollection<string> ReferenceIdsFilled { get; set; } = new();
        //
        private readonly DeviceSelectedStore _deviceSelectedStore;
        public string SeletedDeviceId => _deviceSelectedStore.SeletedDeviceId;
        private string deviceId = "";

        public string DeviceId 
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
                var sortedStations = _deviceStore.Stations.Where(i => i.StationId == deviceId).ToList();
                ReferenceIdsFilled = new ObservableCollection<string>(sortedStations.Select(i => i.ReferenceId).Distinct().OrderBy(s => s));
                OnPropertyChanged(nameof(ReferenceIdsFilled));
            }
        }

        public string ReferenceId { get; set; } = "";

        //
        public ObservableCollection<MFCDto> MFCEntries { get; set; } = new();
        public bool RoleEnabled => _roleEnableStore.IsMFCEnabled;
        public bool ReadOnly => (!RoleEnabled);
        public ICommand LoadMFCSettingViewCommand { get; set; }
        public ICommand UpdateMFCCommand { get; set; }
        public ICommand LoadApiCommand { get; set; }
        public string HomeRefId => _homeDataStore.HomeDatas.First(i => i.Line.LineId == _deviceSelectedStore.LineId).ReferenceId;
        //

        public event Action? UpdateMFCApi;
        public MFCSettingViewModel(IApiService apiService, ReferenceStore referenceStore, StationStore deviceStore, HomeDataStore homeDataStore, DeviceSelectedStore deviceSelectedStore, RoleEnableStore roleEnableStore)
        {
            _apiService = apiService;
            _referenceStore = referenceStore;
            _deviceStore = deviceStore;
            _homeDataStore = homeDataStore;
            _deviceSelectedStore = deviceSelectedStore;
            _roleEnableStore = roleEnableStore;

            LoadMFCSettingViewCommand = new RelayCommand(LoadMFCSettingViewAsync);
            UpdateMFCCommand = new RelayCommand(UpdateMFCAsync);
            LoadApiCommand = new RelayCommand(LoadApi);
        }

        private void LoadMFCSettingViewAsync()
        {
            DeviceIdsFilled = new(DeviceIds);
            DeviceId = SeletedDeviceId;
            ReferenceId = HomeRefId;
            LoadApi();
        }
        private async void LoadApi()
        {
            try
            {
                if (!(String.IsNullOrEmpty(HomeRefId)))
                {
                    var dtos = await _apiService.GetStationReferencesMFCAsync(DeviceId, ReferenceId);
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
                    UpdateMFCApi?.Invoke();
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
