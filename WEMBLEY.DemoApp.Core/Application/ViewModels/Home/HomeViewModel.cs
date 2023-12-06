using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.MachinesInLine;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IDatabaseSynchronizationService _databaseSynchronizationService;
        private readonly ISignalRClient _signalRClient;

        private readonly ReferenceStore _referenceStore;
        public ObservableCollection<string> HerapinCapProductNames => _referenceStore.HerapinCapProductNames;
        public ObservableCollection<string> HerapinCapReferenceNames => _referenceStore.HerapinCapReferenceNames;
        public ObservableCollection<string> HerapinCapReferenceNamesFilled { get; set; } = new();
        //
        //private string herapinCapProductName = "";
        //private string herapinCapReferenceName = "";
        //public string HerapinCapProductName
        //{
        //    get
        //    {
        //        return herapinCapProductName;
        //    }
        //    set
        //    {
        //        herapinCapProductName = value;
        //        if (String.IsNullOrEmpty(value))
        //        {
        //            herapinCapReferenceName = "";
        //            HerapinCapReferenceNamesFilled = new ObservableCollection<string>(HerapinCapReferenceNames);
        //            OnPropertyChanged(nameof(HerapinCapReferenceNamesFilled));
        //            OnPropertyChanged(nameof(HerapinCapReferenceName));
        //        }
        //        else
        //        {
        //            var sortedReference = _referenceStore.References.Where(i => i.ProductName == herapinCapProductName).ToList();
        //            HerapinCapReferenceNamesFilled = new ObservableCollection<string>(sortedReference.Select(i => i.RefName).OrderBy(s => s));
        //            OnPropertyChanged(nameof(HerapinCapReferenceNamesFilled));
        //            OnPropertyChanged(nameof(HerapinCapReferenceName));
        //        }
        //    }
        //}
        //public string HerapinCapReferenceName
        //{
        //    get
        //    {
        //        return herapinCapReferenceName;
        //    }
        //    set
        //    {
        //        herapinCapReferenceName = value;
        //        if (String.IsNullOrEmpty(value))
        //        {
        //            herapinCapProductName = "";
        //            OnPropertyChanged(nameof(HerapinCapProductName));
        //        }
        //        else
        //        {
        //            var reference = _referenceStore.References.First(i => i.RefName == herapinCapReferenceName);
        //            herapinCapProductName = reference.ProductName;
        //            OnPropertyChanged(nameof(HerapinCapProductName));
        //        }
        //    }
        //}



        //
        private EMachineStatus status;
        public EMachineStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                switch (value)
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
        public double? HerapinCapEfficiency { get; set; }
        public long? HerapinCapAllCount { get; set; } 
        public long? HerapinCapGoodCount { get; set; } 
        public long? HerapinCapBadCount { get; set; }
        public TimeSpan HerapinCapDurationTime { get; set; }
        public string HerapinCapProductName { get; set; } = "";
        public string HerapinCapReferenceName { get; set; } = "";
        public string HerapinCapLotId { get; set; } = "";
        public int HerapinCapLotSize { get; set; } = 0;

        //
        //
        public ICommand LoadHomeViewCommand { get; set; }

        ///
        public MachinesInLine1ViewModel MachinesInLine1 { get; set; }

        public HomeViewModel(IApiService apiService, IDatabaseSynchronizationService databaseSynchronizationService, ISignalRClient signalRClient, ReferenceStore referenceStore, MachinesInLine1ViewModel machinesInLine1)
        {
            _apiService = apiService;
            _databaseSynchronizationService = databaseSynchronizationService;
            _signalRClient = signalRClient;
            _referenceStore = referenceStore;

            MachinesInLine1 = machinesInLine1;

            LoadHomeViewCommand = new RelayCommand(LoadHomeView);

            signalRClient.OnTagChanged += OnTagChanged;
        }

        private async void LoadHomeView()
        {
            await _databaseSynchronizationService.SynchronizeReferencesData();
            OnPropertyChanged(nameof(HerapinCapProductNames));

            HerapinCapReferenceNamesFilled = new ObservableCollection<string>(HerapinCapReferenceNames);
            OnPropertyChanged(nameof(HerapinCapReferenceNamesFilled));
            OnPropertyChanged(nameof(HerapinCapReferenceNames));

            Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("machineStatus"));
            HerapinCapDurationTime = TimeSpan.Parse((string)(await _signalRClient.GetBufferValue("operationTime")));
            HerapinCapGoodCount = (long?)(await _signalRClient.GetBufferValue("goodProduct"));
            HerapinCapBadCount = (long?)(await _signalRClient.GetBufferValue("errorProduct"));
            HerapinCapEfficiency = (double?)(await _signalRClient.GetBufferValue("EFF"));
            HerapinCapAllCount = (long?)(await _signalRClient.GetBufferValue("productCount"));

            LoadLotSettingAsync();
        }

        private void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);

            switch (tag.TagId)
            {
                case "machineStatus": Status = (EMachineStatus)Convert.ToInt32(tag.TagValue); break;
                case "operationTime": HerapinCapDurationTime = TimeSpan.Parse((string)tag.TagValue); break;
                case "goodProduct": HerapinCapGoodCount = (long?)tag.TagValue; break;
                case "errorProduct": HerapinCapBadCount = (long?)tag.TagValue; break;
                case "EFF": HerapinCapEfficiency = (double?)tag.TagValue; break;
                case "productCount": HerapinCapAllCount = (long?)tag.TagValue; break;
                default: break;
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceTypeAsync("HerapinCap");
                HerapinCapProductName = dtos.Last().ProductName;
                HerapinCapReferenceName = dtos.Last().RefName;
                HerapinCapLotId = dtos.Last().LotId;
                HerapinCapLotSize = dtos.Last().LotSize;
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
