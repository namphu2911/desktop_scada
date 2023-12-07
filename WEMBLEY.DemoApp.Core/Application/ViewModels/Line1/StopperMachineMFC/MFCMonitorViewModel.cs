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
        private readonly HomeDataStore _homeDataStore;

        public ObservableCollection<MFCDto> MFCEntries { get; set; } = new();

        public ICommand LoadMFCMonitorViewCommand { get; set; }
        public string HomeRefName => _homeDataStore.HomeRefName;

        public MFCMonitorViewModel(IApiService apiService, ReferenceStore referenceStore, HomeDataStore homeDataStore)
        {
            _apiService = apiService;   
            _referenceStore = referenceStore;
            _homeDataStore = homeDataStore;

            LoadMFCMonitorViewCommand = new RelayCommand(LoadMFCMonitorViewAsync);
        }

        private async void LoadMFCMonitorViewAsync()
        {
            OnPropertyChanged(nameof(HomeRefName));
            try
            {
                var homeRefId = _referenceStore.References.First(i => i.RefName == HomeRefName).Id;
                var dtos = await _apiService.GetDeviceReferenceMFCAsync(homeRefId, "HC001");
                var viewModels = dtos.Last().MFCs;
                MFCEntries = new(viewModels);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
