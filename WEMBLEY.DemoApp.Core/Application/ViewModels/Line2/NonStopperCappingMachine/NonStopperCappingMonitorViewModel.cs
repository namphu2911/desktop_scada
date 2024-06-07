using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.NonStopperCappingMachine
{
    public class NonStopperCappingMonitorViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISignalRClient _signalRClient;

        //General
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
        //
        public double Efficiency { get; set; } = 0;

        //Production Data
        public long AllProductCount { get; set; } = 0;
        public long PlasticTrayQuantity { get; set; } = 0;
        public TimeSpan? OperationTime { get; set; }
        public string NonStopperCappingProductName { get; set; } = "";
        public string NonStopperCappingReferenceName { get; set; } = "";
        public string NonStopperCappingLotId { get; set; } = "";
        public int NonStopperCappingLotSize { get; set; } = 0;

        //Vision
        public long VisionTotalTube { get; set; } = 0;
        public long VisionGoodTube { get; set; } = 0;
        public long VisionBadTube { get; set; } = 0;
        public long StyrofoamTrayQuantity { get; set; } = 0;
        public long CurrentGoodTube { get; set; } = 0;
        public long CurrentBadTube { get; set; } = 0;
        //
        public string ColorCap { get; set; } = "#BBBBBB";
        public string ColorTube { get; set; } = "#BBBBBB";
        public string ColorCommunication { get; set; } = "#BBBBBB";
        public string ColorLiftMotor { get; set; } = "#BBBBBB";
        public string ColorPushTray { get; set; } = "#BBBBBB";
        public string ColorVision { get; set; } = "#BBBBBB";


        private EStationEnable capEnable;
        public EStationEnable CapEnable
        {
            get { return capEnable; }
            set
            {
                capEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorCap = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorCap = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorCap = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable tubeEnable;
        public EStationEnable TubeEnable
        {
            get { return tubeEnable; }
            set
            {
                tubeEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorTube = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorTube = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorTube = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable communicationEnable;
        public EStationEnable CommunicationEnable
        {
            get { return communicationEnable; }
            set
            {
                communicationEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorCommunication = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorCommunication = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorCommunication = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable liftMotorEnable;
        public EStationEnable LiftMotorEnable
        {
            get { return liftMotorEnable; }
            set
            {
                liftMotorEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorLiftMotor = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorLiftMotor = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorLiftMotor = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable pushTrayEnable;
        public EStationEnable PushTrayEnable
        {
            get { return pushTrayEnable; }
            set
            {
                pushTrayEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorPushTray = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorPushTray = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorPushTray = "#BBBBBB";
                            break;
                        }
                }
            }
        }       

        private EStationEnable visionEnable;
        public EStationEnable VisionEnable
        {
            get { return visionEnable; }
            set
            {
                visionEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorVision = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorVision = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorVision = "#BBBBBB";
                            break;
                        }
                }
            }
        }   

        //Error
        public string? Error { get; set; }
        List<string> Errors { get; set; } = new();


        public ObservableCollection<string> ErrorStrings { get; set; } = new();
        public ObservableCollection<string> PersonStrings { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();

        //
        public ICommand LoadDosingDryingMonitorViewCommand { get; set; }
        public NonStopperCappingMonitorViewModel(IApiService apiService, ISignalRClient signalRClient)
        {
            _apiService = apiService;
            _signalRClient = signalRClient;
            LoadDosingDryingMonitorViewCommand = new RelayCommand(LoadDosingDryingMonitorView);

            signalRClient.OnTagChanged += OnTagChanged;
        }

        private async void LoadDosingDryingMonitorView()
        {
            LoadLotSettingAsync();
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "machineStatus"));
                OperationTime = TimeSpan.TryParse(Convert.ToString((await _signalRClient.GetBufferValue("IE-F3-BLO02", "operationTimeRaw"))), out var span) ? span : default;

                Efficiency = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO02", "EFF"));
                AllProductCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "productCountRaw"));
                PlasticTrayQuantity = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_PLASTIC_TRAYS_QTY"));

                VisionGoodTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_VISION_GOOD_TUBES"));
                VisionBadTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_VISION_BAD_TUBES"));
                VisionTotalTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_VISION_TOTAL_TUBES"));
                StyrofoamTrayQuantity = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_STYROFOAM_TRAYS_QTY"));
                CurrentGoodTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_VISION_CURRENT_GDS"));
                CurrentBadTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_VISION_CURRENT_BDS"));

                CapEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_CAP_DISABLE"));
                TubeEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_TUBE_DISABLE"));
                CommunicationEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_COMMUNICATION_DISABLE"));
                LiftMotorEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_LIFT_MOTOR_DISABLE"));
                PushTrayEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_PUSH_TRAY_DISABLE"));
                VisionEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO02", "S3_VISION_ENABLE"));


                var errorTags = AllTags.Where(i => i.TagId == "errorStatus" && i.StationId == "IE-F3-BLO02");
                foreach (var tag in errorTags)
                {
                    Error = $"{tag.TimeStamp:MM/dd/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                    if (!(Errors.Contains(Error)))
                    {
                        Errors.Add(Error);
                        ErrorStrings = new(Errors);
                    }
                }
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                PersonStrings = new();
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceAsync("NonVacuumBloodTube");
                NonStopperCappingLotId = dtos.Last().LotCode;
                NonStopperCappingLotSize = dtos.Last().LotSize;
                if (string.IsNullOrEmpty(NonStopperCappingLotId))
                {
                    NonStopperCappingProductName = "";
                    NonStopperCappingReferenceName = "";
                }
                else
                {
                    NonStopperCappingProductName = dtos.Last().ProductName;
                    NonStopperCappingReferenceName = dtos.Last().ReferenceName;
                }
                if (dtos.First().Stations.Count() != 0)
                {
                    var persons = dtos.First().Stations.First(i => i.StationId == "IE-F3-BLO02").Employees;
                    foreach (var person in persons)
                    {
                        PersonStrings.Add($"{person.EmployeeId} - {person.EmployeeName}");
                    }
                    OnPropertyChanged(nameof(PersonStrings));
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        public void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                if (tag.StationId == "IE-F3-BLO02")
                {
                    switch (tag.TagId)
                    {
                        case "machineStatus": Status = (EMachineStatus)Convert.ToInt32(tag.TagValue); break;
                        case "operationTimeRaw": OperationTime = TimeSpan.Parse((string)tag.TagValue); break;

                        case "EFF": Efficiency = Convert.ToDouble(tag.TagValue); break;
                        case "productCountRaw": AllProductCount = Convert.ToInt64(tag.TagValue); break;
                        case "S3_PLASTIC_TRAYS_QTY": PlasticTrayQuantity = Convert.ToInt64(tag.TagValue); break;

                        case "S3_VISION_GOOD_TUBES": VisionGoodTube = Convert.ToInt64(tag.TagValue); break;
                        case "S3_VISION_BAD_TUBES": VisionBadTube = Convert.ToInt64(tag.TagValue); break;
                        case "S3_VISION_TOTAL_TUBES": VisionTotalTube = Convert.ToInt64(tag.TagValue); break;
                        case "S3_STYROFOAM_TRAYS_QTY": StyrofoamTrayQuantity = Convert.ToInt64(tag.TagValue); break;
                        case "S3_VISION_CURRENT_GDS": CurrentGoodTube = Convert.ToInt64(tag.TagValue); break;
                        case "S3_VISION_CURRENT_BDS": CurrentBadTube = Convert.ToInt64(tag.TagValue); break;

                        case "S3_CAP_DISABLE": CapEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S3_TUBE_DISABLE": TubeEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S3_COMMUNICATION_DISABLE": CommunicationEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S3_LIFT_MOTOR_DISABLE": LiftMotorEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S3_PUSH_TRAY_DISABLE": PushTrayEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S3_VISION_ENABLE": VisionEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;

                        case "errorStatus":
                            {
                                Error = $"{tag.TimeStamp:dd/MM/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                                if (!(Errors.Contains(Error)))
                                {
                                    Errors.Add(Error);
                                    ErrorStrings = new(Errors);
                                }
                                break;
                            }
                        case "endErrorStatus":
                            {
                                var a = ErrorStrings.FirstOrDefault(i => i.Contains((string)tag.TagValue));
                                if (a == null) break;
                                Errors.Remove(a);
                                ErrorStrings = new(Errors);
                                break;
                            }
                        default: break;
                    }
                }
            }
        }
    }
}
