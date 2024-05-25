using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;

public class MachinesInLine1ViewModel : BaseViewModel
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
    public ICommand NavigateToStopperMachineViewCommand { get; set; }
    public ICommand LoadMachinesInLine1ViewCommand { get; set; }
    public MachinesInLine1ViewModel(INavigationService navigationService, ISignalRClient signalRClient, DeviceSelectedStore deviceSelectedStore)
    {
        NavigationService = navigationService; 
        _signalRClient = signalRClient;
        _deviceSelectedStore = deviceSelectedStore;

        NavigateToStopperMachineViewCommand = new RelayCommand(ClickCommand);
        LoadMachinesInLine1ViewCommand = new RelayCommand(LoadMachinesInLine1View);

        signalRClient.OnTagChanged += OnTagChanged;

        DelayLoading();
    }

    private async void DelayLoading()
    {
        await Task.Delay(5000);
        IsLoading = false;
    }

    private void ClickCommand()
    {
        _navigationService.NavigateTo<StopperMachineViewModel>();
        _deviceSelectedStore.SetSeletedDevice("IE-F2-HCA01", "HerapinCap");
    }

    private async void LoadMachinesInLine1View()
    {
        var a = await _signalRClient.GetBufferList();
        if (a.Count != 0)
        {
            Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F2-HCA01", "machineStatus"));
        }
    }

    private void OnTagChanged(string json)
    {
        var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);

        if(tag != null)
        {
            if (tag.StationId == "IE-F2-HCA01")
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
