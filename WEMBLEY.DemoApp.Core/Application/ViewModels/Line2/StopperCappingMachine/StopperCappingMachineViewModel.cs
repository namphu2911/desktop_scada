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
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.StopperCappingMachine
{
    public class StopperCappingMachineViewModel : BaseViewModel
    {
        private bool isMFCTabSeleted;
        public bool IsMFCTabSeleted
        {
            get => isMFCTabSeleted;
            set
            {
                isMFCTabSeleted = value;
                //ReloadData();
            }
        }
        public string IsMFCError { get; set; } = "#FFFFFF";

        public StopperCappingMonitorViewModel StopperCappingMonitor { get; set; }
        public FaultHistoryViewModel FaultHistory { get; set; }
        public StopperCappingParameterViewModel StopperCappingParameter { get; set; }
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
        private readonly HomeDataStore _homeDataStore;

       
        public StopperCappingMachineViewModel(INavigationService navigationService, StopperCappingMonitorViewModel stopperCappingMonitor, FaultHistoryViewModel faultHistory, StopperCappingParameterViewModel stopperCappingParameter, MFCSettingViewModel mFCSetting, ReportLongTimeViewModel reportLongTime, ReportForShiftViewModel reportForShift, MachineStatusViewModel machineStatus, ISignalRClient signalRClient, IApiService apiService, HomeDataStore homeDataStore)
        {
            NavigationService = navigationService;
            NavigateBackToHomeViewCommand = new RelayCommand(NavigationService.NavigateTo<HomeNavigationViewModel>);

            StopperCappingMonitor = stopperCappingMonitor;
            FaultHistory = faultHistory;
            StopperCappingParameter = stopperCappingParameter;
            MFCSetting = mFCSetting;
            ReportLongTime = reportLongTime;
            ReportForShift = reportForShift;
            MachineStatus = machineStatus;

            ReportLongTime.Changed += TabChanged;
            //MFCSetting.UpdateMFCApi += LoadMFCMonitorViewAsync;

            _signalRClient = signalRClient;
            _apiService = apiService;
            _homeDataStore = homeDataStore;

            //signalRClient.OnTagChanged += OnTagChanged;
        }

        private void TabChanged()
        {
            SeletedTabIndex = 1;
        }
    }
}
