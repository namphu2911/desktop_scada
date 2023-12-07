using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineDetail;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineMFC;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineReport;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineStatus;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1
{
    public class StopperMachineViewModel : BaseViewModel
    {
        public StopperMachineDetailViewModel StopperMachineDetail { get; set; }
        public MFCNavigationViewModel MFCNavigation { get; set; }
        public ReportNavigationViewModel ReportNavigation { get; set; }
        public StopperMachineStatusViewModel StopperMachineStatus { get; set; }

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
        public StopperMachineViewModel(INavigationService navigationService, StopperMachineDetailViewModel stopperMachineDetail, MFCNavigationViewModel mFCNavigation, ReportNavigationViewModel reportNavigation, StopperMachineStatusViewModel stopperMachineStatus)
        {
            NavigationService = navigationService;
            NavigateBackToHomeViewCommand = new RelayCommand(NavigationService.NavigateTo<HomeNavigationViewModel>);
            StopperMachineDetail = stopperMachineDetail;
            MFCNavigation = mFCNavigation;
            ReportNavigation = reportNavigation;
            StopperMachineStatus = stopperMachineStatus;
        }
        
    }
}
