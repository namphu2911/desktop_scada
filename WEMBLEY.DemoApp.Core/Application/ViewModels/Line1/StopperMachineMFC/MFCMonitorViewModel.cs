using AutoMapper;
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
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineMFC
{
    public class MFCMonitorViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISignalRClient _signalRClient;
        private readonly ReferenceStore _referenceStore;
        private readonly HomeDataStore _homeDataStore;

        public HerapinCapMFC HcMFC { get; set; } = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public ObservableCollection<ComparedMFC> MFCEntries { get; set; } = new();
        public List<MFCDto> MFCDtos { get; set; } = new();
        public List<double?> RealMFCValues { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();
        public ICommand LoadMFCMonitorViewCommand { get; set; }
        public string HomeRefName => _homeDataStore.HomeRefName;

        public MFCMonitorViewModel(ISignalRClient signalRClient, IApiService apiService, ReferenceStore referenceStore, HomeDataStore homeDataStore)
        {
            _signalRClient = signalRClient;
            _apiService = apiService;   
            _referenceStore = referenceStore;
            _homeDataStore = homeDataStore;

            signalRClient.OnTagChanged += OnTagChanged;
            LoadMFCMonitorViewCommand = new RelayCommand(LoadMFCMonitorViewAsync);
        }

        private async void LoadMFCMonitorViewAsync()
        {
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                RealMFCValues = new List<double?>
                {
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S8_OFF_SET_TR1")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S8_MINIMUN_HEIGHT_VALUE_TR1")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S8_MAXIMUM_HEIGHT_VALUE_TR1")),

                    Convert.ToDouble(await _signalRClient.GetBufferValue("S8_OFF_SET_TR3")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S8_MINIMUM_HEIGHT_VALUE_TR3")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S8_MAXIMUM_HEIGHT_VALUE_TR3")),

                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_OFF_SET_TR2")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_MINIMUM_HEIGHT_VALUE_TR2")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_MAXIMUM_HEIGHT_VALUE_TR2")),

                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_OFF_SET_TR4")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_MINIMUM_HEIGHT_VALUE_TR4")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_MAXIMUM_HEIGHT_VALUE_TR4"))
                };
                OnPropertyChanged(nameof(RealMFCValues));
            }
            else
            {
                RealMFCValues = new List<double?> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            OnPropertyChanged(nameof(HomeRefName));
            try
            {
                var homeRefId = _referenceStore.References.First(i => i.RefName == HomeRefName).Id;
                var dtos = await _apiService.GetDeviceReferenceMFCAsync(homeRefId, "HC001");
                MFCDtos = dtos.Last().MFCs;
                var newViewModels = MFCDtos.Select((tag, index) => new ComparedMFC(tag.Name, tag.Value, tag.MinValue, tag.MaxValue, RealMFCValues[index])).ToList();
                MFCEntries = new(newViewModels);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private void ReloadData()
        {
            var newViewModels = MFCDtos.Select((tag, index) => new ComparedMFC(tag.Name, tag.Value, tag.MinValue, tag.MaxValue, RealMFCValues[index])).ToList();
            MFCEntries = new(newViewModels);
        }

        private void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                switch (tag.TagId)
                {
                    case "S8_OFF_SET_TR1": HcMFC.S8OffsetTR1 = Convert.ToDouble(tag.TagValue); break;
                    case "S8_MINIMUN_HEIGHT_VALUE_TR1": HcMFC.S8MinHeightTR1 = Convert.ToDouble(tag.TagValue); break;
                    case "S8_MAXIMUM_HEIGHT_VALUE_TR1": HcMFC.S8MaxHeightTR1 = Convert.ToDouble(tag.TagValue); break;

                    case "S8_OFF_SET_TR3": HcMFC.S8OffsetTR3 = Convert.ToDouble(tag.TagValue); break;
                    case "S8_MINIMUM_HEIGHT_VALUE_TR3": HcMFC.S8MinHeightTR3 = Convert.ToDouble(tag.TagValue); break;
                    case "S8_MAXIMUM_HEIGHT_VALUE_TR3": HcMFC.S8MaxHeightTR3 = Convert.ToDouble(tag.TagValue); break;

                    case "S9_OFF_SET_TR2": HcMFC.S9OffsetTR2 = Convert.ToDouble(tag.TagValue); break;
                    case "S9_MINIMUM_HEIGHT_VALUE_TR2": HcMFC.S9MinHeightTR2 = Convert.ToDouble(tag.TagValue); break;
                    case "S9_MAXIMUM_HEIGHT_VALUE_TR2": HcMFC.S9MaxHeightTR2 = Convert.ToDouble(tag.TagValue); break;

                    case "S9_OFF_SET_TR4": HcMFC.S9OffsetTR4 = Convert.ToDouble(tag.TagValue); break;
                    case "S9_MINIMUM_HEIGHT_VALUE_TR4": HcMFC.S9MinHeightTR4 = Convert.ToDouble(tag.TagValue); break;
                    case "S9_MAXIMUM_HEIGHT_VALUE_TR4": HcMFC.S9MaxHeightTR4 = Convert.ToDouble(tag.TagValue); break;

                    default: break;
                }
            }

            RealMFCValues = new List<double?>
            {
                HcMFC.S8OffsetTR1, HcMFC.S8MinHeightTR1, HcMFC.S8MaxHeightTR1,
                HcMFC.S8OffsetTR3, HcMFC.S8MinHeightTR3, HcMFC.S8MaxHeightTR3,
                HcMFC.S9OffsetTR2, HcMFC.S9MinHeightTR2, HcMFC.S9MaxHeightTR2,
                HcMFC.S9OffsetTR4, HcMFC.S9MinHeightTR4, HcMFC.S9MaxHeightTR4
            };
            OnPropertyChanged(nameof(RealMFCValues));
            ReloadData();
        }
    }
}
