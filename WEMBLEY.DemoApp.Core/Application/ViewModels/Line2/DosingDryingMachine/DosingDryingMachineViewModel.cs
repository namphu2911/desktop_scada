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
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine
{
    public class DosingDryingMachineViewModel : BaseViewModel
    {
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
        
        public DosingDryingMachineViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateBackToHomeViewCommand = new RelayCommand(NavigationService.NavigateTo<HomeNavigationViewModel>);
        }
    }
}
