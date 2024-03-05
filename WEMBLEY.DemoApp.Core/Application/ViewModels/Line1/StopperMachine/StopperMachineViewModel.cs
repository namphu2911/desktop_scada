using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine
{
    public class StopperMachineViewModel : BaseViewModel
    {
        private bool isMFCTabSeleted;
        public bool IsMFCTabSeleted
        {
            get => isMFCTabSeleted;
            set
            {
                isMFCTabSeleted = value;
                ReloadData();
            }
        }
        public string IsMFCError { get; set; } = "#FFFFFF";

        public StopperMachineMonitorViewModel StopperMachineMonitor { get; set; }
        public FaultHistoryViewModel StopperMachineFault { get; set; }
        public MFCMonitorViewModel MFCMonitor { get; set; }
        public MFCSettingViewModel MFCSetting { get; set; }
        public int SeletedTabIndex { get; set; }
        public ReportLongTimeViewModel ReportLongTime { get; set; }
        public ReportForShiftViewModel ReportForShift { get; set; }
        public MachineStatusViewModel StopperMachineStatus { get; set; }

        private INavigationService? _navigationService;

        public INavigationService? NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }
        public ICommand NavigateBackToHomeViewCommand { get; set; }
        //
        //
        //
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
        public int HomeRefId => _homeDataStore.HomeDatas.First(i => i.DeviceType == "HerapinCap").RefId;
        public StopperMachineViewModel(INavigationService navigationService, StopperMachineMonitorViewModel stopperMachineMonitor, FaultHistoryViewModel stopperMachineFault, MFCMonitorViewModel mFCMonitor, MFCSettingViewModel mFCSetting, ReportLongTimeViewModel reportLongTime, ReportForShiftViewModel reportForShift, MachineStatusViewModel stopperMachineStatus, ISignalRClient signalRClient, IApiService apiService, ReferenceStore referenceStore, HomeDataStore homeDataStore)
        {
            NavigationService = navigationService;
            NavigateBackToHomeViewCommand = new RelayCommand(NavigationService.NavigateTo<HomeNavigationViewModel>);

            StopperMachineMonitor = stopperMachineMonitor;
            StopperMachineFault = stopperMachineFault;
            MFCMonitor = mFCMonitor;
            MFCSetting = mFCSetting;
            ReportLongTime = reportLongTime;
            ReportForShift = reportForShift;
            StopperMachineStatus = stopperMachineStatus;

            ReportLongTime.Changed += TabChanged;

            _signalRClient = signalRClient;
            _apiService = apiService;
            _referenceStore = referenceStore;
            _homeDataStore = homeDataStore;

            signalRClient.OnTagChanged += OnTagChanged;
            LoadMFCMonitorViewCommand = new RelayCommand(LoadMFCMonitorViewAsync);

        }

        private void TabChanged()
        {
            SeletedTabIndex = 1;
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
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_HEIGHT_MINIMUM_VALUE_TR4")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_MAXIMUM_HEIGHT_VALUE_TR4"))
                };
                OnPropertyChanged(nameof(RealMFCValues));

                HcMFC = new(
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
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_HEIGHT_MINIMUM_VALUE_TR4")),
                    Convert.ToDouble(await _signalRClient.GetBufferValue("S9_MAXIMUM_HEIGHT_VALUE_TR4")));
            }
            else
            {
                RealMFCValues = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                HcMFC = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }

            OnPropertyChanged(nameof(HomeRefId));
            try
            {
                if (HomeRefId != 0)
                {
                    var dtos = await _apiService.GetDeviceReferenceMFCAsync(HomeRefId, "HC001");
                    MFCDtos = dtos.Last().MFCs;
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

        private void ReloadData()
        {
            var newViewModels = MFCDtos.Select((tag, index) => new ComparedMFC(tag.Name, tag.Value, tag.MinValue, tag.MaxValue, RealMFCValues[index])).ToList();
            MFCEntries = new(newViewModels);
            var a = MFCEntries.Select(i => i.IsAlarmed);
            if (a.Contains(true))
            {
                IsMFCError = "#ED5152";
            }
            else
            {
                if (IsMFCTabSeleted)
                {
                    IsMFCError = "#4169e1";
                }
                else
                {
                    IsMFCError = "#FFFFFF";
                }
            }
        }
    }
}
