﻿using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
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
        private readonly ISignalRClient _signalRClient;
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
                    case EMachineStatus.On:
                        {
                            ColorBack = "#394963";
                            break;
                        }
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
                            ColorBack = "#BBBBBB";
                            break;
                        }
                }
            }
        }

        public string ColorBack { get; set; } = "#BBBBBB";
        public double HerapinCapEfficiency { get; set; } = 0;
        public long HerapinCapAllCount { get; set; } = 0;
        public long HerapinCapGoodCount { get; set; } = 0;
        public long HerapinCapBadCount { get; set; } = 0;
        public TimeSpan? HerapinCapDurationTime { get; set; }
        public string HerapinCapProductName { get; set; } = "";
        public string HerapinCapReferenceName { get; set; } = "";
        public string HerapinCapLotId { get; set; } = "";
        public int HerapinCapLotSize { get; set; } = 0;

        //
        //
        public ICommand LoadHomeViewCommand { get; set; }

        ///
        public MachinesInLine1ViewModel MachinesInLine1 { get; set; }
        public MachinesInLine2ViewModel MachinesInLine2 { get; set; }
        public HomeViewModel(IApiService apiService, ISignalRClient signalRClient, MachinesInLine1ViewModel machinesInLine1, MachinesInLine2ViewModel machinesInLine2)
        {
            _apiService = apiService;
            _signalRClient = signalRClient;

            MachinesInLine1 = machinesInLine1;
            MachinesInLine2 = machinesInLine2;

            LoadHomeViewCommand = new RelayCommand(LoadHomeView);

            signalRClient.OnTagChanged += OnTagChanged;
        }

        private async void LoadHomeView()
        {
            var a = await _signalRClient.GetBufferList();
            if (a.Count != 0)
            {
                Status = (EMachineStatus)Convert.ToInt32(await _signalRClient.GetBufferValue("machineStatus"));
                HerapinCapDurationTime = TimeSpan.TryParse(Convert.ToString((await _signalRClient.GetBufferValue("operationTime"))), out var span) ? span : default;
                HerapinCapGoodCount = Convert.ToInt64(await _signalRClient.GetBufferValue("goodProduct"));
                HerapinCapBadCount = Convert.ToInt64(await _signalRClient.GetBufferValue("errorProduct"));
                HerapinCapEfficiency = Convert.ToDouble(await _signalRClient.GetBufferValue("EFF"));
                HerapinCapAllCount = Convert.ToInt64(await _signalRClient.GetBufferValue("productCount"));
            }
            LoadLotSettingAsync();
        }

        private void OnTagChanged(string json)
        {
            LoadLotSettingAsync();
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                if(tag.StationId == "IE-F2-HCA01")
                {
                    switch (tag.TagId)
                    {
                        case "machineStatus":
                            {
                                Status = (EMachineStatus)Convert.ToInt32(tag.TagValue);
                                LoadLotSettingAsync();
                                break;
                            }
                        case "operationTimeRaw": HerapinCapDurationTime = TimeSpan.Parse((string)tag.TagValue); break;
                        case "goodProductRaw": HerapinCapGoodCount = Convert.ToInt64(tag.TagValue); break;
                        case "errorProduct": HerapinCapBadCount = Convert.ToInt64(tag.TagValue); break;
                        case "EFF": HerapinCapEfficiency = Convert.ToDouble(tag.TagValue); break;
                        case "productCount": HerapinCapAllCount = Convert.ToInt64(tag.TagValue); break;
                        default: break;
                    }
                }
            }
        }

        private async void LoadLotSettingAsync()
        {
            try
            {
                var dtos = await _apiService.GetLotDeviceReferenceByDeviceAsync("HerapinCap");
                HerapinCapLotId = dtos.Last().LotCode;
                HerapinCapLotSize = dtos.Last().LotSize; 
                HerapinCapLotId = dtos.Last().LotCode;
                HerapinCapLotSize = dtos.Last().LotSize;
                if (string.IsNullOrEmpty(HerapinCapLotId))
                {
                    HerapinCapProductName = "";
                    HerapinCapReferenceName = "";
                }
                else
                {
                    HerapinCapProductName = dtos.Last().ProductName;
                    HerapinCapReferenceName = dtos.Last().ReferenceName;
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}