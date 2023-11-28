using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Commands;
using WEMBLEY.DemoApp.Core.Application.Stores;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1
{
    public class StopperMachineViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public IViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;
        public string Header { get; set; } = "aaaaaaaaaaa";
        public ICommand NavigateBackToHomeViewCommand { get; set; }
        public StopperMachineViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            //NavigateBackToHomeViewCommand = new NavigateCommand(navigationService);

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
