using AutoMapper;
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
using WEMBLEY.DemoApp.Core.Domain.Dtos;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineReport
{
    public class ReportLongTimeViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7).Date;
        public DateTime EndDate { get; set; } = DateTime.Today.Date;
        //
        public string IsSeleted { get; set; } = "All";

        public Visibility FullOEEVis { get; set; } = Visibility.Visible;
        public Visibility ChartVis { get; set; } = Visibility.Collapsed;

        public Visibility OEERowVis { get; set; } = Visibility.Collapsed;
        public Visibility ARowVis { get; set; } = Visibility.Collapsed;
        public Visibility PRowVis { get; set; } = Visibility.Collapsed;
        public Visibility QRowVis { get; set; } = Visibility.Collapsed;
        public ObservableCollection<ShiftReportDto> ShiftReportEntries { get; set; } = new();
        public event Action? Changed;

        private readonly IdTransferStore _idTransferStore;
        private ShiftReportDto? selectedEntry;
        public ShiftReportDto? SelectedEntry
        {
            get => selectedEntry;
            set
            {
                selectedEntry = value;
                if (selectedEntry is not null)
                {
                    Changed?.Invoke();
                    _idTransferStore.SetIdTransfer(selectedEntry.Id, IsSeleted, OEERowVis, ARowVis, PRowVis, QRowVis);
                }
            }
        }
        public SeriesCollection SeriesCollection { set; get; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public Func<double, string> ValueFormatter { get; set; }

        public ICommand MainButtonCommand { get; set; }
        public ICommand LoadReportLongTimeCommand { get; set; }
        public ICommand LoadOEECommand { get; set; }
        public ICommand LoadACommand { get; set; }
        public ICommand LoadPCommand { get; set; }
        public ICommand LoadQCommand { get; set; }
        public ReportLongTimeViewModel(IApiService apiService, IdTransferStore idTransferStore)
        {
            _apiService = apiService;
            _idTransferStore = idTransferStore;

            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Values",
                    Fill = Brushes.Transparent,
                    LineSmoothness = 0,
                    PointGeometry = DefaultGeometries.Circle,
                    PointForeground = Brushes.SkyBlue,
                    PointGeometrySize = 7,
                    Values = new ChartValues<ObservablePoint>()
                }
            };
            DateTimeFormatter = value => new DateTime((long)value).ToString("dd/MM/yyyy");
            ValueFormatter = value => value.ToString("0.00");

            MainButtonCommand = new RelayCommand(MainButton);
            LoadReportLongTimeCommand = new RelayCommand(LoadReportLongTime);
            LoadOEECommand = new RelayCommand(LoadOEEReport); ;
            LoadACommand = new RelayCommand(LoadAReport); ;
            LoadPCommand = new RelayCommand(LoadPReport); ;
            LoadQCommand = new RelayCommand(LoadQReport); ;
        }

        private void LoadOEEReport()
        {
            IsSeleted = "OEE";
            FullOEEVis = Visibility.Collapsed;
            ChartVis = Visibility.Visible;
            LoadApiReport();
            OEERowVis = Visibility.Visible;
            ARowVis = Visibility.Collapsed;
            PRowVis = Visibility.Collapsed;
            QRowVis = Visibility.Collapsed;
            OnPropertyChanged(nameof(ShiftReportEntries));
            SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShiftReportEntries.Select(g => new ObservablePoint(g.Date.Ticks, g.OEE)));
        }

        private void LoadAReport()
        {
            IsSeleted = "A";
            FullOEEVis = Visibility.Collapsed;
            ChartVis = Visibility.Visible;
            LoadApiReport();
            ARowVis = Visibility.Visible;
            OEERowVis = Visibility.Collapsed;
            PRowVis = Visibility.Collapsed;
            QRowVis = Visibility.Collapsed;
            OnPropertyChanged(nameof(ShiftReportEntries));
            SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShiftReportEntries.Select(g => new ObservablePoint(g.Date.Ticks, g.A)));
        }

        private void LoadPReport()
        {
            IsSeleted = "P";
            FullOEEVis = Visibility.Collapsed;
            ChartVis = Visibility.Visible;
            LoadApiReport();
            PRowVis = Visibility.Visible;
            OEERowVis = Visibility.Collapsed;
            ARowVis = Visibility.Collapsed;
            QRowVis = Visibility.Collapsed;
            OnPropertyChanged(nameof(ShiftReportEntries));
            SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShiftReportEntries.Select(g => new ObservablePoint(g.Date.Ticks, g.P)));
        }

        private void LoadQReport()
        {
            IsSeleted = "Q";
            FullOEEVis = Visibility.Collapsed;
            ChartVis = Visibility.Visible;
            LoadApiReport();
            QRowVis = Visibility.Visible;
            ARowVis = Visibility.Collapsed;
            PRowVis = Visibility.Collapsed;
            OEERowVis = Visibility.Collapsed;
            OnPropertyChanged(nameof(ShiftReportEntries));
            SeriesCollection[0].Values = new ChartValues<ObservablePoint>(ShiftReportEntries.Select(g => new ObservablePoint(g.Date.Ticks, g.Q)));
        }

        private void LoadReportLongTime()
        {
            IsSeleted = "All";
            FullOEEVis = Visibility.Visible;
            ChartVis = Visibility.Collapsed;
            QRowVis = Visibility.Collapsed;
            ARowVis = Visibility.Collapsed;
            PRowVis = Visibility.Collapsed;
            OEERowVis = Visibility.Collapsed;
            LoadApiReport();
        }

        private async void LoadApiReport()
        {
            try
            {
                var dtos = await _apiService.GetShiftReportHistoryAsync("HC001", StartDate, EndDate);
                ShiftReportEntries = new(dtos);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private void MainButton()
        {
            switch (IsSeleted)
            {
                case "All": LoadReportLongTime(); break;
                case "OEE": LoadOEEReport(); break;
                case "A": LoadAReport(); break;
                case "P": LoadPReport(); break;
                case "Q": LoadQReport(); break;
                    default: break;

            }
        }
    }
}
