using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine
{
    public class MachinesInLine2ViewModel : BaseViewModel
    {
        private readonly ISignalRClient _signalRClient;
        private readonly DeviceSelectedStore _deviceSelectedStore;
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
        //
        private EMachineStatus status;
        public EMachineStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                switch (value)
                {
                    case EMachineStatus.On:
                        {
                            ColorBack = "#394963";
                            break;
                        }
                    case EMachineStatus.Run:
                        {
                            ColorBack = "#3EB17F";
                            break;
                        }
                    case EMachineStatus.Off:
                        {
                            ColorBack = "#BBBBBB";
                            break;
                        }
                    case EMachineStatus.Alarm:
                        {
                            ColorBack = "#ED5152";
                            break;
                        }
                    case EMachineStatus.Idle:
                        {
                            ColorBack = "#FAAF24";
                            break;
                        }
                    case EMachineStatus.Setup:
                        {
                            ColorBack = "#8B72C8";
                            break;
                        }
                    default:
                        {
                            ColorBack = "#BBBBBB";
                            break;
                        }
                }
            }
        }
        public string ColorBack { get; set; } = "#BBBBBB";
        public bool IsLoading { get; set; } = true;
        public ICommand NavigateToDosingDryingMachineViewCommand { get; set; }
        public ICommand LoadMachinesInLine2ViewCommand { get; set; }
        public MachinesInLine2ViewModel(INavigationService navigationService, ISignalRClient signalRClient, DeviceSelectedStore deviceSelectedStore)
        {
            NavigationService = navigationService;
            _signalRClient = signalRClient;
            _deviceSelectedStore = deviceSelectedStore;

            NavigateToDosingDryingMachineViewCommand = new RelayCommand(ClickCommand);
            LoadMachinesInLine2ViewCommand = new RelayCommand(LoadMachinesInLine2View);

            signalRClient.OnTagChanged += OnTagChanged;

            DelayLoading();
        }

        private async void DelayLoading()
        {
            await Task.Delay(6000);
            IsLoading = false;
        }

        private void ClickCommand()
        {
            _navigationService.NavigateTo<DosingDryingMachineViewModel>();
            _deviceSelectedStore.SetSeletedDevice("HC001");
        }

        private async void LoadMachinesInLine2View()
        {
            var a = await _signalRClient.GetBufferList();
            if (a.Count != 0)
            {
                Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("machineStatus"));
            }
        }

        private void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);

            if (tag != null)
            {
                switch (tag.TagId)
                {
                    case "machineStatus":
                        {
                            Status = (EMachineStatus)Convert.ToInt32(tag.TagValue);
                            break;
                        }
                    default: break;
                }
            }

        }
    }
}
