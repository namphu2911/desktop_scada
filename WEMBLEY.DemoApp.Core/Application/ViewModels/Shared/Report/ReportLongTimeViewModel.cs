using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineReport
{
    public class ReportLongTimeViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly DeviceStore _deviceStore;
        public ObservableCollection<string> DeviceIds => _deviceStore.DeviceIds;
        //
        private readonly DeviceSelectedStore _deviceSelectedStore;
        public string SeletedDeviceId => _deviceSelectedStore.SeletedDeviceId;
        public string DeviceId { get; set; } = "";
        //
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-65).Date;
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
        public ObservableCollection<ShiftReportDto> ShiftTableEntries { get; set; } = new();
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
        public ObservableCollection<string> Datelabel { get; set; } = new();
        public ICommand LoadReportViewCommand { get; set; }
        public ICommand MainButtonCommand { get; set; }
        public ICommand ExportReportCommand { get; set; }
        public ICommand LoadReportLongTimeCommand { get; set; }
        public ICommand LoadOEECommand { get; set; }
        public ICommand LoadACommand { get; set; }
        public ICommand LoadPCommand { get; set; }
        public ICommand LoadQCommand { get; set; }
        public ReportLongTimeViewModel(IApiService apiService, IdTransferStore idTransferStore, DeviceStore deviceStore, DeviceSelectedStore deviceSelectedStore)
        {
            _apiService = apiService;
            _idTransferStore = idTransferStore;
            _deviceStore = deviceStore;
            _deviceSelectedStore = deviceSelectedStore;

            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Values",
                    Fill = Brushes.Transparent,
                    LineSmoothness = 2,
                    PointGeometry = DefaultGeometries.Circle,
                    PointForeground = Brushes.SkyBlue,
                    PointGeometrySize = 7
                }
            };

            LoadReportViewCommand = new RelayCommand(LoadReportView);
            MainButtonCommand = new RelayCommand(MainButton);
            ExportReportCommand = new RelayCommand<string>(ExportReport);
            LoadReportLongTimeCommand = new RelayCommand(LoadReportLongTime);
            LoadOEECommand = new RelayCommand(LoadOEEReport); ;
            LoadACommand = new RelayCommand(LoadAReport); ;
            LoadPCommand = new RelayCommand(LoadPReport); ;
            LoadQCommand = new RelayCommand(LoadQReport); ;
        }

       
        private async void ExportReport(string? filePath)
        {
            if (filePath is not null)
            {
                try
                {
                    var fileBytes = await _apiService.DownloadShiftReportFileAsync(DeviceId, StartDate, EndDate);
                    if (fileBytes != null)
                    {
                        File.WriteAllBytes(filePath, fileBytes);
                        MessageBox.Show("File downloaded successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to download the file.");
                    }
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra");
                }
            }
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

            Datelabel = new (ShiftReportEntries.Select(g => $"{g.Date:dd/MM/yyyy} - {g.ShiftNumber}"));
            SeriesCollection[0].Values = ShiftReportEntries.Select(g => Math.Round(g.OEE,2)).AsChartValues();
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

            Datelabel = new(ShiftReportEntries.Select(g => $"{g.Date:dd/MM/yyyy} - {g.ShiftNumber}"));
            SeriesCollection[0].Values = ShiftReportEntries.Select(g => Math.Round(g.A, 2)).AsChartValues();
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

            Datelabel = new(ShiftReportEntries.Select(g => $"{g.Date:dd/MM/yyyy} - {g.ShiftNumber}"));
            SeriesCollection[0].Values = ShiftReportEntries.Select(g => Math.Round(g.P, 2)).AsChartValues();
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

            Datelabel = new(ShiftReportEntries.Select(g => $"{g.Date:dd/MM/yyyy} - {g.ShiftNumber}"));
            SeriesCollection[0].Values = ShiftReportEntries.Select(g => Math.Round(g.Q, 2)).AsChartValues();
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
                var dtos = await _apiService.GetShiftReportHistoryAsync(DeviceId, StartDate, EndDate);
                foreach (var d in dtos)
                {
                    d.UpdateOEE(d.OEE,d.A , d.P, d.Q);
                }
                ShiftReportEntries = new(dtos.OrderBy(s => s.Date).ThenBy(s => s.ShiftNumber));
                ShiftTableEntries = new(dtos);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            OnPropertyChanged(nameof(ShiftReportEntries));
            OnPropertyChanged(nameof(ShiftTableEntries));
        }

        private void LoadReportView()
        {
            DeviceId = SeletedDeviceId;
            MainButton();
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
