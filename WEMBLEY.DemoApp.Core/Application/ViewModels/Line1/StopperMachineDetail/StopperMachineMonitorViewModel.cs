using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using System.Windows.Media;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineDetail
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
                switch(value)
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
        public double NaNValue { get; set; }

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


        public string? Error { get; set; }
        List<string> Errors { get; set; } = new();

        public ObservableCollection<RejectionEntryViewModel> RejectionEntries { get; set; } = new();
        public ObservableCollection<string> ErrorStrings { get; set; } = new();
        public ObservableCollection<string> PersonStrings { get; set; } = new();
        public ObservableCollection<OEEEntryViewModel> OEEEntries { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();
        //
        public List<DataPoint> OEEGraphTags { get; set; } = new();
        public SeriesCollection SeriesCollection { get; set; } = new();
        public ObservableCollection<ObservablePoint> OEEObservablePoints { get; set; } = new();
        public Func<double, string> DateTimeFormatter { get; set; }
        public Func<double, string> ValueFormatter { get; set; }
        public ICommand LoadStopperMachineMonitorViewCommand { get; set; }
        public ICommand LoadReloadGraphCommand { get; set; }
        public ICommand LoadApiOEECommand { get; set; }
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
            LoadReloadGraphCommand = new RelayCommand(ReloadGraph);
            LoadApiOEECommand = new RelayCommand(LoadApiOEE);
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

        private async void LoadStopperMachineMonitorView()
        {
            LoadLotSettingAsync();
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                OEE = Convert.ToDouble(await _signalRClient.GetBufferValue("OEE")) * 100;
                A = Convert.ToDouble(await _signalRClient.GetBufferValue("A")) * 100;
                P = Convert.ToDouble(await _signalRClient.GetBufferValue("P")) * 100;
                Q = Convert.ToDouble(await _signalRClient.GetBufferValue("Q")) * 100;

                Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("machineStatus"));
                OperationTime = TimeSpan.TryParse(Convert.ToString((await _signalRClient.GetBufferValue("operationTime"))), out var span) ? span : default;
                GoodCount = Convert.ToInt64(await _signalRClient.GetBufferValue("goodProduct"));
                BadCount = Convert.ToInt64(await _signalRClient.GetBufferValue("errorProduct"));
                Efficiency = Convert.ToDouble(await _signalRClient.GetBufferValue("EFF"));
                AllProductCount = Convert.ToInt64(await _signalRClient.GetBufferValue("productCount"));

                if(AllProductCount > 0 && AllProductCount < 300)
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
                if (AllProductCount >= 2500 && AllProductCount <3700)
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
                foreach(var tag in errorTags)
                {
                    Error = $"{tag.TimeStamp:MM/dd/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                    if (!(Errors.Contains(Error)))
                    {
                        Errors.Add(Error);
                        ErrorStrings = new(Errors);
                    }
                }

                TR1 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("BOTTOM_CAP_REJ_TR1")), Convert.ToInt64(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR1")),
                         Convert.ToInt64(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR1")), Convert.ToInt64(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR1")),
                         Convert.ToInt64(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_OK_TR1")));
                TR2 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("BOTTOM_CAP_REJ_TR2")), Convert.ToInt64(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR2")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR2")), Convert.ToInt64(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR2")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_TR2")));
                TR3 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("BOTTOM_CAP_REJ_TR3")), Convert.ToInt64(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR3")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR3")), Convert.ToInt64(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR3")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_TR3")));
                TR4 = new(Convert.ToInt64(await _signalRClient.GetBufferValue("BOTTOM_CAL_REJ_TR4")), Convert.ToInt64(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR4")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR4")), Convert.ToInt64(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR4")),
                          Convert.ToInt64(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_TR4")));

                OEEEntries = new ObservableCollection<OEEEntryViewModel>()
                {
                    new OEEEntryViewModel(OEE, A, P, Q)
                };
                OnPropertyChanged(nameof(OEEEntries));
                //OnPropertyChanged(nameof(SeriesCollection));
                //OnPropertyChanged(nameof(OEEGraphTags));

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
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                PersonStrings = new();
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceTypeAsync("HerapinCap");
                HerapinCapLotId = dtos.Last().LotId;
                HerapinCapLotSize = dtos.Last().LotSize;
                if(string.IsNullOrEmpty(HerapinCapLotId))
                {
                    HerapinCapProductName = "";
                    HerapinCapReferenceName = "";
                }
                else
                {
                    HerapinCapProductName = dtos.Last().ProductName;
                    HerapinCapReferenceName = dtos.Last().RefName;
                }
                var persons = dtos.First().Devices.First(i => i.DeviceId == "HC001").Persons;
                foreach(var person in persons)
                {
                    PersonStrings.Add($"{person.PersonId} - {person.PersonName}");
                }
                OnPropertyChanged(nameof(PersonStrings));
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
                var dtos = await _apiService.GetLastestOEEAsync("HC001", Interval);
                OEEGraphTags = dtos.Select(i => new DataPoint(i.OEE * 100, i.TimeStamp)).ToList();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            SeriesCollection[0].Values = new ChartValues<ObservablePoint>(OEEGraphTags.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.OEE)));
        }

        private void ReloadGraph()
        {
            OnPropertyChanged(nameof(SeriesCollection));
        }

        public void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                if (tag.DeviceId == "HC001")
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
                                break;
                            }
                        case "A": A = Convert.ToDouble(tag.TagValue) * 100; break;
                        case "P": P = Convert.ToDouble(tag.TagValue) * 100; break;
                        case "Q": Q = Convert.ToDouble(tag.TagValue) * 100; break;

                        case "machineStatus": Status = (EMachineStatus)Convert.ToInt32(tag.TagValue); break;
                        case "operationTimeRaw": OperationTime = TimeSpan.Parse((string)tag.TagValue); break;
                        case "goodProductRaw": GoodCount = Convert.ToInt64(tag.TagValue); break;
                        case "errorProduct": BadCount = Convert.ToInt64(tag.TagValue); break;
                        case "EFF": Efficiency = Convert.ToDouble(tag.TagValue); break;
                        case "productCount": AllProductCount = Convert.ToInt64(tag.TagValue); break;
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
                        case "BOTTOM_CAP_REJ_TR1": TR1.BOTTOMCAP = Convert.ToInt64(tag.TagValue); break;
                        case "BOTTOM_CAP_REJ_TR2": TR2.BOTTOMCAP = Convert.ToInt64(tag.TagValue); break;
                        case "BOTTOM_CAP_REJ_TR3": TR3.BOTTOMCAP = Convert.ToInt64(tag.TagValue); break;
                        case "BOTTOM_CAP_REJ_TR4": TR4.BOTTOMCAP = Convert.ToInt64(tag.TagValue); break;

                        case "SILICON_PRESENCE_REJ_TR1": TR1.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); break;
                        case "SILICON_PRESENCE_REJ_TR2": TR2.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); break;
                        case "SILICON_PRESENCE_REJ_TR3": TR3.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); break;
                        case "SILICON_PRESENCE_REJ_TR4": TR4.SILICONPRESENCE = Convert.ToInt64(tag.TagValue); break;

                        case "COVER_PRESENCE_REJ_TR1": TR1.COVERPRESENCE = Convert.ToInt64(tag.TagValue); break;
                        case "COVER_PRESENCE_REJ_TR2": TR2.COVERPRESENCE = Convert.ToInt64(tag.TagValue); break;
                        case "COVER_PRESENCE_REJ_TR3": TR3.COVERPRESENCE = Convert.ToInt64(tag.TagValue); break;
                        case "COVER_PRESENCE_REJ_TR4": TR4.COVERPRESENCE = Convert.ToInt64(tag.TagValue); break;

                        case "HEIGHT_CHK_REJ_TR1": TR1.HEIGHTCHK = Convert.ToInt64(tag.TagValue); break;
                        case "HEIGHT_CHK_REJ_TR2": TR2.HEIGHTCHK = Convert.ToInt64(tag.TagValue); break;
                        case "HEIGHT_CHK_REJ_TR3": TR3.HEIGHTCHK = Convert.ToInt64(tag.TagValue); break;
                        case "HEIGHT_CHK_REJ_TR4": TR4.HEIGHTCHK = Convert.ToInt64(tag.TagValue); break;

                        case "LEAK_TEST_CHK_OK_TR1": TR1.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); break;
                        case "LEAK_TEST_CHK_TR2": TR2.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); break;
                        case "LEAK_TEST_CHK_TR3": TR3.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); break;
                        case "LEAK_TEST_CHK_TR4": TR4.LEAKTESTCHKOK = Convert.ToInt64(tag.TagValue); break;

                        default: break;
                    }
                }
                
            }
            
            OEEEntries = new ObservableCollection<OEEEntryViewModel>()
            {
                new OEEEntryViewModel(OEE, A, P, Q)
            };
            OnPropertyChanged(nameof(OEEEntries));

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
    }
}