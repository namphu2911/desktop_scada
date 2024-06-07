using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine
{
    public class StopperMachineMonitorViewModel : BaseViewModel
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
        public TimeSpan? OperationTime { get; set; }
        public string HerapinCapProductName { get; set; } = "";
        public string HerapinCapReferenceName { get; set; } = "";
        public string HerapinCapLotId { get; set; } = "";
        public int HerapinCapLotSize { get; set; } = 0;
        public long MinDateValue { get; set; } = DateTime.MinValue.Ticks;
        public long MaxDateValue { get; set; } = DateTime.MaxValue.Ticks;

        //OEE
        public double? OEE { get; set; }
        public double? A { get; set; }
        public double? P { get; set; }
        public double? Q { get; set; }

        //Reject
        public HerapinCapRejection TR1 { get; set; } = new(0, 0, 0, 0, 0);
        public HerapinCapRejection TR2 { get; set; } = new(0, 0, 0, 0, 0);
        public HerapinCapRejection TR3 { get; set; } = new(0, 0, 0, 0, 0);
        public HerapinCapRejection TR4 { get; set; } = new(0, 0, 0, 0, 0);

        //sieu am
        public Visibility RejectionVis { get; set; } = Visibility.Visible;
        public Visibility UltrasonicWelding13Vis { get; set; } = Visibility.Collapsed;
        public Visibility UltrasonicWelding24Vis { get; set; } = Visibility.Collapsed;
        public UltrasonicWeldingViewModel TR13 { get; set; } = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public UltrasonicWeldingViewModel TR24 { get; set; } = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        //
        public string? Error { get; set; }
        List<string> Errors { get; set; } = new();

        public ObservableCollection<RejectionEntryViewModel> RejectionEntries { get; set; } = new();
        public ObservableCollection<string> ErrorStrings { get; set; } = new();
        public ObservableCollection<string> PersonStrings { get; set; } = new();
        public ObservableCollection<OEEEntryViewModel> OEEEntries { get; set; } = new();
        public ObservableCollection<UltrasonicWeldingViewModel> UltrasonicWeldingTR13Entries { get; set; } = new();
        public ObservableCollection<UltrasonicWeldingViewModel> UltrasonicWeldingTR24Entries { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();
        //
        public List<DataPoint> OEEGraphTags { get; set; } = new();
        public SeriesCollection SeriesCollection { get; set; } = new();
        public Func<double, string> DateTimeFormatter { get; set; }
        public Func<double, string> ValueFormatter { get; set; }
        public ICommand LoadStopperMachineMonitorViewCommand { get; set; }
        public ICommand LoadApiOEECommand { get; set; }
        public ICommand ShowRejectionCommand { get; set; }
        public ICommand ShowUltrasonicWelding13Command { get; set; }
        public ICommand ShowUltrasonicWelding24Command { get; set; }
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
        public StopperMachineMonitorViewModel(IApiService apiService, ISignalRClient signalRClient)
        {
            _apiService = apiService;
            _signalRClient = signalRClient;
            Intervals = new ObservableCollection<int>() { 1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200 };
            LoadStopperMachineMonitorViewCommand = new RelayCommand(LoadStopperMachineMonitorView);
            LoadApiOEECommand = new RelayCommand(LoadApiOEE);
            ShowRejectionCommand = new RelayCommand(ShowRejection);
            ShowUltrasonicWelding13Command = new RelayCommand(ShowUltrasonicWelding13);
            ShowUltrasonicWelding24Command = new RelayCommand(ShowUltrasonicWelding24);

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

        private void ShowRejection()
        {
            RejectionVis = Visibility.Visible;
            UltrasonicWelding13Vis = Visibility.Collapsed;
            UltrasonicWelding24Vis = Visibility.Collapsed;
        }

        private void ShowUltrasonicWelding13()
        {
            RejectionVis = Visibility.Collapsed;
            UltrasonicWelding13Vis = Visibility.Visible;
            UltrasonicWelding24Vis = Visibility.Collapsed;
        }

        private void ShowUltrasonicWelding24()
        {
            RejectionVis = Visibility.Collapsed;
            UltrasonicWelding13Vis = Visibility.Collapsed;
            UltrasonicWelding24Vis = Visibility.Visible;
        }

        private async void LoadStopperMachineMonitorView()
        {
            LoadLotSettingAsync();
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                OEE = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "OEE")) * 100;
                A = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "A")) * 100;
                P = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "P")) * 100;
                Q = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "Q")) * 100;

                Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("IE-F2-HCA01", "machineStatus"));
                OperationTime = TimeSpan.TryParse(Convert.ToString((await _signalRClient.GetBufferValue("IE-F2-HCA01", "operationTimeRaw"))), out var span) ? span : default;
                GoodCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01", "goodProductRaw"));
                BadCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01", "errorProduct"));
                Efficiency = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "EFF"));
                AllProductCount = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01", "productCount"));

                TR13.Cycle = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01", "Weld_Cycle_Tr1&3_S7"));

                TR13.RunTime = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "RunTime_Tr1&3_S7"));
                TR13.PkPwr = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "Pk_Pwr_Tr1&3_S7")); 
                TR13.Energy = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01", "Energy_Tr1&3_S7"));
                TR13.WeldAbs = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Weld_Abs_Tr1&3_S7"));
                TR13.WeldCol = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Weld_Col_Tr1&3_S7"));
                TR13.TotalCol = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Total_Col_Tr1&3_S7"));
                
                TR13.TrigForce = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Trig_Force_Tr1&3_S7"));
                TR13.WeldForce = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Weld_Force_Tr1&3_S7"));
                TR13.FreqChg = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Freq_Chg_Tr1&3_S7"));
                TR13.SetAMPA = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Set_AMP_A_Tr1&3_S7"));
                TR13.Velocity = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Velocity_Tr1&3_S7"));
                
                //
                TR24.Cycle = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Weld_Cycle_Tr2&4_S6"));
                
                TR24.RunTime = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","RunTime_Tr2&4_S6"));
                TR24.PkPwr = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Pk_Pwr_Tr2&4_S6"));
                TR24.Energy = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Energy_Tr2&4_S6"));
                TR24.WeldAbs = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Weld_Abs_Tr2&4_S6"));
                TR24.WeldCol = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Weld_Col_Tr2&4_S6"));
                TR24.TotalCol = Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F2-HCA01","Total_Col_Tr2&4_S6"));
                
                TR24.TrigForce = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Trig_Force_Tr2&4_S6"));
                TR24.WeldForce = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Weld_Force_Tr2&4_S6"));
                TR24.FreqChg = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Freq_Chg_Tr2&4_S6"));
                TR24.SetAMPA = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Set_AMP_A_Tr2&4_S6"));
                TR24.Velocity = Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","Velocity_Tr2&4_S6"));

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
                    if ((string)tag.TagValue != "Wifi disconnected")
                    {
                        Error = $"{tag.TimeStamp:dd/MM/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                        if (!(Errors.Contains(Error)))
                        {
                            Errors.Add(Error);
                            ErrorStrings = new(Errors);
                        }
                    }
                }

                TR1 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","BOTTOM_CAP_REJ_TR1")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","SILICON_PRESENCE_REJ_TR1")),
                         Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","COVER_PRESENCE_REJ_TR1")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","HEIGHT_CHK_REJ_TR1")),
                         Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","LEAK_TEST_CHK_OK_TR1")));
                TR2 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","BOTTOM_CAP_REJ_TR2")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","SILICON_PRESENCE_REJ_TR2")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","COVER_PRESENCE_REJ_TR2")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","HEIGHT_CHK_REJ_TR2")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","LEAK_TEST_CHK_TR2")));
                TR3 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","BOTTOM_CAP_REJ_TR3")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","SILICON_PRESENCE_REJ_TR3")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","COVER_PRESENCE_REJ_TR3")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","HEIGHT_CHK_REJ_TR3")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","LEAK_TEST_CHK_TR3")));
                TR4 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","BOTTOM_CAL_REJ_TR4")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","SILICON_PRESENCE_REJ_TR4")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","COVER_PRESENCE_REJ_TR4")), Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","HEIGHT_CHK_REJ_TR4")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F2-HCA01","LEAK_TEST_CHK_TR4")));

                OEEChanged();
                RejectionChanged();
                UltrasonicWeldingTR13Changed();
                UltrasonicWeldingTR24Changed();
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                PersonStrings = new();
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceAsync("HerapinCap");
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
                if (dtos.First().Stations.Count() != 0)
                {
                    var persons = dtos.First().Stations.First(i => i.StationId == "IE-F2-HCA01").Employees;
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
                var dtos = await _apiService.GetLastestOEEAsync("IE-F2-HCA01", Interval);
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
                if (tag.StationId == "IE-F2-HCA01")
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
                        case "errorProduct": BadCount = Convert.ToInt64(tag.TagValue); break;
                        case "EFF": Efficiency = Convert.ToDouble(tag.TagValue); break;
                        case "productCount": AllProductCount = Convert.ToInt64(tag.TagValue); break;
                        case "errorStatus":
                            {
                                if ((string)tag.TagValue == "Wifi disconnected")
                                {
                                    ShowErrorMessage("Rasberry Wifi disconnected");
                                }
                                else
                                {
                                    Error = $"{tag.TimeStamp:dd/MM/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                                    if (!(Errors.Contains(Error)))
                                    {
                                        Errors.Add(Error);
                                        ErrorStrings = new(Errors);
                                    }
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
                        case "BOTTOM_CAP_REJ_TR1": TR1.BOTTOMCAP = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "BOTTOM_CAP_REJ_TR2": TR2.BOTTOMCAP = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "BOTTOM_CAP_REJ_TR3": TR3.BOTTOMCAP = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "BOTTOM_CAL_REJ_TR4": TR4.BOTTOMCAP = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;

                        case "SILICON_PRESENCE_REJ_TR1": TR1.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "SILICON_PRESENCE_REJ_TR2": TR2.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "SILICON_PRESENCE_REJ_TR3": TR3.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "SILICON_PRESENCE_REJ_TR4": TR4.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;

                        case "COVER_PRESENCE_REJ_TR1": TR1.COVERPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "COVER_PRESENCE_REJ_TR2": TR2.COVERPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "COVER_PRESENCE_REJ_TR3": TR3.COVERPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "COVER_PRESENCE_REJ_TR4": TR4.COVERPRESENCE = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;

                        case "HEIGHT_CHK_REJ_TR1": TR1.HEIGHTCHK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "HEIGHT_CHK_REJ_TR2": TR2.HEIGHTCHK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "HEIGHT_CHK_REJ_TR3": TR3.HEIGHTCHK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "HEIGHT_CHK_REJ_TR4": TR4.HEIGHTCHK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;

                        case "LEAK_TEST_CHK_OK_TR1": TR1.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "LEAK_TEST_CHK_TR2": TR2.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "LEAK_TEST_CHK_TR3": TR3.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;
                        case "LEAK_TEST_CHK_TR4": TR4.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); RejectionChanged(); break;

                        //
                        case "Weld_Cycle_Tr1&3_S7":
                            TR13.Cycle = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "RunTime_Tr1&3_S7":
                            TR13.RunTime = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Pk_Pwr_Tr1&3_S7":
                            TR13.PkPwr = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Energy_Tr1&3_S7":
                            TR13.Energy = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Weld_Abs_Tr1&3_S7":
                            TR13.WeldAbs = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Weld_Col_Tr1&3_S7":
                            TR13.WeldCol = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Total_Col_Tr1&3_S7":
                            TR13.TotalCol = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;

                        case "Trig_Force_Tr1&3_S7":
                            TR13.TrigForce = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Weld_Force_Tr1&3_S7":
                            TR13.WeldForce = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Freq_Chg_Tr1&3_S7":
                            TR13.FreqChg = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Set_AMP_A_Tr1&3_S7":
                            TR13.SetAMPA = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;
                        case "Velocity_Tr1&3_S7":
                            TR13.Velocity = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR13Changed(); break;

                        //
                        case "Weld_Cycle_Tr2&4_S6":
                            TR24.Cycle = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;

                        case "RunTime_Tr2&4_S6":
                            TR24.RunTime = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Pk_Pwr_Tr2&4_S6":
                            TR24.PkPwr = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Energy_Tr2&4_S6":
                            TR24.Energy = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Weld_Abs_Tr2&4_S6":
                            TR24.WeldAbs = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Weld_Col_Tr2&4_S6":
                            TR24.WeldCol = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Total_Col_Tr2&4_S6":
                            TR24.TotalCol = Convert.ToDouble(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;

                        case "Trig_Force_Tr2&4_S6":
                            TR24.TrigForce = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Weld_Force_Tr2&4_S6":
                            TR24.WeldForce = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Freq_Chg_Tr2&4_S6":
                            TR24.FreqChg = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Set_AMP_A_Tr2&4_S6":
                            TR24.SetAMPA = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;
                        case "Velocity_Tr2&4_S6":
                            TR24.Velocity = Convert.ToInt64(tag.TagValue);
                            UltrasonicWeldingTR24Changed(); break;

                        default: break;
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

        private void RejectionChanged()
        {
            RejectionEntries = new ObservableCollection<RejectionEntryViewModel>()
            {
                new RejectionEntryViewModel("Station-1 Bottom Cap Check", TR1.BOTTOMCAP, TR2.BOTTOMCAP, TR3.BOTTOMCAP, TR4.BOTTOMCAP),
                new RejectionEntryViewModel("Station-3 Silicon Check", TR1.SILICONPRESENCE, TR2.SILICONPRESENCE, TR3.SILICONPRESENCE, TR4.SILICONPRESENCE),
                new RejectionEntryViewModel("Station-5 Cover Check", TR1.COVERPRESENCE, TR2.COVERPRESENCE, TR3.COVERPRESENCE, TR4.COVERPRESENCE),
                new RejectionEntryViewModel("Station-8,9 Height Check", TR1.HEIGHTCHK, TR2.HEIGHTCHK, TR3.HEIGHTCHK, TR4.HEIGHTCHK),
                new RejectionEntryViewModel("Station-10 Leak Check", TR1.LEAKTESTCHKOK, TR2.LEAKTESTCHKOK, TR3.LEAKTESTCHKOK, TR4.LEAKTESTCHKOK)
            };
            OnPropertyChanged(nameof(RejectionEntries));
        }       
        
        private void UltrasonicWeldingTR13Changed()
        {
            UltrasonicWeldingTR13Entries = new ObservableCollection<UltrasonicWeldingViewModel>()
            {
                new UltrasonicWeldingViewModel(TR13.Cycle, TR13.RunTime, TR13.PkPwr, TR13.Energy, TR13.WeldAbs, TR13.WeldCol, TR13.TotalCol, TR13.TrigForce, TR13.WeldForce, TR13.FreqChg, TR13.SetAMPA, TR13.Velocity)
            };
            OnPropertyChanged(nameof(UltrasonicWeldingTR13Entries));
        }

        private void UltrasonicWeldingTR24Changed()
        {
            UltrasonicWeldingTR24Entries = new ObservableCollection<UltrasonicWeldingViewModel>()
            {
                new UltrasonicWeldingViewModel(TR24.Cycle, TR24.RunTime, TR24.PkPwr, TR24.Energy, TR24.WeldAbs, TR24.WeldCol, TR24.TotalCol, TR24.TrigForce, TR24.WeldForce, TR24.FreqChg, TR24.SetAMPA, TR24.Velocity)
            };
            OnPropertyChanged(nameof(UltrasonicWeldingTR24Entries));
        }
    }
}