using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Commands;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine
{
    public class MachinesInLine1ViewModel : BaseViewModel
    {
        private readonly ISignalRClient _signalRClient;
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
                            ColorBack = "#394963";
                            break;
                        }
                }
            }
        }
        public string ColorBack { get; set; } = "#394963";

        public ICommand NavigateToStopperMachineViewCommand { get; set; }
        public ICommand LoadMachinesInLine1ViewCommand { get; set; }
        public MachinesInLine1ViewModel(INavigationService navigationService, ISignalRClient signalRClient)
        {
            NavigationService = navigationService; 
            _signalRClient = signalRClient;

            NavigateToStopperMachineViewCommand = new RelayCommand(NavigationService.NavigateTo<StopperMachineViewModel>);
            LoadMachinesInLine1ViewCommand = new RelayCommand(LoadMachinesInLine1View);

            signalRClient.OnTagChanged += OnTagChanged;
        }

        private async void LoadMachinesInLine1View()
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

            if(tag != null)
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
