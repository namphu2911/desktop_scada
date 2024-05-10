using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
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
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.StopperCappingMachine
{
    public class StopperCappingParameterViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISignalRClient _signalRClient;
        private readonly HomeDataStore _homeDataStore;
        public ObservableCollection<ComparedMFC> MFCEntries { get; set; } = new();
        

        public List<MFCDto> MFCDtos { get; set; } = new();
        public List<RealMFC> RealMFCValues { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();
        public ICommand LoadMFCMonitorViewCommand { get; set; }
        public string HomeRefId => _homeDataStore.HomeDatas.First(i => i.Line.LineId == "NonVacuumBloodTube").ReferenceId;

        public StopperCappingParameterViewModel(ISignalRClient signalRClient, IApiService apiService, HomeDataStore homeDataStore)
        {
            _signalRClient = signalRClient;
            _apiService = apiService;
            _homeDataStore = homeDataStore;

            signalRClient.OnTagChanged += OnTagChanged;
            LoadMFCMonitorViewCommand = new RelayCommand(LoadMFCMonitorViewAsync);
        }

        private async void LoadMFCMonitorViewAsync()
        {
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                RealMFCValues = new()
                {
                    new RealMFC("Spray Valve", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_SPRAY_VALVE")),
                    
                };
                OnPropertyChanged(nameof(RealMFCValues));
            }
            else
            {
                RealMFCValues = new()
                {
                    new RealMFC("Spray Valve", 0),
                };

            }

            OnPropertyChanged(nameof(HomeRefId));
            try
            {
                if (!(String.IsNullOrEmpty(HomeRefId)))
                {
                    var dtos = await _apiService.GetStationReferencesMFCAsync("IE-F2-HCA01", HomeRefId);
                    if(dtos.ToList().Count != 0)
                    {
                        MFCDtos = dtos.Last().MFCs;
                    }
                }
                ReloadData();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                switch (tag.TagId)
                {
                    case "S1_PARA_SPRAY_VALVE":
                        RealMFCValues[0] = new RealMFC("Spray Valve", tag.TagValue);
                        break;
                    default: break;
                }
            }
            OnPropertyChanged(nameof(RealMFCValues));
            ReloadData();
        }

        private void ReloadData()
        {
            //var newViewModels = MFCDtos.Select((tag, index) => new ComparedMFC(tag.MFCName, tag.Value, tag.MinValue, tag.MaxValue, RealMFCValues[index])).ToList();
            //MFCEntries = new(newViewModels);
        }
    }
}