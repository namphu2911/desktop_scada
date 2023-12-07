using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Commands;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine
{
    public class MachinesInLine1ViewModel : BaseViewModel
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
        public ICommand NavigateToStopperMachineViewCommand { get; set; }
        public MachinesInLine1ViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateToStopperMachineViewCommand = new RelayCommand(NavigationService.NavigateTo<StopperMachineViewModel>);
        }
    }
}
