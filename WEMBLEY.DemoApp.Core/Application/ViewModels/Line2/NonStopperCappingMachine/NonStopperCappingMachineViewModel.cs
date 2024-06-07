using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.StopperCappingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.NonStopperCappingMachine
{
    public class NonStopperCappingMachineViewModel : BaseViewModel
    {
        public NonStopperCappingMonitorViewModel NonStopperCappingMonitor { get; set; }
        public FaultHistoryViewModel FaultHistory { get; set; }
        public NonStopperCappingParameterViewModel NonStopperCappingParameter { get; set; }
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

        public NonStopperCappingMachineViewModel(INavigationService navigationService, NonStopperCappingMonitorViewModel nonStopperCappingMonitor, FaultHistoryViewModel faultHistory, NonStopperCappingParameterViewModel nonStopperCappingParameter, MFCSettingViewModel mFCSetting, ReportLongTimeViewModel reportLongTime, ReportForShiftViewModel reportForShift, MachineStatusViewModel machineStatus)
        {
            NavigationService = navigationService;
            NavigateBackToHomeViewCommand = new RelayCommand(NavigationService.NavigateTo<HomeNavigationViewModel>);

            NonStopperCappingMonitor = nonStopperCappingMonitor;
            FaultHistory = faultHistory;
            NonStopperCappingParameter = nonStopperCappingParameter;
            MFCSetting = mFCSetting;
            ReportLongTime = reportLongTime;
            ReportForShift = reportForShift;
            MachineStatus = machineStatus;

            ReportLongTime.Changed += TabChanged;
        }

        private void TabChanged()
        {
            SeletedTabIndex = 1;
        }
    }
}
