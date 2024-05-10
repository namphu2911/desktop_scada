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
        public StopperMachineViewModel(INavigationService navigationService, StopperMachineMonitorViewModel stopperMachineMonitor, FaultHistoryViewModel stopperMachineFault, MFCMonitorViewModel mFCMonitor, MFCSettingViewModel mFCSetting, ReportLongTimeViewModel reportLongTime, ReportForShiftViewModel reportForShift, MachineStatusViewModel stopperMachineStatus)
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
        }

        private void TabChanged()
        {
            SeletedTabIndex = 1;
        }
    }
}
