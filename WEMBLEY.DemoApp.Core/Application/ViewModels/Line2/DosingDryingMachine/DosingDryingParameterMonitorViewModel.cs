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
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.Line2.DosingDryingMachine
{
    public class DosingDryingParameterMonitorViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly ISignalRClient _signalRClient;
        private readonly HomeDataStore _homeDataStore;

        public ObservableCollection<ComparedMFC> MFCEntries { get; set; } = new();
        public List<MFCDto> MFCDtos { get; set; } = new();
        public List<RealMFC> RealMFCValues { get; set; } = new();
        public List<TagChangedNotification> AllTags { get; set; } = new();
        public ICommand LoadMFCMonitorViewCommand { get; set; }
        public string HomeRefId => _homeDataStore.HomeDatas.First(i => i.Line.LineId == "NonVacuumBloodTube").ReferenceId;

        public DosingDryingParameterMonitorViewModel(ISignalRClient signalRClient, IApiService apiService, HomeDataStore homeDataStore)
        {
            _signalRClient = signalRClient;
            _apiService = apiService;
            _homeDataStore = homeDataStore;

            signalRClient.OnTagChanged += OnTagChanged;
            LoadMFCMonitorViewCommand = new RelayCommand(LoadMFCMonitorViewAsync);
        }

        private async void LoadMFCMonitorViewAsync()
        {
            AllTags = await _signalRClient.GetBufferList();
            if (AllTags.Count != 0)
            {
                RealMFCValues = new()
                {
                    new RealMFC("Spray Valve", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_VALVE"))),
                    new RealMFC("T Pre Spray", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_T_PRE_SPRAY"))),
                    new RealMFC("T Spraying", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_T_SPRAYING"))),
                    new RealMFC("Spray Delay", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_DELAY"))),
                    new RealMFC("Nozzle Clean", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_NOZZLE_CLEAN"))),
                    new RealMFC("Wait To Clean Time", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_WAIT_TO_CLEAN_TIME"))),
                    new RealMFC("Clean Duration Time", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_CLEAN_DURATION_TIME"))),
                    new RealMFC("Spray Fill Way Inserted", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_FILL_WAY_INSERTED"))),

                    new RealMFC("Drying Times", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_VALVE"))),
                    new RealMFC("Position 01", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_T_PRE_SPRAY"))),
                    new RealMFC("Position 12", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_T_SPRAYING"))),
                    new RealMFC("Position 23", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_DELAY"))),
                    new RealMFC("Pause 1", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_NOZZLE_CLEAN"))),
                    new RealMFC("Pause 2", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_WAIT_TO_CLEAN_TIME"))),
                    new RealMFC("Pause 3", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_CLEAN_DURATION_TIME"))),
                    new RealMFC("T Pre Spray Ion", Convert.ToInt64(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_FILL_WAY_INSERTED"))),

                    new RealMFC("Robot Y Pos 1", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_VALVE"))),
                    new RealMFC("Robot Y Pos 2", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_T_PRE_SPRAY"))),
                    new RealMFC("Robot Z Pos 1", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_T_SPRAYING"))),
                    new RealMFC("Robot Z Pos 2", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_DELAY"))),
                    new RealMFC("Robot Z Pos 3", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_NOZZLE_CLEAN"))),
                    new RealMFC("Robot Z Pos 4", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_WAIT_TO_CLEAN_TIME"))),
                    new RealMFC("Robot Z Pos 5", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_CLEAN_DURATION_TIME"))),

                    new RealMFC("FS Max", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_FILL_WAY_INSERTED"))),
                    new RealMFC("FS Min", Convert.ToDouble(await _signalRClient.GetBufferValue("IE-F3-BLO06","S1_PARA_SPRAY_FILL_WAY_INSERTED"))),

                    new RealMFC("Heating Temp SP", Convert.ToDouble(await _signalRClient.GetBufferValue("S8_OFF_SET_TR1")))
                };      
                OnPropertyChanged(nameof(RealMFCValues));
            }
            else
            {
                RealMFCValues = new()
                {
                    new RealMFC("Spray Valve", 0),
                    new RealMFC("T Pre Spray", 0),
                    new RealMFC("T Spraying", 0),
                    new RealMFC("Spray Delay", 0),
                    new RealMFC("Nozzle Clean", 0),
                    new RealMFC("Wait To Clean Time", 0),
                    new RealMFC("Clean Duration Time", 0),
                    new RealMFC("Spray Fill Way Inserted", 0),

                    new RealMFC("Drying Times", 0),
                    new RealMFC("Position 01", 0),
                    new RealMFC("Position 12", 0),
                    new RealMFC("Position 23", 0),
                    new RealMFC("Pause 1", 0),
                    new RealMFC("Pause 2", 0),
                    new RealMFC("Pause 3", 0),
                    new RealMFC("T Pre Spray Ion", 0),

                    new RealMFC("Robot Y Pos 1", 0),
                    new RealMFC("Robot Y Pos 2", 0),
                    new RealMFC("Robot Z Pos 1", 0),
                    new RealMFC("Robot Z Pos 2", 0),
                    new RealMFC("Robot Z Pos 3", 0),
                    new RealMFC("Robot Z Pos 4", 0),
                    new RealMFC("Robot Z Pos 5", 0),

                    new RealMFC("FS Max", 0),
                    new RealMFC("FS Min", 0),

                    new RealMFC("Heating Temp SP", 0)
                };
            }

            OnPropertyChanged(nameof(HomeRefId));
            try
            {
                if (!(String.IsNullOrEmpty(HomeRefId)))
                {
                    var dtos = await _apiService.GetStationReferencesMFCAsync("IE-F3-BLO06", HomeRefId);
                    MFCDtos = dtos.Last().MFCs;
                }
                ReloadData();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private void OnTagChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);
            if (tag != null)
            {
                if (tag.StationId == "IE-F2-HCA01")
                {
                    switch (tag.TagId)
                    {
                        case "S1_PARA_SPRAY_VALVE":
                            RealMFCValues[0] = new RealMFC("Spray Valve", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_T_PRE_SPRAY":
                            RealMFCValues[1] = new RealMFC("T Pre Spray", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_T_SPRAYING":
                            RealMFCValues[2] = new RealMFC("T Spraying", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_SPRAY_DELAY":
                            RealMFCValues[3] = new RealMFC("Spray Delay", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_NOZZLE_CLEAN":
                            RealMFCValues[4] = new RealMFC("Nozzle Clean", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_WAIT_TO_CLEAN_TIME":
                            RealMFCValues[5] = new RealMFC("Wait To Clean Time", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_CLEAN_DURATION_TIME":
                            RealMFCValues[6] = new RealMFC("Clean Duration Time", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_SPRAY_FILL_WAY_INSERTED":
                            RealMFCValues[7] = new RealMFC("Spray Fill Way Inserted", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_DRYING_TIMES":
                            RealMFCValues[8] = new RealMFC("Drying Times", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_POSITION_01":
                            RealMFCValues[9] = new RealMFC("Position 01", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_POSITION_12":
                            RealMFCValues[10] = new RealMFC("Position 12", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_POSITION_23":
                            RealMFCValues[11] = new RealMFC("Position 23", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_PAUSE_1":
                            RealMFCValues[12] = new RealMFC("Pause 1", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_PAUSE_2":
                            RealMFCValues[13] = new RealMFC("Pause 2", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_PAUSE_3":
                            RealMFCValues[14] = new RealMFC("Pause 3", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_T_PRE_SPRAY_ION":
                            RealMFCValues[15] = new RealMFC("T Pre Spray Ion", Convert.ToInt64(tag.TagValue));
                            break;
                        case "S1_PARA_ROBOT_Y_POS_1":
                            RealMFCValues[16] = new RealMFC("Robot Y Pos 1", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_ROBOT_Y_POS_2":
                            RealMFCValues[17] = new RealMFC("Robot Y Pos 2", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_ROBOT_Z_POS_1":
                            RealMFCValues[18] = new RealMFC("Robot Z Pos 1", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_ROBOT_Z_POS_2":
                            RealMFCValues[19] = new RealMFC("Robot Z Pos 2", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_ROBOT_Z_POS_3":
                            RealMFCValues[20] = new RealMFC("Robot Z Pos 3", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_ROBOT_Z_POS_4":
                            RealMFCValues[21] = new RealMFC("Robot Z Pos 4", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_ROBOT_Z_POS_5":
                            RealMFCValues[22] = new RealMFC("Robot Z Pos 5", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_FS_MAX":
                            RealMFCValues[23] = new RealMFC("FS Max", Convert.ToDouble(tag.TagValue));
                            break;
                        case "S1_PARA_FS_MIN":
                            RealMFCValues[24] = new RealMFC("FS Min", Convert.ToDouble(tag.TagValue));
                            break;
                        default: break;
                    }
                }
            }
            OnPropertyChanged(nameof(RealMFCValues));
            ReloadData();
        }

        private void ReloadData()
        {
            //var newViewModels = MFCDtos.Select((tag, index) => new ComparedMFC(tag.MFCName, tag.Value, tag.MinValue, tag.MaxValue, Convert.ToDouble(RealMFCValues.First(i => i.Name == tag.MFCName).RealValue))).ToList();
            //MFCEntries = new(newViewModels);
        }
    }
}
