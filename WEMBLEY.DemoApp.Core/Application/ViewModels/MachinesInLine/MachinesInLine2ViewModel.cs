using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.NonStopperCappingMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.StopperCappingMachine;
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
        private EMachineStatus statusDosing;
        public EMachineStatus StatusDosing
        {
            get { return statusDosing; }
            set
            {
                statusDosing = value;
                switch (value)
                {
                    case EMachineStatus.On:
                        {
                            ColorBackDosing = "#394963";
                            break;
                        }
                    case EMachineStatus.Run:
                        {
                            ColorBackDosing = "#3EB17F";
                            break;
                        }
                    case EMachineStatus.Off:
                        {
                            ColorBackDosing = "#BBBBBB";
                            break;
                        }
                    case EMachineStatus.Alarm:
                        {
                            ColorBackDosing = "#ED5152";
                            break;
                        }
                    case EMachineStatus.Idle:
                        {
                            ColorBackDosing = "#FAAF24";
                            break;
                        }
                    case EMachineStatus.Setup:
                        {
                            ColorBackDosing = "#8B72C8";
                            break;
                        }
                    default:
                        {
                            ColorBackDosing = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EMachineStatus statusStopper;
        public EMachineStatus StatusStopper
        {
            get { return statusStopper; }
            set
            {
                statusStopper = value;
                switch (value)
                {
                    case EMachineStatus.On:
                        {
                            ColorBackStopper = "#394963";
                            break;
                        }
                    case EMachineStatus.Run:
                        {
                            ColorBackStopper = "#3EB17F";
                            break;
                        }
                    case EMachineStatus.Off:
                        {
                            ColorBackStopper = "#BBBBBB";
                            break;
                        }
                    case EMachineStatus.Alarm:
                        {
                            ColorBackStopper = "#ED5152";
                            break;
                        }
                    case EMachineStatus.Idle:
                        {
                            ColorBackStopper = "#FAAF24";
                            break;
                        }
                    case EMachineStatus.Setup:
                        {
                            ColorBackStopper = "#8B72C8";
                            break;
                        }
                    default:
                        {
                            ColorBackStopper = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EMachineStatus statusNonStopper;
        public EMachineStatus StatusNonStopper
        {
            get { return statusNonStopper; }
            set
            {
                statusNonStopper = value;
                switch (value)
                {
                    case EMachineStatus.On:
                        {
                            ColorBackNonStopper = "#394963";
                            break;
                        }
                    case EMachineStatus.Run:
                        {
                            ColorBackNonStopper = "#3EB17F";
                            break;
                        }
                    case EMachineStatus.Off:
                        {
                            ColorBackNonStopper = "#BBBBBB";
                            break;
                        }
                    case EMachineStatus.Alarm:
                        {
                            ColorBackNonStopper = "#ED5152";
                            break;
                        }
                    case EMachineStatus.Idle:
                        {
                            ColorBackNonStopper = "#FAAF24";
                            break;
                        }
                    case EMachineStatus.Setup:
                        {
                            ColorBackNonStopper = "#8B72C8";
                            break;
                        }
                    default:
                        {
                            ColorBackNonStopper = "#BBBBBB";
                            break;
                        }
                }
            }
        }
        public string ColorBackDosing { get; set; } = "#BBBBBB";
        public string ColorBackStopper { get; set; } = "#BBBBBB";
        public string ColorBackNonStopper { get; set; } = "#BBBBBB";

        public bool IsLoading { get; set; } = true;

        public ICommand NavigateToDosingDryingMachineViewCommand { get; set; }
        public ICommand NavigateToStopperCappingViewCommand { get; set; }
        public ICommand NavigateToNonStopperCappingViewCommand { get; set; }
        public ICommand LoadMachinesInLine2ViewCommand { get; set; }
        public MachinesInLine2ViewModel(INavigationService navigationService, ISignalRClient signalRClient, DeviceSelectedStore deviceSelectedStore)
        {
            NavigationService = navigationService;
            _signalRClient = signalRClient;
            _deviceSelectedStore = deviceSelectedStore;

            NavigateToDosingDryingMachineViewCommand = new RelayCommand(NavigateToDosingDrying);
            NavigateToStopperCappingViewCommand = new RelayCommand(NavigateToStopperCapping);
            NavigateToNonStopperCappingViewCommand = new RelayCommand(NavigateToNonStopperCapping);
            LoadMachinesInLine2ViewCommand = new RelayCommand(LoadMachinesInLine2View);

            signalRClient.OnTagChanged += OnTagChanged;

            DelayLoading();
        }

        

        private async void DelayLoading()
        {
            await Task.Delay(5000);
            IsLoading = false;
        }

        private void NavigateToDosingDrying()
        {
            _navigationService.NavigateTo<DosingDryingMachineViewModel>();
            _deviceSelectedStore.SetSeletedDevice("IE-F3-BLO06", "NonVacuumBloodTube");
        }
        private void NavigateToStopperCapping()
        {
            _navigationService.NavigateTo<StopperCappingMachineViewModel>();
            _deviceSelectedStore.SetSeletedDevice("IE-F3-BLO01", "NonVacuumBloodTube");
        }
        private void NavigateToNonStopperCapping()
        {
            _navigationService.NavigateTo<NonStopperCappingMachineViewModel>();
            _deviceSelectedStore.SetSeletedDevice("IE-F3-BLO02", "NonVacuumBloodTube");
        }   

        private async void LoadMachinesInLine2View()
        {
            var a = await _signalRClient.GetBufferList();
            if (a.Count != 0)
            {
                StatusDosing = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "machineStatus"));
                StatusStopper = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO01", "machineStatus"));
                StatusNonStopper = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "machineStatus"));
            }
        }

        private void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);

            if (tag != null)
            {
                if (tag.StationId == "IE-F3-BLO06")
                {
                    switch (tag.TagId)
                    {
                        case "machineStatus":
                            {
                                StatusDosing = (EMachineStatus)Convert.ToInt32(tag.TagValue);
                                break;
                            }
                        default: break;
                    }
                }
                else if (tag.StationId == "IE-F3-BLO01")
                {
                    switch (tag.TagId)
                    {
                        case "machineStatus":
                            {
                                StatusStopper = (EMachineStatus)Convert.ToInt32(tag.TagValue);
                                break;
                            }
                        default: break;
                    }
                }
                else if (tag.StationId == "IE-F3-BLO02")
                {
                    switch (tag.TagId)
                    {
                        case "machineStatus":
                            {
                                StatusNonStopper = (EMachineStatus)Convert.ToInt32(tag.TagValue);
                                break;
                            }
                        default: break;
                    }
                }   
            }
        }
    }
}
