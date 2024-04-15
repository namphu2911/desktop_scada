using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISignalRClient _signalRClient;
        //
        private EMachineStatus herapinCapStatus;
        public EMachineStatus HerapinCapStatus
        {
            get { return herapinCapStatus; }
            set
            {
                herapinCapStatus = value;
                switch (value)
                {
                    case EMachineStatus.On:
                        {
                            HerapinCapColorBack = "#394963";
                            break;
                        }
                    case EMachineStatus.Run:
                        {
                            HerapinCapColorBack = "#3EB17F";
                            break;
                        }
                    case EMachineStatus.Off:
                        {
                            HerapinCapColorBack = "#BBBBBB";
                            break;
                        }
                    case EMachineStatus.Alarm:
                        {
                            HerapinCapColorBack = "#ED5152";
                            break;
                        }
                    case EMachineStatus.Idle:
                        {
                            HerapinCapColorBack = "#FAAF24";
                            break;
                        }
                    case EMachineStatus.Setup:
                        {
                            HerapinCapColorBack = "#8B72C8";
                            break;
                        }
                    default:
                        {
                            HerapinCapColorBack = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        public string HerapinCapColorBack { get; set; } = "#BBBBBB";
        public double HerapinCapEfficiency { get; set; } = 0;
        public long HerapinCapAllCount { get; set; } = 0;
        public long HerapinCapGoodCount { get; set; } = 0;
        public long HerapinCapBadCount { get; set; } = 0;
        public TimeSpan? HerapinCapDurationTime { get; set; }
        public string HerapinCapProductName { get; set; } = "";
        public string HerapinCapReferenceName { get; set; } = "";
        public string HerapinCapLotId { get; set; } = "";
        public int HerapinCapLotSize { get; set; } = 0;

        //
        private EMachineStatus bloodTubeStatus;
        public EMachineStatus BloodTubeStatus
        {
            get { return bloodTubeStatus; }
            set
            {
                bloodTubeStatus = value;
                switch (value)
                {
                    case EMachineStatus.On:
                        {
                            BloodTubeColorBack = "#394963";
                            break;
                        }
                    case EMachineStatus.Run:
                        {
                            BloodTubeColorBack = "#3EB17F";
                            break;
                        }
                    case EMachineStatus.Off:
                        {
                            BloodTubeColorBack = "#BBBBBB";
                            break;
                        }
                    case EMachineStatus.Alarm:
                        {
                            BloodTubeColorBack = "#ED5152";
                            break;
                        }
                    case EMachineStatus.Idle:
                        {
                            BloodTubeColorBack = "#FAAF24";
                            break;
                        }
                    case EMachineStatus.Setup:
                        {
                            BloodTubeColorBack = "#8B72C8";
                            break;
                        }
                    default:
                        {
                            BloodTubeColorBack = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        public string BloodTubeColorBack { get; set; } = "#BBBBBB";
        public double BloodTubeEfficiency { get; set; } = 0;
        public long BloodTubeAllCount { get; set; } = 0;
        public long BloodTubeGoodCount { get; set; } = 0;
        public long BloodTubeBadCount { get; set; } = 0;
        public TimeSpan? BloodTubeDurationTime { get; set; }
        public string BloodTubeProductName { get; set; } = "";
        public string BloodTubeReferenceName { get; set; } = "";
        public string BloodTubeLotId { get; set; } = "";
        public int BloodTubeLotSize { get; set; } = 0;


        //
        public List<TagChangedNotification> AllTags { get; set; } = new();
        public ICommand LoadHomeViewCommand { get; set; }

        ///
        public MachinesInLine1ViewModel MachinesInLine1 { get; set; }
        public MachinesInLine2ViewModel MachinesInLine2 { get; set; }
        public HomeViewModel(IApiService apiService, ISignalRClient signalRClient, MachinesInLine1ViewModel machinesInLine1, MachinesInLine2ViewModel machinesInLine2)
        {
            _apiService = apiService;
            _signalRClient = signalRClient;

            MachinesInLine1 = machinesInLine1;
            MachinesInLine2 = machinesInLine2;

            LoadHomeViewCommand = new RelayCommand(LoadHomeView);

            signalRClient.OnTagChanged += OnTagChanged;
        }

        private async void LoadHomeView()
        {
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                HerapinCapStatus = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F2-HCA01","machineStatus"));
                HerapinCapDurationTime = TimeSpan.TryParse(Convert.ToString((await _signalRClient.GetBufferValue("IE-F2-HCA01","operationTime"))), out var span) ? span : default;
                HerapinCapGoodCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","goodProduct"));
                HerapinCapBadCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","errorProduct"));
                HerapinCapEfficiency = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","EFF"));
                HerapinCapAllCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","productCount"));

                BloodTubeStatus = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "machineStatus"));
                BloodTubeDurationTime = TimeSpan.TryParse(Convert.ToString((await _signalRClient.GetBufferValue("IE-F3-BLO06", "operationTime"))), out var span2) ? span2 : default;
                BloodTubeGoodCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "goodProduct"));
                BloodTubeBadCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "errorProduct"));
                BloodTubeEfficiency = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06", "EFF"));
                BloodTubeAllCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "productCount"));
            }
            LoadLotSettingAsync();
        }

        private void OnTagChanged(string json)
        {
            LoadLotSettingAsync();
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                if (tag.StationId == "IE-F2-HCA01")
                {
                    switch (tag.TagId)
                    {
                        case "machineStatus":
                            {
                                HerapinCapStatus = (EMachineStatus)Convert.ToInt32(tag.TagValue);
                                LoadLotSettingAsync();
                                break;
                            }
                        case "operationTimeRaw": HerapinCapDurationTime = TimeSpan.Parse((string)tag.TagValue); break;
                        case "goodProductRaw": HerapinCapGoodCount = Convert.ToInt64(tag.TagValue); break;
                        case "errorProduct": HerapinCapBadCount = Convert.ToInt64(tag.TagValue); break;
                        case "EFF": HerapinCapEfficiency = Convert.ToDouble(tag.TagValue); break;
                        case "productCount": HerapinCapAllCount = Convert.ToInt64(tag.TagValue); break;
                        default: break;
                    }
                }
                else if (tag.StationId == "IE-F3-BLO06")
                {
                    switch (tag.TagId)
                    {
                        case "machineStatus":
                            {
                                BloodTubeStatus = (EMachineStatus)Convert.ToInt32(tag.TagValue);
                                LoadLotSettingAsync();
                                break;
                            }
                        case "operationTimeRaw": BloodTubeDurationTime = TimeSpan.Parse((string)tag.TagValue); break;
                        case "goodProductRaw": BloodTubeGoodCount = Convert.ToInt64(tag.TagValue); break;
                        case "errorProduct": BloodTubeBadCount = Convert.ToInt64(tag.TagValue); break;
                        case "EFF": BloodTubeEfficiency = Convert.ToDouble(tag.TagValue); break;
                        case "productCount": BloodTubeAllCount = Convert.ToInt64(tag.TagValue); break;
                        default: break;
                    }
                }   
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceAsync("HerapinCap");
                var dtos2 = await _apiService.GetLotDeviceReferenceByDeviceAsync("NonVacuumBloodTube");
                
                HerapinCapLotId = dtos.Last().LotCode;
                HerapinCapLotSize = dtos.Last().LotSize; 
                HerapinCapLotId = dtos.Last().LotCode;
                HerapinCapLotSize = dtos.Last().LotSize;
                if (string.IsNullOrEmpty(HerapinCapLotId))
                {
                    HerapinCapProductName = "";
                    HerapinCapReferenceName = "";
                }
                else
                {
                    HerapinCapProductName = dtos.Last().ProductName;
                    HerapinCapReferenceName = dtos.Last().ReferenceName;
                }

                
                BloodTubeLotId = dtos2.Last().LotCode;
                BloodTubeLotSize = dtos2.Last().LotSize;
                if (string.IsNullOrEmpty(BloodTubeLotId))
                {
                    BloodTubeProductName = "";
                    BloodTubeReferenceName = "";
                }
                else
                {
                    BloodTubeProductName = dtos2.Last().ProductName;
                    BloodTubeReferenceName = dtos2.Last().ReferenceName;
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}