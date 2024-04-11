using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Employees;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;
using WEMBLEY.DemoApp.Core.Domain.Exceptions;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class LineInitialSettingViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;

        private readonly IDatabaseSynchronizationService _databaseSynchronizationService;
        private readonly ReferenceStore _referenceStore;
        private readonly EmployeeStore _personStore;
        public ObservableCollection<string> DeviceTypes => _referenceStore.LineIds;
        public ObservableCollection<string> ProductNames => _referenceStore.ProductNames;
        public ObservableCollection<string> ReferenceNames => _referenceStore.ReferenceNames;
        public ObservableCollection<string> ProductNamesFilled { get; set; } = new();
        public ObservableCollection<string> ReferenceNamesFilled { get; set; } = new();

        public string PersonId { get; set; } = "";
        public string PersonName { get; set; } = "";

        private string deviceType = "";
        private string productName = "";
        private string referenceName = "";
        public string DeviceType
        {
            get
            {
                return deviceType;
            }
            set
            {
                deviceType = value;
                if (String.IsNullOrEmpty(value))
                {
                    productName = "";
                    referenceName = "";
                    ProductNamesFilled = new ObservableCollection<string>(ProductNames);
                    ReferenceNamesFilled = new ObservableCollection<string>(ReferenceNames);
                    OnPropertyChanged(nameof(ProductNamesFilled));
                    OnPropertyChanged(nameof(ReferenceNamesFilled));
                    OnPropertyChanged(nameof(ProductName));
                    OnPropertyChanged(nameof(ReferenceName));
                }
                else
                {
                    var sortedReference = _referenceStore.ReferenceSimples.Where(i => i.LineId == deviceType).ToList();
                    ProductNamesFilled = new ObservableCollection<string>(sortedReference.Select(i => i.ProductName).Distinct().OrderBy(s => s));
                    OnPropertyChanged(nameof(ProductNamesFilled));
                    OnPropertyChanged(nameof(ReferenceNamesFilled));
                    OnPropertyChanged(nameof(ProductName));
                    OnPropertyChanged(nameof(ReferenceName));
                }
            }
        }
        public string ProductName
        {
            get
            {
                return productName;
            }
            set
            {
                productName = value;
                if (String.IsNullOrEmpty(value))
                {
                    referenceName = "";
                    ReferenceNamesFilled = new ObservableCollection<string>(ReferenceNames);
                    OnPropertyChanged(nameof(ReferenceNamesFilled));
                    OnPropertyChanged(nameof(ReferenceName));
                }
                else
                {
                    var sortedReference = _referenceStore.References.Where(i => i.ProductName == productName); 
                    ReferenceNamesFilled = new ObservableCollection<string>(sortedReference.Select(i => i.ReferenceName).OrderBy(s => s));
                    OnPropertyChanged(nameof(ReferenceNamesFilled));
                    OnPropertyChanged(nameof(ReferenceName));
                }
            }
        }

        public string ReferenceName
        {
            get
            {
                return referenceName;
            }
            set
            {
                referenceName = value;
                if (String.IsNullOrEmpty(value))
                {

                }
                else
                {
                    var reference = _referenceStore.ReferenceSimples.First(i => i.ReferenceName == referenceName);
                    DeviceType = reference.LineId;
                    ProductName = reference.ProductName;
                    OnPropertyChanged(nameof(DeviceType));
                    OnPropertyChanged(nameof(ProductName));
                }
                    

            }
        }

        public string LotId { get; set; } = "";
        public int LotSize { get; set; } = 0;

        public ObservableCollection<LineInitialSettingEntry> LotSettingEntries { get; set; } = new();
        public ObservableCollection<PersonViewModel> PersonsEntries { get; set; } = new();
        public ICommand CreateInitialSettingCommand { get; set; }
        public ICommand LoadLineInitialSettingViewCommand { get; set; }
        public ICommand CreatePersonCommand { get; set; }
        public LineInitialSettingViewModel(IApiService apiService, IMapper mapper, IDatabaseSynchronizationService databaseSynchronizationService, ReferenceStore referenceStore, EmployeeStore personStore)
        {
            _apiService = apiService;
            _mapper = mapper;
            _databaseSynchronizationService = databaseSynchronizationService;
            _referenceStore = referenceStore;
            _personStore = personStore;

            CreateInitialSettingCommand = new RelayCommand(CreateInitialSetting);
            LoadLineInitialSettingViewCommand = new RelayCommand(LoadLineInitialSettingView);
            CreatePersonCommand = new RelayCommand(CreatePerson);
        }

        private async void CreateInitialSetting()
        {
            var createDto = new CreateLotDto(
                LotId,
                LotSize);
            try
            {
                await _apiService.CreateLotAsync(ReferenceName, createDto);
                MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadLotSettingAsync();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            catch (DuplicateEntityException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mã vật tư đã tồn tại.");
            }
            catch (Exception)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo mới.");
            }
            LotId = "";
            LotSize = 0;
            DeviceType = "";
            ProductName = "";
            ReferenceName = "";
        }

        private async void CreatePerson()
        {
            var createDto = new EmployeeWorkingDto(
                PersonId,
                PersonName);
            try
            {
                await _apiService.CreatePersonAsync(createDto);
                LoadPersonsAsync();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            catch (DuplicateEntityException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mã vật tư đã tồn tại.");
            }
            catch (Exception)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo mới.");
            }
            MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            PersonId = "";
            PersonName = "";
        }

        private void LoadLineInitialSettingView()
        {
            _databaseSynchronizationService.SynchronizeReferencesData();
            OnPropertyChanged(nameof(DeviceTypes));

            ProductNamesFilled = new ObservableCollection<string>(ProductNames);
            ReferenceNamesFilled = new ObservableCollection<string>(ReferenceNames);
            OnPropertyChanged(nameof(ProductNamesFilled));
            OnPropertyChanged(nameof(ReferenceNamesFilled));

            LoadPersonsAsync();
        }
        private async void LoadPersonsAsync()
        {
            try
            {
                var dtos = await _apiService.GetAllPersonAsync();
                var viewModels = _mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<PersonViewModel>>(dtos);
                PersonsEntries = new(viewModels);
                foreach (var viewModel in PersonsEntries)
                {
                    viewModel.OnRemoved += LoadPersonsAsync;
                    viewModel.SetApiService(_apiService);
                    viewModel.OnException += Error;
                }


                await _databaseSynchronizationService.SynchronizeEmployeesData();
                LoadLotSettingAsync();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                var dtos = await _apiService.GetAllLotDeviceReferenceAsync();
                foreach(var dto in dtos)
                {
                    foreach(var station in  dto.Stations)
                    {
                        if(station.Employees.Count == 0)
                        {
                            station.Employees.Add(new EmployeeWorkingDto("", ""));
                        }
                    }
                }
                var viewModels = dtos.Select(c => new LineInitialSettingEntry(
                    c.Line.LineName,
                    c.ProductName,
                    c.ReferenceName,
                    c.LotCode,
                    c.LotSize,
                    c.Stations.SelectMany(i => i.Employees.Select(x => new DeviceInfoViewModel(
                        i.StationId,
                        x.EmployeeId,
                        x.EmployeeName))).ToList()));

                LotSettingEntries = new(viewModels);

                //if (viewModels != null)
                //{
                //    foreach (var viewModel in viewModels)
                //    {
                //        for (int i = 0; i < viewModel.Devices.Count - 1; i++)
                //        {
                //            if (viewModel.Devices[i + 1].StationId == viewModel.Devices[i].StationId)
                //            {
                //                viewModel.Devices[i + 1].StationId = "";
                //            }
                //            OnPropertyChanged(nameof(viewModels));
                //        }
                //    }
                //    LotSettingEntries = new(viewModels);
                //}

                foreach (var entry in LotSettingEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);
                    entry.SetStore(_personStore);
                    entry.SublotCreated += CreateSublot;
                    entry.DeletedPerson += DeleteAPerson;
                    entry.Updated += LoadLotSettingAsync;
                    entry.OnException += Error;
                }

                await _databaseSynchronizationService.SynchronizeHomeData();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void CreateSublot(object? sender, EventArgs args)
        {
            if (sender is not null)
            {
                var lot = (LineInitialSettingEntry)sender;
                var listPersonIds  = new List<string>();
                listPersonIds.Add(lot.PersonId!);
                var createDto = new AddPersonToDeviceDto(listPersonIds);
                try
                {
                    await _apiService.AddPersonToDeviceAsync(lot.DeviceId, createDto);
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
                catch (DuplicateEntityException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mã vật tư đã tồn tại.");
                }
                catch (Exception)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo mới.");
                }
                MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            LoadLotSettingAsync();
        }

        private async void DeleteAPerson(object? sender, EventArgs e)
        {
            if (sender is not null)
            {
                var persons = (DeviceInfoViewModel)sender;
                var listPersonIds = new List<string>();
                listPersonIds.Add(persons.PersonId);
                var createDto = new AddPersonToDeviceDto(listPersonIds);
                try
                {
                    await _apiService.DeletePersonToDeviceAsync(persons.DeviceId, createDto);
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
                catch (DuplicateEntityException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mã vật tư đã tồn tại.");
                }
                catch (Exception)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo mới.");
                }
                MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            LoadLotSettingAsync();
        }

        private void Error()
        {
            ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
        }
    }
}
