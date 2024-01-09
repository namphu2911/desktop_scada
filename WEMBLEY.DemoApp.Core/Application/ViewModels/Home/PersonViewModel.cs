using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class PersonViewModel : BaseViewModel
    {
        private IApiService _apiService;
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public ICommand DeleteCommand { get; set; }
        public event Action? OnRemoved;
        public event Action? OnException;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PersonViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            DeleteCommand = new RelayCommand(DeleteAsync);
        }

        public PersonViewModel(IApiService apiService, string personId, string personName)
        {
            _apiService = apiService;
            PersonId = personId;
            PersonName = personName;
            
            DeleteCommand = new RelayCommand(DeleteAsync);
        }

        public void SetApiService(IApiService apiService)
        {
            try
            {
                _apiService = apiService;
            }
            catch (HttpRequestException)
            {
                OnException?.Invoke();
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void DeleteAsync()
        {
            try
            {
                if (MessageBox.Show("Xác nhận xóa", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _apiService.DeletePersonAsync(PersonId);
                    OnRemoved?.Invoke();
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else { }
            }
            catch (HttpRequestException)
            {
                OnException?.Invoke();
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
