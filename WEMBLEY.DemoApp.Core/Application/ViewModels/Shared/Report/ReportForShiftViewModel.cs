using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report
{
    public class ReportForShiftViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IdTransferStore _idTransferStore;
        public string Id => _idTransferStore.Id;
        public string IsSeleted => _idTransferStore.IsSelected;
        public Visibility OEERowVis => _idTransferStore.OEERowVis;
        public Visibility ARowVis => _idTransferStore.ARowVis;
        public Visibility PRowVis => _idTransferStore.PRowVis;
        public Visibility QRowVis => _idTransferStore.QRowVis;
        public ObservableCollection<ShotDto> ShotEntries { get; set; } = new();
        public SeriesCollection SeriesCollection { set; get; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public Func<double, string> ValueFormatter { get; set; }
        public ICommand ReportForShiftViewCommand { get; set; }
        //
        
        private int interval = 5;
        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                LoadApiReport();
            }

        }
        public ObservableCollection<int> Intervals { get; private set; }
        public ReportForShiftViewModel(IApiService apiService, IdTransferStore idTransferStore)
        {
            _apiService = apiService;
            _idTransferStore = idTransferStore;
            Intervals = new ObservableCollection<int>() {1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200};
            ReportForShiftViewCommand = new RelayCommand(ReportForShiftView);

            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
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
            DateTimeFormatter = value => new DateTime((long)value).ToString("HH:mm:ss");
            ValueFormatter = value => value.ToString("0.00");
        }
        private void ReportForShiftView()
        {
            Interval = 5;
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(IsSeleted));
            OnPropertyChanged(nameof(OEERowVis));
            OnPropertyChanged(nameof(ARowVis));
            OnPropertyChanged(nameof(PRowVis));
            OnPropertyChanged(nameof(QRowVis));
            LoadApiReport();
            //OnPropertyChanged(nameof(ShotEntries));

            //switch (IsSeleted)
            //{
            //    case "OEE": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.OEE))); break;
            //    case "A": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.A))); break;
            //    case "P": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.P))); break;
            //    case "Q": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.Q))); break;
            //    default: break;
            //}
        }

        private async void LoadApiReport()
        {
            if (!(String.IsNullOrEmpty(Id)))
            {
                try
                {
                    var dtos = await _apiService.GetShortenShiftReportWithShotByShiftIdAsync(Id, Interval);
                    foreach (var d in dtos)
                    {
                        d.UpdateOEE(d.OEE, d.A, d.P, d.Q);
                    }
                    var shots = dtos.Last().Shots;
                    foreach (var sh in shots)
                    {
                        sh.UpdateOEE(sh.OEE, sh.A, sh.P, sh.Q);
                    }
                    ShotEntries = new(shots);
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
                OnPropertyChanged(nameof(ShotEntries));

                switch (IsSeleted)
                {
                    case "OEE": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.OEE))); break;
                    case "A": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.A))); break;
                    case "P": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.P))); break;
                    case "Q": SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShotEntries.Select(g => new ObservablePoint(g.TimeStamp.Ticks, g.Q))); break;
                    default: break;
                }
            }
        }
    }
}
