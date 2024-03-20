using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine
{
    public class DosingDryingMachineViewModel : BaseViewModel
    {
        //private bool isMFCTabSeleted;
        //public bool IsMFCTabSeleted
        //{
        //    get => isMFCTabSeleted;
        //    set
        //    {
        //        isMFCTabSeleted = value;
        //        ReloadData();
        //    }
        //}
        public string IsMFCError { get; set; } = "#FFFFFF";

        public DosingDryingMonitorViewModel DosingDryingMonitor { get; set; }
        public FaultHistoryViewModel FaultHistory { get; set; }
        public DosingDryingParameterMonitorViewModel DosingDryingParameterMonitor { get; set; }
        public MFCSettingViewModel MFCSetting { get; set; }
        public int SeletedTabIndex { get; set; }
        public ReportLongTimeViewModel ReportLongTime { get; set; }
        public ReportForShiftViewModel ReportForShift { get; set; }
        public MachineStatusViewModel MachineStatus { get; set; }

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
        public string HomeRefId => _homeDataStore.HomeDatas.First(i => i.Line.LineId == "NonVacuumBloodTube").ReferenceId;

        //Đoạn code báo đỏ MFC




        //
        public DosingDryingMachineViewModel(INavigationService navigationService, DosingDryingMonitorViewModel dosingDryingMonitor, FaultHistoryViewModel faultHistory, DosingDryingParameterMonitorViewModel dosingDryingParameterMonitor, MFCSettingViewModel mFCSetting, ReportLongTimeViewModel reportLongTime, ReportForShiftViewModel reportForShift, MachineStatusViewModel machineStatus, ISignalRClient signalRClient, IApiService apiService, ReferenceStore referenceStore, HomeDataStore homeDataStore)
        {
            NavigationService = navigationService;
            NavigateBackToHomeViewCommand = new RelayCommand(NavigationService.NavigateTo<HomeNavigationViewModel>);

            DosingDryingMonitor = dosingDryingMonitor;
            FaultHistory = faultHistory;
            DosingDryingParameterMonitor = dosingDryingParameterMonitor;
            MFCSetting = mFCSetting;
            ReportLongTime = reportLongTime;
            ReportForShift = reportForShift;
            MachineStatus = machineStatus;

            ReportLongTime.Changed += TabChanged;

            _signalRClient = signalRClient;
            _apiService = apiService;
            _referenceStore = referenceStore;
            _homeDataStore = homeDataStore;

            //signalRClient.OnTagChanged += OnTagChanged;
            //LoadMFCMonitorViewCommand = new RelayCommand(LoadMFCMonitorViewAsync);
        }

        private void TabChanged()
        {
            SeletedTabIndex = 1;
        }
    }
}
