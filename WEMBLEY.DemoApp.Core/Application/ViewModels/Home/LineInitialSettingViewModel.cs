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
using WEMBLEY.DemoApp.Core.Domain.Dtos;
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
        public ObservableCollection<string> DeviceTypes => _referenceStore.DeviceTypes;
        public ObservableCollection<string> ProductNames => _referenceStore.ProductNames;
        public ObservableCollection<string> ReferenceNames => _referenceStore.ReferenceNames;
        public ObservableCollection<string> ProductNamesFilled { get; set; } = new();
        public ObservableCollection<string> ReferenceNamesFilled { get; set; } = new();

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
                    var sortedReference = _referenceStore.References.Where(i => i.DeviceType == deviceType).ToList();
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
                    ReferenceNamesFilled = new ObservableCollection<string>(sortedReference.Select(i => i.RefName).OrderBy(s => s));
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
                    var reference = _referenceStore.References.First(i => i.RefName == referenceName);
                    DeviceType = reference.DeviceType;
                    ProductName = reference.ProductName;
                    OnPropertyChanged(nameof(DeviceType));
                    OnPropertyChanged(nameof(ProductName));
                }
                    

            }
        }

        public string LotId { get; set; } = "";
        public int LotSize { get; set; } = 0;

        public ObservableCollection<LineInitialSettingEntry> LotSettingEntries { get; set; } = new();
        public ICommand CreateInitialSettingCommand { get; set; }
        public ICommand LoadLineInitialSettingViewCommand { get; set; }
        public LineInitialSettingViewModel(IApiService apiService, IMapper mapper, IDatabaseSynchronizationService databaseSynchronizationService, ReferenceStore referenceStore)
        {
            _apiService = apiService;
            _mapper = mapper;
            _databaseSynchronizationService = databaseSynchronizationService;
            _referenceStore = referenceStore;

            CreateInitialSettingCommand = new RelayCommand(CreateInitialSetting);
            LoadLineInitialSettingViewCommand = new RelayCommand(LoadLineInitialSettingView);
        }

        private async void CreateInitialSetting()
        {
            var createDto = new CreateLotDto(
                LotId,
                LotSize);
            try
            {
                await _apiService.CreateLot(ReferenceName, createDto);
                //LoadManageItemView();
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
            LotId = "";
            LotSize = 0;
            DeviceType = "";
            ProductName = "";
            ReferenceName = "";
        }

        private void LoadLineInitialSettingView()
        {
            _databaseSynchronizationService.SynchronizeReferencesData();
            OnPropertyChanged(nameof(DeviceTypes));

            ProductNamesFilled = new ObservableCollection<string>(ProductNames);
            ReferenceNamesFilled = new ObservableCollection<string>(ReferenceNames);
            OnPropertyChanged(nameof(ProductNamesFilled));
            OnPropertyChanged(nameof(ReferenceNamesFilled));

            LoadLotSettingAsync();
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                var dtos = await _apiService.GetAllLotDeviceReferenceAsync();
                var viewModels = _mapper.Map<IEnumerable<LotDeviceReferenceDto>, IEnumerable<LineInitialSettingEntry>>(dtos);
                LotSettingEntries = new(viewModels);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
