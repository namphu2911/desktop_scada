using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineDetail
{
    public class StopperMachineMonitorViewModel : BaseViewModel
    {
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

        public string ColorBack { get; set; } = "";
        public double? Efficiency { get; set; }
        public long? AllProductCount { get; set; }
        public long? GoodCount { get; set; }
        public long? BadCount { get; set; }
        public TimeSpan OperationTime { get; set; }

        //OEE
        public double? OEE { get; set; }
        public double? A { get; set; }
        public double? P { get; set; }
        public double? Q { get; set; }

        //Reject
        public HerapinCapRejection TR1 { get; set; }
        public HerapinCapRejection TR2 { get; set; }
        public HerapinCapRejection TR3 { get; set; }
        public HerapinCapRejection TR4 { get; set; }


        public string? Error { get; set; }
        List<string> Errors { get; set; } = new();

        public ObservableCollection<RejectionEntryViewModel> RejectionEntries { get; set; } = new();
        public ObservableCollection<string> ErrorStrings { get; set; } = new();
        public ObservableCollection<OEEEntryViewModel> OEEEntries { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();
        //
        public List<DataPoint> OEEGraphTags { get; set; } = new();
        public SeriesCollection SeriesCollection { set; get; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public Func<double, string> ValueFormatter { get; set; }
        public ICommand LoadStopperMachineMonitorViewCommand { get; set; }
        public StopperMachineMonitorViewModel(ISignalRClient signalRClient)
        { 
            _signalRClient = signalRClient;
            LoadStopperMachineMonitorViewCommand = new RelayCommand(LoadStopperMachineMonitorView);
            signalRClient.OnTagChanged += OnTagChanged;
            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Values",
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
            AllTags = await _signalRClient.GetBufferList();
            OEE = (double)(await _signalRClient.GetBufferValue("OEE"));
            A = (double?)(await _signalRClient.GetBufferValue("A"));
            P = (double?)(await _signalRClient.GetBufferValue("P"));
            Q = Convert.ToDouble(await _signalRClient.GetBufferValue("Q"));

            Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("machineStatus"));
            OperationTime = TimeSpan.Parse((string)(await _signalRClient.GetBufferValue("operationTime")));
            GoodCount = (long?)(await _signalRClient.GetBufferValue("goodProduct"));
            BadCount = (long?)(await _signalRClient.GetBufferValue("errorProduct"));
            Efficiency = (double?)(await _signalRClient.GetBufferValue("EFF"));
            AllProductCount = (long?)(await _signalRClient.GetBufferValue("productCount"));

            TR1 = new((long?)(await _signalRClient.GetBufferValue("BOTTOM_CAP_REJ_TR1")), (long?)(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR1")),
                      (long?)(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR1")), (long?)(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR1")),
                      (long?)(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_OK_TR1")));
            TR2 = new((long?)(await _signalRClient.GetBufferValue("BOTTOM_CAP_REJ_TR2")), (long?)(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR2")),
                      (long?)(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR2")), (long?)(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR2")),
                      (long?)(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_TR2")));
            TR3 = new((long?)(await _signalRClient.GetBufferValue("BOTTOM_CAP_REJ_TR3")), (long?)(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR3")),
                      (long?)(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR3")), (long?)(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR3")),
                      (long?)(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_TR3")));
            TR4 = new((long?)(await _signalRClient.GetBufferValue("BOTTOM_CAL_REJ_TR4")), (long?)(await _signalRClient.GetBufferValue("SILICON_PRESENCE_REJ_TR4")),
                      (long?)(await _signalRClient.GetBufferValue("COVER_PRESENCE_REJ_TR4")), (long?)(await _signalRClient.GetBufferValue("HEIGHT_CHK_REJ_TR4")),
                      (long?)(await _signalRClient.GetBufferValue("LEAK_TEST_CHK_TR4")));

            OEEEntries = new ObservableCollection<OEEEntryViewModel>()
            {
                new OEEEntryViewModel(OEE, A, P, Q)
            };

            RejectionEntries = new ObservableCollection<RejectionEntryViewModel>()
            {
                new RejectionEntryViewModel("Station-1 Bottom Cap Check", TR1.BOTTOMCAP, TR2.BOTTOMCAP, TR3.BOTTOMCAP, TR4.BOTTOMCAP),
                new RejectionEntryViewModel("Station-3 Silicon Check", TR1.SILICONPRESENCE, TR2.SILICONPRESENCE, TR3.SILICONPRESENCE, TR4.SILICONPRESENCE),
                new RejectionEntryViewModel("Station-5 Cover Check", TR1.COVERPRESENCE, TR2.COVERPRESENCE, TR3.COVERPRESENCE, TR4.COVERPRESENCE),
                new RejectionEntryViewModel("Station-8,9 Height Check", TR1.HEIGHTCHK, TR2.HEIGHTCHK, TR3.HEIGHTCHK, TR4.HEIGHTCHK),
                new RejectionEntryViewModel("Station-10 Leak Check", TR1.LEAKTESTCHKOK, TR2.LEAKTESTCHKOK, TR3.LEAKTESTCHKOK, TR4.LEAKTESTCHKOK)

            };
        }
        private void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
           
            switch(tag.TagId)
            {
                case "OEE":
                    {
                        OEE = (double?)tag.TagValue; 
                        OEEGraphTags.Add(new DataPoint((double)tag.TagValue, tag.TimeStamp));
                        if(SeriesCollection[0].Values.Count > 20)
                        {
                            var i = SeriesCollection[0].Values.Count - 20;
                            SeriesCollection[0].Values.RemoveAt(i-1);
                        }
                        SeriesCollection[0].Values.Add(new ObservablePoint
                        {
                            X = OEEGraphTags.Last().TimeStamp.Ticks,
                            Y = (double)OEEGraphTags.Last().TagValue
                        });
                    break;
                    }
                case "A": A = (double?)tag.TagValue; break;
                case "P": P = (double?)tag.TagValue; break;
                case "Q": Q = (double?)tag.TagValue; break;

                case "machineStatus": Status = (EMachineStatus)Convert.ToInt32(tag.TagValue); break;
                case "operationTime": OperationTime = TimeSpan.Parse((string)tag.TagValue); break;
                case "goodProduct": GoodCount = (long?)tag.TagValue; break;
                case "errorProduct": BadCount = (long?)tag.TagValue; break;
                case "EFF": Efficiency = (double?)tag.TagValue; break;
                case "productCount": AllProductCount = (long?)tag.TagValue; break;
                case "errorStatus":
                    {
                        Error = $"{tag.TimeStamp.ToString("dd-MM-yyyy")}: {(string)tag.TagValue}";
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
                case "BOTTOM_CAP_REJ_TR1": TR1.BOTTOMCAP = (long?)tag.TagValue; break;
                case "BOTTOM_CAP_REJ_TR2": TR2.BOTTOMCAP = (long?)tag.TagValue; break;
                case "BOTTOM_CAP_REJ_TR3": TR3.BOTTOMCAP = (long?)tag.TagValue; break;
                case "BOTTOM_CAP_REJ_TR4": TR4.BOTTOMCAP = (long?)tag.TagValue; break;

                case "SILICON_PRESENCE_REJ_TR1": TR1.SILICONPRESENCE = (long?)tag.TagValue; break;
                case "SILICON_PRESENCE_REJ_TR2": TR2.SILICONPRESENCE = (long?)tag.TagValue; break;
                case "SILICON_PRESENCE_REJ_TR3": TR3.SILICONPRESENCE = (long?)tag.TagValue; break;
                case "SILICON_PRESENCE_REJ_TR4": TR4.SILICONPRESENCE = (long?)tag.TagValue; break;

                case "COVER_PRESENCE_REJ_TR1": TR1.COVERPRESENCE = (long?)tag.TagValue; break;
                case "COVER_PRESENCE_REJ_TR2": TR2.COVERPRESENCE = (long?)tag.TagValue; break;
                case "COVER_PRESENCE_REJ_TR3": TR3.COVERPRESENCE = (long?)tag.TagValue; break;
                case "COVER_PRESENCE_REJ_TR4": TR4.COVERPRESENCE = (long?)tag.TagValue; break;

                case "HEIGHT_CHK_REJ_TR1": TR1.HEIGHTCHK = (long?)tag.TagValue; break;
                case "HEIGHT_CHK_REJ_TR2": TR2.HEIGHTCHK = (long?)tag.TagValue; break;
                case "HEIGHT_CHK_REJ_TR3": TR3.HEIGHTCHK = (long?)tag.TagValue; break;
                case "HEIGHT_CHK_REJ_TR4": TR4.HEIGHTCHK = (long?)tag.TagValue; break;

                case "LEAK_TEST_CHK_OK_TR1": TR1.LEAKTESTCHKOK = (long?)tag.TagValue; break;
                case "LEAK_TEST_CHK_TR2": TR2.LEAKTESTCHKOK = (long?)tag.TagValue; break;
                case "LEAK_TEST_CHK_TR3": TR3.LEAKTESTCHKOK = (long?)tag.TagValue; break;
                case "LEAK_TEST_CHK_TR4": TR4.LEAKTESTCHKOK = (long?)tag.TagValue; break;

                default: break;
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