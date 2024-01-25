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
        public event Action? ChartUpdated; 
        public StopperMachineMonitorViewModel(IApiService apiService, ISignalRClient signalRClient)
        {
            _apiService = apiService;
            _signalRClient = signalRClient;
            LoadStopperMachineMonitorViewCommand = new RelayCommand(LoadStopperMachineMonitorView);
            LoadReloadGraphCommand = new RelayCommand(ReloadGraph);
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

                var errorTags = AllTags.Where(i => i.TagId == "errorStatus");
                foreach(var tag in errorTags)
                {
                    Error = $"{tag.TimeStamp:MM/dd/yyyy HH:mm:ss}: {(string)tag.TagValue}";
                    Errors.Add(Error);
                    ErrorStrings = new(Errors);
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
                HerapinCapProductName = dtos.Last().ProductName;
                HerapinCapReferenceName = dtos.Last().RefName;
                HerapinCapLotId = dtos.Last().LotId;
                HerapinCapLotSize = dtos.Last().LotSize;
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

                                //if (SeriesCollection[0].Values is not null && SeriesCollection[0].Values.Count > 20)
                                //{
                                //    var i = SeriesCollection[0].Values.Count - 20;
                                //    SeriesCollection[0].Values.RemoveAt(i - 1);
                                //}

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
                        case "operationTime": OperationTime = TimeSpan.Parse((string)tag.TagValue); break;
                        case "goodProduct": GoodCount = Convert.ToInt64(tag.TagValue); break;
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