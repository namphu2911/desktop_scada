using CommunityToolkit.Mvvm.Input;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;
using System.Text.RegularExpressions;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine
{
    public class DosingDryingMonitorViewModel : BaseViewModel
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
        public double Efficiency { get; set; } = 0;
        public long AllProductCount { get; set; } = 0;
        public long GoodCount { get; set; } = 0;
        public long BadCount { get; set; } = 0;
        public long PlasticTrayQuantity { get; set; } = 0;
        public double HeatingTemperature { get; set; } = 0;
        public TimeSpan? OperationTime { get; set; }
        public string BloodTubeProductName { get; set; } = "";
        public string BloodTubeReferenceName { get; set; } = "";
        public string BloodTubeLotId { get; set; } = "";
        public int BloodTubeLotSize { get; set; } = 0;
        public long MinDateValue { get; set; } = DateTime.MinValue.Ticks;
        public long MaxDateValue { get; set; } = DateTime.MaxValue.Ticks;
        //
        public long FSNozzle1 { get; set; } = 0;
        public long FSNozzle2 { get; set; } = 0;
        //
        public long VisionTotalTube { get; set; } = 0;
        public long VisionGoodTube { get; set; } = 0;
        public long VisionBadTube { get; set; } = 0;
        public long StyrofoamTrayQuantity { get; set; } = 0;
        //
        public string ColorDrying1 { get; set; } = "#BBBBBB";
        public string ColorDrying2 { get; set; } = "#BBBBBB";
        public string ColorRobotArm { get; set; } = "#BBBBBB";
        public string ColorCapRubber { get; set; } = "#BBBBBB";
        public string ColorCapNonRubber { get; set; } = "#BBBBBB";

        private EStationEnable drying1Enable;
        public EStationEnable Drying1Enable
        {
            get { return drying1Enable; }
            set
            {
                drying1Enable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorDrying1 = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorDrying1 = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorDrying1 = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable drying2Enable;
        public EStationEnable Drying2Enable
        {
            get { return drying2Enable; }
            set
            {
                drying2Enable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorDrying2 = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorDrying2 = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorDrying2 = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable robotArmEnable;
        public EStationEnable RobotArmEnable
        {
            get { return robotArmEnable; }
            set
            {
                robotArmEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorRobotArm = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorRobotArm = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorRobotArm = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable capRubberEnable;
        public EStationEnable CapRubberEnable
        {
            get { return capRubberEnable; }
            set
            {
                capRubberEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorRobotArm = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorRobotArm = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorRobotArm = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        private EStationEnable capNonRubberEnable;
        public EStationEnable CapNonRubberEnable
        {
            get { return capNonRubberEnable; }
            set
            {
                capNonRubberEnable = value;
                switch (value)
                {
                    case EStationEnable.NonUse:
                        {
                            ColorRobotArm = "#ED5152";
                            break;
                        }
                    case EStationEnable.Use:
                        {
                            ColorRobotArm = "#3EB17F";
                            break;
                        }
                    default:
                        {
                            ColorRobotArm = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        //OEE
        public double? OEE { get; set; }
        public double? A { get; set; }
        public double? P { get; set; }
        public double? Q { get; set; }

        //

        //Chuyen trang
        public Visibility MonitorVis { get; set; } = Visibility.Visible;
        public Visibility DetectionVis { get; set; } = Visibility.Collapsed;
        public Visibility DetectionCurrentVis { get; set; } = Visibility.Collapsed;
        public Visibility DetectionHistoryVis { get; set; } = Visibility.Collapsed;

        //
        public string? Error { get; set; }
        List<string> Errors { get; set; } = new();


        public ObservableCollection<string> ErrorStrings { get; set; } = new();
        public ObservableCollection<string> PersonStrings { get; set; } = new();
        public ObservableCollection<OEEEntryViewModel> OEEEntries { get; set; } = new();
        public ObservableCollection<DetectionEntryViewModel> DetectionEntries { get; set; } = new();
        public ObservableCollection<DetectionEntryViewModel> DetectionHistoryEntries { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();
        //
        public List<DataPoint> OEEGraphTags { get; set; } = new();
        public SeriesCollection SeriesCollection { get; set; } = new();
        public Func<double, string> DateTimeFormatter { get; set; }
        public Func<double, string> ValueFormatter { get; set; }
        //

        public string currentPattern = @"S1_FS_CURRENT_(\d+)_(\d+)";
        public string historyPattern = @"S1_FS_HISTORY_(\d+)_(\d+)_(\d+)";

        public long[,] DetectionCurrent { get; set; } = new long[10, 10];
        public long[,] DetectionHistory1 { get; set; } = new long[10, 10];
        public long[,] DetectionHistory2 { get; set; } = new long[10, 10];
        public long[,] DetectionHistory3 { get; set; } = new long[10, 10];
        public long[,] DetectionHistory4 { get; set; } = new long[10, 10];
        public long[,] DetectionHistory5 { get; set; } = new long[10, 10];

        //
        public ICommand LoadDosingDryingMonitorViewCommand { get; set; }
        public ICommand LoadApiOEECommand { get; set; }
        public ICommand LoadMainMonitorCommand { get; set; }
        public ICommand LoadDetectionCommand { get; set; }
        public ICommand LoadDetectionCurrentCommand { get; set; }
        public ICommand LoadDetectionHistoryCommand { get; set; }
        //
        public ICommand LoadHistory1Command { get; set; }
        public ICommand LoadHistory2Command { get; set; }
        public ICommand LoadHistory3Command { get; set; }
        public ICommand LoadHistory4Command { get; set; }
        public ICommand LoadHistory5Command { get; set; }



        public event Action? ChartUpdated;
        private int interval;
        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
            }

        }
        public ObservableCollection<int> Intervals { get; private set; }
        public DosingDryingMonitorViewModel(IApiService apiService, ISignalRClient signalRClient)
        {
            _apiService = apiService;
            _signalRClient = signalRClient;
            Intervals = new ObservableCollection<int>() { 1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200 };
            LoadDosingDryingMonitorViewCommand = new RelayCommand(LoadDosingDryingMonitorView);
            LoadApiOEECommand = new RelayCommand(LoadApiOEE);

            LoadMainMonitorCommand = new RelayCommand(LoadMainMonitor);
            LoadDetectionCommand = new RelayCommand(LoadLoadDetection);
            LoadDetectionCurrentCommand = new RelayCommand(LoadDetectionCurrent);
            LoadDetectionHistoryCommand = new RelayCommand(LoadDetectionHistory);

            LoadHistory1Command = new RelayCommand(LoadHistory1);
            LoadHistory2Command = new RelayCommand(LoadHistory2);
            LoadHistory3Command = new RelayCommand(LoadHistory3);
            LoadHistory4Command = new RelayCommand(LoadHistory4);
            LoadHistory5Command = new RelayCommand(LoadHistory5);


            signalRClient.OnTagChanged += OnTagChanged;

            SeriesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Title = "Values",
                    Fill = Brushes.Transparent,
                    LineSmoothness = 2,
                    PointGeometry = DefaultGeometries.Circle,
                    PointForeground = Brushes.SkyBlue,
                    PointGeometrySize = 7,
                    Values = new ChartValues<ObservablePoint>()
                }
            };
            DateTimeFormatter = value => new DateTime((long)value).ToString("hh:mm:ss");
            ValueFormatter = value => value.ToString("0.00");
        }

        

        private void LoadMainMonitor()
        {
            MonitorVis = Visibility.Visible;
            DetectionVis = Visibility.Collapsed;
            DetectionCurrentVis = Visibility.Collapsed;
            DetectionHistoryVis = Visibility.Collapsed;
        }

        private void LoadLoadDetection()
        {
            MonitorVis = Visibility.Collapsed;
            DetectionVis = Visibility.Visible;
            DetectionCurrentVis = Visibility.Visible;
            DetectionHistoryVis = Visibility.Collapsed;
            DetectionChanged();
        }

        private void LoadDetectionCurrent()
        {
            MonitorVis = Visibility.Collapsed;
            DetectionVis = Visibility.Visible;
            DetectionCurrentVis = Visibility.Visible;
            DetectionHistoryVis = Visibility.Collapsed;
            DetectionChanged();
        }

        private void LoadDetectionHistory()
        {
            MonitorVis = Visibility.Collapsed;
            DetectionVis = Visibility.Visible;
            DetectionCurrentVis = Visibility.Collapsed;
            DetectionHistoryVis = Visibility.Visible;
        }

        private async void LoadDosingDryingMonitorView()
        {
            LoadLotSettingAsync();
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                OEE = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06", "OEE")) * 100;
                A = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06", "A")) * 100;
                P = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06", "P")) * 100;
                Q = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06", "Q")) * 100;

                Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "machineStatus"));
                OperationTime = TimeSpan.TryParse(Convert.ToString((await _signalRClient.GetBufferValue("IE-F3-BLO06", "operationTimeRaw"))), out var span) ? span : default;
                GoodCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "goodProductRaw"));
                BadCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "errorProductRaw"));
                Efficiency = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06", "EFF"));
                AllProductCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "productCountRaw"));
                HeatingTemperature = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_HEATING_TEMP"));
                PlasticTrayQuantity = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PLASTIC_TRAYS_QTY"));

                VisionGoodTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_VISION_GOOD_TUBES"));
                VisionBadTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_VISION_BAD_TUBES"));
                VisionTotalTube = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_VISION_TOTAL_TUBES"));
                StyrofoamTrayQuantity = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_STYROFOAM_TRAYS_QTY"));    

                Drying1Enable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_DRYING_1_ENABLE"));
                Drying2Enable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_DRYING_2_ENABLE"));
                RobotArmEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_ROBOT_ARM_ENABLE"));
                CapRubberEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_CAP_RUBBER_ENABLE"));  
                CapNonRubberEnable = (EStationEnable)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_CAP_NON_RUBBER_ENABLE"));

                FSNozzle1 = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_FS_NOZZLE_1"));
                FSNozzle2 = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_FS_NOZZLE_2"));

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        DetectionCurrent[i, j] = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", $"S1_FS_CURRENT_{i}_{j}"));
                        DetectionHistory1[i, j] = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", $"S1_FS_HISTORY_1_{i}_{j}"));
                        DetectionHistory2[i, j] = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", $"S1_FS_HISTORY_2_{i}_{j}"));
                        DetectionHistory3[i, j] = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", $"S1_FS_HISTORY_3_{i}_{j}"));
                        DetectionHistory4[i, j] = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", $"S1_FS_HISTORY_4_{i}_{j}"));
                        DetectionHistory5[i, j] = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06", $"S1_FS_HISTORY_5_{i}_{j}"));
                    }
                }

                if (AllProductCount > 0 && AllProductCount < 300)
                {
                    Interval = 1;
                }
                if (AllProductCount >= 300 && AllProductCount < 1100)
                {
                    Interval = 5;
                }
                if (AllProductCount >= 1100 && AllProductCount < 2500)
                {
                    Interval = 10;
                }
                if (AllProductCount >= 2500 && AllProductCount < 3700)
                {
                    Interval = 20;
                }
                if (AllProductCount >= 3700 && AllProductCount < 4900)
                {
                    Interval = 30;
                }
                if (AllProductCount >= 4900 && AllProductCount < 6100)
                {
                    Interval = 40;
                }
                if (AllProductCount >= 6100 && AllProductCount < 7300)
                {
                    Interval = 50;
                }
                if (AllProductCount >= 7300 && AllProductCount < 8500)
                {
                    Interval = 60;
                }
                if (AllProductCount >= 8500 && AllProductCount < 10000)
                {
                    Interval = 70;
                }
                if (AllProductCount >= 10000 && AllProductCount < 11500)
                {
                    Interval = 80;
                }
                if (AllProductCount >= 11500 && AllProductCount < 13000)
                {
                    Interval = 90;
                }
                if (AllProductCount >= 13000 && AllProductCount < 14500)
                {
                    Interval = 100;
                }
                if (AllProductCount >= 14500 && AllProductCount < 16000)
                {
                    Interval = 110;
                }
                if (AllProductCount >= 16000 && AllProductCount < 175000)
                {
                    Interval = 120;
                }
                if (AllProductCount >= 17500 && AllProductCount < 19000)
                {
                    Interval = 130;
                }
                if (AllProductCount >= 19000)
                {
                    Interval = 140;
                }

                LoadApiOEE();
                var errorTags = AllTags.Where(i => i.TagId == "errorStatus");
                foreach (var tag in errorTags)
                {
                    Error = $"{tag.TimeStamp:MM/dd/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                    if (!(Errors.Contains(Error)))
                    {
                        Errors.Add(Error);
                        ErrorStrings = new(Errors);
                    }
                }

                OEEChanged();
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                PersonStrings = new();
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceAsync("NonVacuumBloodTube");
                BloodTubeLotId = dtos.Last().LotCode;
                BloodTubeLotSize = dtos.Last().LotSize;
                if (string.IsNullOrEmpty(BloodTubeLotId))
                {
                    BloodTubeProductName = "";
                    BloodTubeReferenceName = "";
                }
                else
                {
                    BloodTubeProductName = dtos.Last().ProductName;
                    BloodTubeReferenceName = dtos.Last().ReferenceName;
                }
                if (dtos.First().Stations.Count() != 0)
                {
                    var persons = dtos.First().Stations.First(i => i.StationId == "IE-F3-BLO06").Employees;
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

        private async void LoadApiOEE()
        {
            try
            {
                var dtos = await _apiService.GetLastestOEEAsync("IE-F3-BLO06", Interval);
                OEEGraphTags = dtos.Select(i => new DataPoint(i.OEE * 100, i.TimeStamp)).ToList();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            SeriesCollection[0].Values = new ChartValues<ObservablePoint>(OEEGraphTags.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.OEE)));
        }

        public void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                if (tag.StationId == "IE-F3-BLO06")
                {
                    switch (tag.TagId)
                    {
                        case "OEE":
                            {
                                OEE = Convert.ToDouble(tag.TagValue) * 100;
                                OEEGraphTags.Add(new DataPoint(Convert.ToDouble(tag.TagValue) * 100, tag.TimeStamp));

                                SeriesCollection[0].Values.Add(new ObservablePoint
                                {
                                    X = tag.TimeStamp.Ticks,
                                    Y = Convert.ToDouble(tag.TagValue) * 100
                                });
                                if (SeriesCollection[0].Values is not null && SeriesCollection[0].Values.Count > 5)
                                {
                                    ChartUpdated?.Invoke();
                                }
                                OEEChanged(); break;
                            }
                        case "A": A = Convert.ToDouble(tag.TagValue) * 100; OEEChanged(); break;
                        case "P": P = Convert.ToDouble(tag.TagValue) * 100; OEEChanged(); break;
                        case "Q": Q = Convert.ToDouble(tag.TagValue) * 100; OEEChanged(); break;

                        case "machineStatus": Status = (EMachineStatus)Convert.ToInt32(tag.TagValue); break;
                        case "operationTimeRaw": OperationTime = TimeSpan.Parse((string)tag.TagValue); break;
                        case "goodProductRaw": GoodCount = Convert.ToInt64(tag.TagValue); break;
                        case "errorProductRaw": BadCount = Convert.ToInt64(tag.TagValue); break;
                        case "EFF": Efficiency = Convert.ToDouble(tag.TagValue); break;
                        case "productCountRaw": AllProductCount = Convert.ToInt64(tag.TagValue); break;
                        case "S1_HEATING_TEMP": HeatingTemperature = Convert.ToDouble(tag.TagValue); break;
                        case "S1_PLASTIC_TRAYS_QTY": PlasticTrayQuantity = Convert.ToInt64(tag.TagValue); break;

                        case "S1_VISION_GOOD_TUBES": VisionGoodTube = Convert.ToInt64(tag.TagValue); break;
                        case "S1_VISION_BAD_TUBES": VisionBadTube = Convert.ToInt64(tag.TagValue); break;
                        case "S1_VISION_TOTAL_TUBES": VisionTotalTube = Convert.ToInt64(tag.TagValue); break;
                        case "S1_STYROFOAM_TRAYS_QTY": StyrofoamTrayQuantity = Convert.ToInt64(tag.TagValue); break;

                        case "S1_DRYING_1_ENABLE": Drying1Enable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S1_DRYING_2_ENABLE": Drying2Enable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S1_ROBOT_ARM_ENABLE": RobotArmEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S1_CAP_RUBBER_ENABLE": CapRubberEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;
                        case "S1_CAP_NON_RUBBER_ENABLE": CapNonRubberEnable = (EStationEnable)Convert.ToInt32(tag.TagValue); break;

                        case "S1_FS_NOZZLE_1": FSNozzle1 = Convert.ToInt64(tag.TagValue); break;
                        case "S1_FS_NOZZLE_2": FSNozzle2 = Convert.ToInt64(tag.TagValue); break;

                        case "errorStatus":
                            {
                                Error = $"{tag.TimeStamp:MM/dd/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                                Errors.Add(Error);
                                ErrorStrings = new(Errors);
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

                        //
                        default:
                            {
                                if (tag.TagId.StartsWith("S1_FS_CURRENT_"))
                                {
                                    Match match = Regex.Match(tag.TagId, currentPattern);

                                    if (match.Success)
                                    {
                                        int row = int.Parse(match.Groups[1].Value);
                                        int col = int.Parse(match.Groups[2].Value);

                                        DetectionCurrent[row, col] = Convert.ToInt64(tag.TagValue);
                                        DetectionChanged();
                                    }
                                }
                                else if (tag.TagId.StartsWith("S1_FS_HISTORY_"))
                                {
                                    Match match = Regex.Match(tag.TagId, historyPattern);

                                    if (match.Success)
                                    {
                                        int historyIndex = int.Parse(match.Groups[1].Value);
                                        int row = int.Parse(match.Groups[2].Value);
                                        int col = int.Parse(match.Groups[3].Value);

                                        switch (historyIndex)
                                        {
                                            case 1:
                                                DetectionHistory1[row, col] = Convert.ToInt64(tag.TagValue);
                                                LoadHistory1();
                                                break;
                                            case 2:
                                                DetectionHistory2[row, col] = Convert.ToInt64(tag.TagValue);
                                                LoadHistory2();
                                                break;
                                            case 3:
                                                DetectionHistory3[row, col] = Convert.ToInt64(tag.TagValue);
                                                LoadHistory3();
                                                break;
                                            case 4:
                                                DetectionHistory4[row, col] = Convert.ToInt64(tag.TagValue);
                                                LoadHistory4();
                                                break;
                                            case 5:
                                                DetectionHistory5[row, col] = Convert.ToInt64(tag.TagValue);
                                                LoadHistory5();
                                                break;
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }

        private void OEEChanged()
        {
            OEEEntries = new ObservableCollection<OEEEntryViewModel>()
            {
                new OEEEntryViewModel(OEE, A, P, Q)
            };
            OnPropertyChanged(nameof(OEEEntries));
        }

        private void DetectionChanged()
        {
            DetectionEntries = new ObservableCollection<DetectionEntryViewModel>();
            for (int i = 0; i < 10; i++)
            {
                DetectionEntries.Add(new DetectionEntryViewModel
                    ($"Row {10 - i}",
                    DetectionCurrent[i, 0],
                    DetectionCurrent[i, 1],
                    DetectionCurrent[i, 2],
                    DetectionCurrent[i, 3],
                    DetectionCurrent[i, 4],
                    DetectionCurrent[i, 5],
                    DetectionCurrent[i, 6],
                    DetectionCurrent[i, 7],
                    DetectionCurrent[i, 8],
                    DetectionCurrent[i, 9]));
            }
            OnPropertyChanged(nameof(DetectionEntries));
        }

        private void LoadHistory1()
        {
            //DetectionHistoryEntries = new ObservableCollection<DetectionEntryViewModel>();
            //for (int i = 0; i < 10; i++)
            //{
            //    DetectionHistoryEntries.Add(new DetectionEntryViewModel
            //        ($"Row {10 - i}",
            //        DetectionHistory1[i, 0],
            //        DetectionHistory1[i, 1],
            //        DetectionHistory1[i, 2],
            //        DetectionHistory1[i, 3],
            //        DetectionHistory1[i, 4],
            //        DetectionHistory1[i, 5],
            //        DetectionHistory1[i, 6],
            //        DetectionHistory1[i, 7],
            //        DetectionHistory1[i, 8],
            //        DetectionHistory1[i, 9]));
            //}
            //OnPropertyChanged(nameof(DetectionHistoryEntries));
        }

        private void LoadHistory2()
        {
            DetectionHistoryEntries = new ObservableCollection<DetectionEntryViewModel>();
            for (int i = 0; i < 10; i++)
            {
                DetectionHistoryEntries.Add(new DetectionEntryViewModel
                    ($"Row {10 - i}",
                    DetectionHistory2[i, 0],
                    DetectionHistory2[i, 1],
                    DetectionHistory2[i, 2],
                    DetectionHistory2[i, 3],
                    DetectionHistory2[i, 4],
                    DetectionHistory2[i, 5],
                    DetectionHistory2[i, 6],
                    DetectionHistory2[i, 7],
                    DetectionHistory2[i, 8],
                    DetectionHistory2[i, 9]));
                OnPropertyChanged(nameof(DetectionHistoryEntries));
            }
        }

        private void LoadHistory3()
        {
            DetectionHistoryEntries = new ObservableCollection<DetectionEntryViewModel>();
            for (int i = 0; i < 10; i++)
            {
                DetectionHistoryEntries.Add(new DetectionEntryViewModel
                    ($"Row {10 - i}",
                    DetectionHistory3[i, 0],
                    DetectionHistory3[i, 1],
                    DetectionHistory3[i, 2],
                    DetectionHistory3[i, 3],
                    DetectionHistory3[i, 4],
                    DetectionHistory3[i, 5],
                    DetectionHistory3[i, 6],
                    DetectionHistory3[i, 7],
                    DetectionHistory3[i, 8],
                    DetectionHistory3[i, 9]));
                OnPropertyChanged(nameof(DetectionHistoryEntries));
            }
        }

        private void LoadHistory4()
        {
            DetectionHistoryEntries = new ObservableCollection<DetectionEntryViewModel>();
            for (int i = 0; i < 10; i++)
            {
                DetectionHistoryEntries.Add(new DetectionEntryViewModel
                    ($"Row {10 - i}",
                    DetectionHistory4[i, 0],
                    DetectionHistory4[i, 1],
                    DetectionHistory4[i, 2],
                    DetectionHistory4[i, 3],
                    DetectionHistory4[i, 4],
                    DetectionHistory4[i, 5],
                    DetectionHistory4[i, 6],
                    DetectionHistory4[i, 7],
                    DetectionHistory4[i, 8],
                    DetectionHistory4[i, 9]));
                OnPropertyChanged(nameof(DetectionHistoryEntries));
            }
        }

        private void LoadHistory5()
        {
            DetectionHistoryEntries = new ObservableCollection<DetectionEntryViewModel>();
            for (int i = 0; i < 10; i++)
            {
                DetectionHistoryEntries.Add(new DetectionEntryViewModel
                    ($"Row {10 - i}",
                    DetectionHistory5[i, 0],
                    DetectionHistory5[i, 1],
                    DetectionHistory5[i, 2],
                    DetectionHistory5[i, 3],
                    DetectionHistory5[i, 4],
                    DetectionHistory5[i, 5],
                    DetectionHistory5[i, 6],
                    DetectionHistory5[i, 7],
                    DetectionHistory5[i, 8],
                    DetectionHistory5[i, 9]));
                OnPropertyChanged(nameof(DetectionHistoryEntries));
            }
        }

    }
}
