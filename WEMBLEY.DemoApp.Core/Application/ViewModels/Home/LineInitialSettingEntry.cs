using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;
using WEMBLEY.DemoApp.Core.Domain.Exceptions;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class LineInitialSettingEntry : BaseViewModel
    {
        private PersonStore? _personStore;
        public ObservableCollection<string>? PersonIds => _personStore?.PersonIds;
        public ObservableCollection<string>? PersonNames => _personStore?.PersonNames;
        private IApiService? _apiService;
        private IMapper? _mapper;
        public string DeviceType { get; set; }
        public string ProductName { get; set; }
        public string RefName { get; set; }
        public string LotId { get; set; }
        public int LotSize { get; set; }
        public List<DeviceInfoViewModel> Devices { get; set; }

        public ObservableCollection<string>? DeviceIds => new(Devices.Select(i => i.DeviceId).Distinct());
        public string DeviceId { get; set; } = "";
        private string? personId;
        private string? personName;
        public string? PersonId
        {
            get
            {
                return personId;
            }
            set
            {
                personId = value;
                if (_personStore is not null && !String.IsNullOrEmpty(value))
                {
                    var person = _personStore.Persons.First(i => i.PersonId == personId);
                    personName = person.PersonName;
                    OnPropertyChanged(nameof(PersonName));
                }
            }

        }
        public string? PersonName
        {
            get
            {
                return personName;
            }
            set
            {
                personName = value;
                if (_personStore is not null && !String.IsNullOrEmpty(value))
                {
                    var person = _personStore.Persons.First(i => i.PersonName == personName);
                    personId = person.PersonId;
                    OnPropertyChanged(nameof(PersonId));
                }
            }
        }
        public ICommand CreateSublotCommand { get; set; }
        public ICommand UpdateLotCommand { get; set; }
        public ICommand CompleteRefCommand { get; set; }
        public event EventHandler? SublotCreated;
        public event EventHandler? DeletedPerson;

        public event Action? Updated;
        public event Action? OnException;
        public LineInitialSettingEntry(string deviceType, string productName, string refName, string lotId, int lotSize, List<DeviceInfoViewModel> devices)
        {
            DeviceType = deviceType;
            ProductName = productName;
            RefName = refName;
            LotId = lotId;
            LotSize = lotSize;
            Devices = devices;

            CreateSublotCommand = new RelayCommand(CreateSublot);
            UpdateLotCommand = new RelayCommand(UpdateLot);
            CompleteRefCommand = new RelayCommand(CompleteRef);
            foreach (var device in Devices)
            {
                device.OnRemoved += Device_OnRemoved;
            }
        }

        private async void UpdateLot()
        {
            var createDto = new CreateLotDto(
                LotId,
                LotSize);
            try
            {
                if (MessageBox.Show("Xác nhận cập nhật", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _apiService.UpdateLotAsync(RefName, createDto);
                    Updated?.Invoke();
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

        private async void CompleteRef()
        {
            try
            {
                if (MessageBox.Show("Xác nhận kết thúc Ref", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _apiService.CompleteRefAsync(RefName);
                    Updated?.Invoke();
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



        private void Device_OnRemoved(object? sender, EventArgs e)
        {
            if(sender is not null)
            {
                DeletedPerson?.Invoke(sender, EventArgs.Empty);
            }
        }

        public void SetApiService(IApiService apiService)
        {
            _apiService = apiService;
        }
        public void SetMapper(IMapper mapper)
        {
            _mapper = mapper;
            OnPropertyChanged();
        }
        public void SetStore(PersonStore personStore)
        {
            _personStore = personStore;
            OnPropertyChanged();
        }

        private void CreateSublot()
        {
            SublotCreated?.Invoke(this, EventArgs.Empty);
        }
    }
}
