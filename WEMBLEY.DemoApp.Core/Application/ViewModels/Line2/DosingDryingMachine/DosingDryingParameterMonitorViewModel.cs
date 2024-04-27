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
        public ObservableCollection<ComparedMFC> DosingEntries { get; set; } = new();
        public ObservableCollection<ComparedMFC> Drying1Entries { get; set; } = new();
        public ObservableCollection<ComparedMFC> Drying2Entries { get; set; } = new();
        public ObservableCollection<ComparedMFC> RobotArmEntries { get; set; } = new();
        public ObservableCollection<ComparedMFC> OthersEntries { get; set; } = new();

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
                    new RealMFC("Spray Valve", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_SPRAY_VALVE")),
                    new RealMFC("T Pre Spray", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_T_PRE_SPRAY")),
                    new RealMFC("T Spraying", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_T_SPRAYING")),
                    new RealMFC("Spray Delay", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_SPRAY_DELAY")),
                    new RealMFC("Nozzle Clean", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_NOZZLE_CLEAN")),
                    new RealMFC("Wait To Clean Time", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_WAIT_TO_CLEAN_TIME")),
                    new RealMFC("Clean Duration Time", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_CLEAN_DURATION_TIME")),
                    new RealMFC("Spray Fill Way Inserted", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_SPRAY_FILL_WAY_INSERTED")),
                    new RealMFC("Trig Pos A", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TRIG_POS_A")),
                    new RealMFC("Trig Pos B", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TRIG_POS_B")),
                    new RealMFC("T Moving", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_T_MOVING")),
                    new RealMFC("Testing Dosing Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_DOSING_BIT")),

                    new RealMFC("Dry1 Times", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_TIMES")),
                    new RealMFC("Dry1 Position 01", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_POSITION_01")),
                    new RealMFC("Dry1 Position 12", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_POSITION_12")),
                    new RealMFC("Dry1 Position 23", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_POSITION_23")),
                    new RealMFC("Dry1 Pause 1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE_1")),
                    new RealMFC("Dry1 Pause 2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE_2")),
                    new RealMFC("Dry1 Pause 3", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE_3")),
                    new RealMFC("Testing Dry1 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_DRY1_BIT")),
                    new RealMFC("Dry1 Use Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_USE_BIT")),
                    new RealMFC("Dry1 Pause1 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE1_BIT")),
                    new RealMFC("Dry1 Pause2 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE2_BIT")),
                    new RealMFC("Dry1 Pause3 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE3_BIT")),
                    new RealMFC("Dry1 Heating Status", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_HEATING_STATUS")),

                    new RealMFC("Dry2 Times", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_TIMES")),
                    new RealMFC("Dry2 Position 01", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_POSITION_01")),
                    new RealMFC("Dry2 Position 12", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_POSITION_12")),
                    new RealMFC("Dry2 Position 23", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_POSITION_23")),
                    new RealMFC("Dry2 Pause 1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE_1")),
                    new RealMFC("Dry2 Pause 2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE_2")),
                    new RealMFC("Dry2 Pause 3", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE_3")),
                    new RealMFC("Testing Dry2 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_DRY2_BIT")),
                    new RealMFC("Dry2 Use Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_USE_BIT")),
                    new RealMFC("Dry2 Pause1 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE1_BIT")),
                    new RealMFC("Dry2 Pause2 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE2_BIT")),
                    new RealMFC("Dry2 Pause3 Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE3_BIT")),
                    new RealMFC("Dry2 Heating Status", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_HEATING_STATUS")),

                    new RealMFC("Robot Y Pos 1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Y_POS_1")),
                    new RealMFC("Robot Y Pos 2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Y_POS_2")),
                    new RealMFC("Robot Z Pos 1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_1")),
                    new RealMFC("Robot Z Pos 2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_2")),
                    new RealMFC("Robot Z Pos 3", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_3")),
                    new RealMFC("Robot Z Pos 4", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_4")),
                    new RealMFC("Robot Z Pos 5", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_5")),
                    new RealMFC("Testing Robot Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_ROBOT_BIT")),

                    new RealMFC("FS Max", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_FS_MAX")),
                    new RealMFC("FS Min", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_FS_MIN")),
                    new RealMFC("Testing FS Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_FS_BIT")),

                    new RealMFC("Heating Temp SP", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_HEATING_TEMP_SP")),
                    new RealMFC("Testing Heating Bit", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_HEATING_BIT")),

                    new RealMFC("Vision Enable", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_VISION_ENABLE")),

                    new RealMFC("Cap Rubber Enable", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_CAP_RUBBER_ENABLE")),

                    new RealMFC("Cap Non Rubber Enable", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_CAP_NON_RUBBER_ENABLE"))
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
                    new RealMFC("Trig Pos A", 0),
                    new RealMFC("Trig Pos B", 0),
                    new RealMFC("T Moving", 0),
                    new RealMFC("Testing Dosing Bit", 0),
                    new RealMFC("Dry1 Times", 0),
                    new RealMFC("Dry1 Position 01", 0),
                    new RealMFC("Dry1 Position 12", 0),
                    new RealMFC("Dry1 Position 23", 0),
                    new RealMFC("Dry1 Pause 1", 0),
                    new RealMFC("Dry1 Pause 2", 0),
                    new RealMFC("Dry1 Pause 3", 0),
                    new RealMFC("Testing Dry1 Bit", 0),
                    new RealMFC("Dry1 Use Bit", 0),
                    new RealMFC("Dry1 Pause1 Bit", 0),
                    new RealMFC("Dry1 Pause2 Bit", 0),
                    new RealMFC("Dry1 Pause3 Bit", 0),
                    new RealMFC("Dry1 Heating Status", 0),
                    new RealMFC("Dry2 Times", 0),
                    new RealMFC("Dry2 Position 01", 0),
                    new RealMFC("Dry2 Position 12", 0),
                    new RealMFC("Dry2 Position 23", 0),
                    new RealMFC("Dry2 Pause 1", 0),
                    new RealMFC("Dry2 Pause 2", 0),
                    new RealMFC("Dry2 Pause 3", 0),
                    new RealMFC("Testing Dry2 Bit", 0),
                    new RealMFC("Dry2 Use Bit", 0),
                    new RealMFC("Dry2 Pause1 Bit", 0),
                    new RealMFC("Dry2 Pause2 Bit", 0),
                    new RealMFC("Dry2 Pause3 Bit", 0),
                    new RealMFC("Dry2 Heating Status", 0),
                    new RealMFC("Robot Y Pos 1", 0),
                    new RealMFC("Robot Y Pos 2", 0),
                    new RealMFC("Robot Z Pos 1", 0),
                    new RealMFC("Robot Z Pos 2", 0),
                    new RealMFC("Robot Z Pos 3", 0),
                    new RealMFC("Robot Z Pos 4", 0),
                    new RealMFC("Robot Z Pos 5", 0),
                    new RealMFC("Testing Robot Bit", 0),
                    new RealMFC("FS Max", 0),
                    new RealMFC("FS Min", 0),
                    new RealMFC("Testing FS Bit", 0),
                    new RealMFC("Heating Temp SP", 0),
                    new RealMFC("Testing Heating Bit", 0),
                    new RealMFC("Vision Enable", 0),
                    new RealMFC("Cap Rubber Enable", 0),
                    new RealMFC("Cap Non Rubber Enable", 0)
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
                            RealMFCValues[0] = new RealMFC("Spray Valve", tag.TagValue);
                            break;
                        case "S1_PARA_T_PRE_SPRAY":
                            RealMFCValues[1] = new RealMFC("T Pre Spray", tag.TagValue);
                            break;
                        case "S1_PARA_T_SPRAYING":
                            RealMFCValues[2] = new RealMFC("T Spraying", tag.TagValue);
                            break;
                        case "S1_PARA_SPRAY_DELAY":
                            RealMFCValues[3] = new RealMFC("Spray Delay", tag.TagValue);
                            break;
                        case "S1_PARA_NOZZLE_CLEAN":
                            RealMFCValues[4] = new RealMFC("Nozzle Clean", tag.TagValue);
                            break;
                        case "S1_PARA_WAIT_TO_CLEAN_TIME":
                            RealMFCValues[5] = new RealMFC("Wait To Clean Time", tag.TagValue);
                            break;
                        case "S1_PARA_CLEAN_DURATION_TIME":
                            RealMFCValues[6] = new RealMFC("Clean Duration Time", tag.TagValue);
                            break;
                        case "S1_PARA_SPRAY_FILL_WAY_INSERTED":
                            RealMFCValues[7] = new RealMFC("Spray Fill Way Inserted", tag.TagValue);
                            break;
                        case "S1_PARA_TRIG_POS_A":
                            RealMFCValues[8] = new RealMFC("Trig Pos A", tag.TagValue);
                            break;
                        case "S1_PARA_TRIG_POS_B":
                            RealMFCValues[9] = new RealMFC("Trig Pos B", tag.TagValue);
                            break;
                        case "S1_PARA_T_MOVING":
                            RealMFCValues[10] = new RealMFC("T Moving", tag.TagValue);
                            break;
                        case "S1_PARA_TESTING_DOSING_BIT":
                            RealMFCValues[11] = new RealMFC("Testing Dosing Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_TIMES":
                            RealMFCValues[12] = new RealMFC("Dry1 Times", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_POSITION_01":
                            RealMFCValues[13] = new RealMFC("Dry1 Position 01", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_POSITION_12":
                            RealMFCValues[14] = new RealMFC("Dry1 Position 12", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_POSITION_23":
                            RealMFCValues[15] = new RealMFC("Dry1 Position 23", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_PAUSE_1":
                            RealMFCValues[16] = new RealMFC("Dry1 Pause 1", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_PAUSE_2":
                            RealMFCValues[17] = new RealMFC("Dry1 Pause 2", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_PAUSE_3":
                            RealMFCValues[18] = new RealMFC("Dry1 Pause 3", tag.TagValue);
                            break;
                        case "S1_PARA_TESTING_DRY1_BIT":
                            RealMFCValues[19] = new RealMFC("Testing Dry1 Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_USE_BIT":
                            RealMFCValues[20] = new RealMFC("Dry1 Use Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_PAUSE1_BIT":
                            RealMFCValues[21] = new RealMFC("Dry1 Pause1 Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_PAUSE2_BIT":
                            RealMFCValues[22] = new RealMFC("Dry1 Pause2 Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY1_PAUSE3_BIT":
                            RealMFCValues[23] = new RealMFC("Dry1 Pause3 Bit", tag.TagValue);
                            break;

                        case "S1_PARA_DRY2_TIMES":
                            RealMFCValues[25] = new RealMFC("Dry2 Times", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_POSITION_01":
                            RealMFCValues[26] = new RealMFC("Dry2 Position 01", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_POSITION_12":
                            RealMFCValues[27] = new RealMFC("Dry2 Position 12", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_POSITION_23":
                            RealMFCValues[28] = new RealMFC("Dry2 Position 23", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_PAUSE_1":
                            RealMFCValues[29] = new RealMFC("Dry2 Pause 1", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_PAUSE_2":
                            RealMFCValues[30] = new RealMFC("Dry2 Pause 2", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_PAUSE_3":
                            RealMFCValues[31] = new RealMFC("Dry2 Pause 3", tag.TagValue);
                            break;
                        case "S1_PARA_TESTING_DRY2_BIT":
                            RealMFCValues[32] = new RealMFC("Testing Dry2 Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_USE_BIT":
                            RealMFCValues[33] = new RealMFC("Dry2 Use Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_PAUSE1_BIT":
                            RealMFCValues[34] = new RealMFC("Dry2 Pause1 Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_PAUSE2_BIT":
                            RealMFCValues[35] = new RealMFC("Dry2 Pause2 Bit", tag.TagValue);
                            break;
                        case "S1_PARA_DRY2_PAUSE3_BIT":
                            RealMFCValues[36] = new RealMFC("Dry2 Pause3 Bit", tag.TagValue);
                            break;
                        case "S1_PARA_HEATING_STATUS":
                            RealMFCValues[24] = new RealMFC("Dry1 Heating Status", tag.TagValue);
                            RealMFCValues[37] = new RealMFC("Dry2 Heating Status", tag.TagValue);

                            break;
                        case "S1_PARA_ROBOT_Y_POS_1":
                            RealMFCValues[38] = new RealMFC("Robot Y Pos 1", tag.TagValue);
                            break;
                        case "S1_PARA_ROBOT_Y_POS_2":
                            RealMFCValues[39] = new RealMFC("Robot Y Pos 2", tag.TagValue);
                            break;
                        case "S1_PARA_ROBOT_Z_POS_1":
                            RealMFCValues[40] = new RealMFC("Robot Z Pos 1", tag.TagValue);
                            break;
                        case "S1_PARA_ROBOT_Z_POS_2":
                            RealMFCValues[41] = new RealMFC("Robot Z Pos 2", tag.TagValue);
                            break;
                        case "S1_PARA_ROBOT_Z_POS_3":
                            RealMFCValues[42] = new RealMFC("Robot Z Pos 3", tag.TagValue);
                            break;
                        case "S1_PARA_ROBOT_Z_POS_4":
                            RealMFCValues[43] = new RealMFC("Robot Z Pos 4", tag.TagValue);
                            break;
                        case "S1_PARA_ROBOT_Z_POS_5":
                            RealMFCValues[44] = new RealMFC("Robot Z Pos 5", tag.TagValue);
                            break;
                        case "S1_PARA_TESTING_ROBOT_BIT":
                            RealMFCValues[45] = new RealMFC("Testing Robot Bit", tag.TagValue);
                            break;
                        case "S1_PARA_FS_MAX":
                            RealMFCValues[46] = new RealMFC("FS Max", tag.TagValue);
                            break;
                        case "S1_PARA_FS_MIN":
                            RealMFCValues[47] = new RealMFC("FS Min", tag.TagValue);
                            break;
                        case "S1_PARA_TESTING_FS_BIT":
                            RealMFCValues[48] = new RealMFC("Testing FS Bit", tag.TagValue);
                            break;
                        case "S1_PARA_HEATING_TEMP_SP":
                            RealMFCValues[49] = new RealMFC("Heating Temp SP", tag.TagValue);
                            break;
                        case "S1_PARA_TESTING_HEATING_BIT":
                            RealMFCValues[50] = new RealMFC("Testing Heating Bit", tag.TagValue);
                            break;
                        case "S1_VISION_ENABLE":
                            RealMFCValues[51] = new RealMFC("Vision Enable", tag.TagValue);
                            break;
                        case "S1_CAP_RUBBER_ENABLE":
                            RealMFCValues[52] = new RealMFC("Cap Rubber Enable", tag.TagValue);
                            break;
                        case "S1_CAP_NON_RUBBER_ENABLE":
                            RealMFCValues[53] = new RealMFC("Cap Non Rubber Enable", tag.TagValue);
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
            var newViewModels = MFCDtos.Select((tag, index) => new ComparedMFC(tag.MFCName, tag.Value, tag.MinValue, tag.MaxValue, Convert.ToDouble(RealMFCValues.First(i => i.Name == tag.MFCName).RealValue))).ToList();
            MFCEntries = new(newViewModels);
            DosingEntries = new(MFCEntries.Take(12));
            Drying1Entries = new(MFCEntries.Skip(12).Take(13));
            Drying2Entries = new(MFCEntries.Skip(25).Take(13));
            RobotArmEntries = new(MFCEntries.Skip(38).Take(8));
            OthersEntries = new(MFCEntries.Skip(46).Take(8));
        }
    }
}
