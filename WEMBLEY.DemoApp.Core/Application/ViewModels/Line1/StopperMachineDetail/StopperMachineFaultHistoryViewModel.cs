using AutoMapper;
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
using WEMBLEY.DemoApp.Core.Domain.Dtos.ErrorInformations;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineDetail
{
    public class StopperMachineFaultHistoryViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7).Date;
        public DateTime EndDate { get; set; } = DateTime.Today.Date;
        public ICommand LoadMachineFaultHistoryCommand {  get; set; }

        public ObservableCollection<ErrorStatusDto> ErrorsHistoryEntries { get; set; } = new();
        public StopperMachineFaultHistoryViewModel(IApiService apiService)
        {
            _apiService = apiService;
            LoadMachineFaultHistoryCommand = new RelayCommand(LoadMachineFaultHistory);
        }

        private async void LoadMachineFaultHistory()
        {
            try
            {
                var dtos = await _apiService.GetErrorsHistoryAsync("HC001", StartDate, EndDate);
                ErrorsHistoryEntries = new(dtos);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
