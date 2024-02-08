
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Services;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Home;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDatabaseSynchronizationService _databaseSynchronizationService;
        private INavigationService? _navigationService;
        private readonly ISignalRClient _signalRClient;
        public INavigationService? NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        public StopperMachineViewModel StopperMachine { get; set; }
        public ICommand LoadMainWindowCommand { get; set; }
        public ICommand RunSignalRCommand { get; set; }

        public MainViewModel(IDatabaseSynchronizationService databaseSynchronizationService, INavigationService navigationService, ISignalRClient signalRClient, StopperMachineViewModel stopperMachine)
        {
            _databaseSynchronizationService = databaseSynchronizationService;
            NavigationService = navigationService;
            _signalRClient = signalRClient;
            LoadMainWindowCommand = new RelayCommand(NavigationService.NavigateTo<HomeNavigationViewModel>);
            RunSignalRCommand = new RelayCommand(InitialRunning);
            StopperMachine = stopperMachine;
        }

        private async void InitialRunning()
        {
            try
            {
                await _signalRClient.ConnectAsync();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            await Task.WhenAll(
                _databaseSynchronizationService.SynchronizeReferencesData(),
                _databaseSynchronizationService.SynchronizeDevicesData(),
                _databaseSynchronizationService.SynchronizeHomeData(),
                _databaseSynchronizationService.SynchronizePersonsData()
                );
        }
    }
}
