using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.MachineStatus;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineStatus
{
    public class StopperMachineStatusViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7).Date;
        public DateTime EndDate { get; set; } = DateTime.Today.Date;
        public ICommand LoadMachineStatusHistoryCommand { get; set; }

        public ObservableCollection<MachineStatusDto> StatusHistoryEntries { get; set; } = new();
        public StopperMachineStatusViewModel(IApiService apiService)
        {
            _apiService = apiService;
            LoadMachineStatusHistoryCommand = new RelayCommand(LoadMachineStatusHistory);
        }

        private async void LoadMachineStatusHistory()
        {
            try
            {
                var dtos = await _apiService.GetStatusHistoryAsync("HC001", StartDate, EndDate);
                StatusHistoryEntries = new(dtos);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
