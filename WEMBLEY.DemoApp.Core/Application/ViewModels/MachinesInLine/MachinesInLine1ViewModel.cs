using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Commands;
using WEMBLEY.DemoApp.Core.Application.Stores;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine
{
    public class MachinesInLine1ViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public IViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;
        public ICommand NavigateToStopperMachineViewCommand { get; set; }
        public MachinesInLine1ViewModel(NavigationStore navigationStore, INavigationService<StopperMachineViewModel> navigationService)
        {
            _navigationStore = navigationStore;
            NavigateToStopperMachineViewCommand = new NavigateCommand(navigationService);

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
