using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Notifications.Wpf.Core;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
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
        public List<RealMFC> RealMFCValues { get; set; } = new(54);
        public List<TagChangedNotification> AllTags { get; set; } = new();
        public List<bool> IsMFCAlarm { get; set; } = new();
        public ICommand LoadMFCMonitorViewCommand { get; set; }
        public string HomeRefId => _homeDataStore.LineReferences.First(i => i.LineId == "NonVacuumBloodTube").ReferenceId;

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
                    new RealMFC("S1_PARA_SPRAY_VALVE", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_SPRAY_VALVE")),
                    new RealMFC("S1_PARA_T_PRE_SPRAY", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_T_PRE_SPRAY")),
                    new RealMFC("S1_PARA_T_SPRAYING", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_T_SPRAYING")),
                    new RealMFC("S1_PARA_SPRAY_DELAY", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_SPRAY_DELAY")),
                    new RealMFC("S1_PARA_NOZZLE_CLEAN", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_NOZZLE_CLEAN")),
                    new RealMFC("S1_PARA_WAIT_TO_CLEAN_TIME", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_WAIT_TO_CLEAN_TIME")),
                    new RealMFC("S1_PARA_CLEAN_DURATION_TIME", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_CLEAN_DURATION_TIME")),
                    new RealMFC("S1_PARA_SPRAY_FILL_WAY_INSERTED", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_SPRAY_FILL_WAY_INSERTED")),
                    new RealMFC("S1_PARA_TRIG_POS_A", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TRIG_POS_A")),
                    new RealMFC("S1_PARA_TRIG_POS_B", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TRIG_POS_B")),
                    new RealMFC("S1_PARA_T_MOVING", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_T_MOVING")),
                    new RealMFC("S1_PARA_TESTING_DOSING_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_DOSING_BIT")),
                    new RealMFC("S1_PARA_DRY1_TIMES", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_TIMES")),
                    new RealMFC("S1_PARA_DRY1_POSITION_01", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_POSITION_01")),
                    new RealMFC("S1_PARA_DRY1_POSITION_12", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_POSITION_12")),
                    new RealMFC("S1_PARA_DRY1_POSITION_23", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_POSITION_23")),
                    new RealMFC("S1_PARA_DRY1_PAUSE_1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE_1")),
                    new RealMFC("S1_PARA_DRY1_PAUSE_2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE_2")),
                    new RealMFC("S1_PARA_DRY1_PAUSE_3", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE_3")),
                    new RealMFC("S1_PARA_TESTING_DRY1_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_DRY1_BIT")),
                    new RealMFC("S1_PARA_DRY1_USE_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_USE_BIT")),
                    new RealMFC("S1_PARA_DRY1_PAUSE1_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE1_BIT")),
                    new RealMFC("S1_PARA_DRY1_PAUSE2_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE2_BIT")),
                    new RealMFC("S1_PARA_DRY1_PAUSE3_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY1_PAUSE3_BIT")),
                    new RealMFC("S1_PARA_HEATING_STATUS", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_HEATING_STATUS")),
                    new RealMFC("S1_PARA_DRY2_TIMES", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_TIMES")),
                    new RealMFC("S1_PARA_DRY2_POSITION_01", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_POSITION_01")),
                    new RealMFC("S1_PARA_DRY2_POSITION_12", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_POSITION_12")),
                    new RealMFC("S1_PARA_DRY2_POSITION_23", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_POSITION_23")),
                    new RealMFC("S1_PARA_DRY2_PAUSE_1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE_1")),
                    new RealMFC("S1_PARA_DRY2_PAUSE_2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE_2")),
                    new RealMFC("S1_PARA_DRY2_PAUSE_3", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE_3")),
                    new RealMFC("S1_PARA_TESTING_DRY2_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_DRY2_BIT")),
                    new RealMFC("S1_PARA_DRY2_USE_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_USE_BIT")),
                    new RealMFC("S1_PARA_DRY2_PAUSE1_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE1_BIT")),
                    new RealMFC("S1_PARA_DRY2_PAUSE2_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE2_BIT")),
                    new RealMFC("S1_PARA_DRY2_PAUSE3_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_DRY2_PAUSE3_BIT")),
                    new RealMFC("S1_PARA_HEATING_STATUS", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_HEATING_STATUS")),
                    new RealMFC("S1_PARA_ROBOT_Y_POS_1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Y_POS_1")),
                    new RealMFC("S1_PARA_ROBOT_Y_POS_2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Y_POS_2")),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_1", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_1")),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_2", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_2")),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_3", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_3")),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_4", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_4")),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_5", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_ROBOT_Z_POS_5")),
                    new RealMFC("S1_PARA_TESTING_ROBOT_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_ROBOT_BIT")),
                    new RealMFC("S1_PARA_FS_MAX", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_FS_MAX")),
                    new RealMFC("S1_PARA_FS_MIN", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_FS_MIN")),
                    new RealMFC("S1_PARA_TESTING_FS_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_FS_BIT")),
                    new RealMFC("S1_PARA_HEATING_TEMP_SP", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_HEATING_TEMP_SP")),
                    new RealMFC("S1_PARA_TESTING_HEATING_BIT", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_PARA_TESTING_HEATING_BIT")),
                    new RealMFC("S1_VISION_ENABLE", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_VISION_ENABLE")),
                    new RealMFC("S1_CAP_RUBBER_ENABLE", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_CAP_RUBBER_ENABLE")),
                    new RealMFC("S1_CAP_NON_RUBBER_ENABLE", await _signalRClient.GetBufferValue("IE-F3-BLO06", "S1_CAP_NON_RUBBER_ENABLE"))
                    };
                OnPropertyChanged(nameof(RealMFCValues));
            }
            else
            {
                RealMFCValues = new()
                {
                    new RealMFC("S1_PARA_SPRAY_VALVE", 0),
                    new RealMFC("S1_PARA_T_PRE_SPRAY", 0),
                    new RealMFC("S1_PARA_T_SPRAYING", 0),
                    new RealMFC("S1_PARA_SPRAY_DELAY", 0),
                    new RealMFC("S1_PARA_NOZZLE_CLEAN", 0),
                    new RealMFC("S1_PARA_WAIT_TO_CLEAN_TIME", 0),
                    new RealMFC("S1_PARA_CLEAN_DURATION_TIME", 0),
                    new RealMFC("S1_PARA_SPRAY_FILL_WAY_INSERTED", 0),
                    new RealMFC("S1_PARA_TRIG_POS_A", 0),
                    new RealMFC("S1_PARA_TRIG_POS_B", 0),
                    new RealMFC("S1_PARA_T_MOVING", 0),
                    new RealMFC("S1_PARA_TESTING_DOSING_BIT", 0),
                    new RealMFC("S1_PARA_DRY1_TIMES", 0),
                    new RealMFC("S1_PARA_DRY1_POSITION_01", 0),
                    new RealMFC("S1_PARA_DRY1_POSITION_12", 0),
                    new RealMFC("S1_PARA_DRY1_POSITION_23", 0),
                    new RealMFC("S1_PARA_DRY1_PAUSE_1", 0),
                    new RealMFC("S1_PARA_DRY1_PAUSE_2", 0),
                    new RealMFC("S1_PARA_DRY1_PAUSE_3", 0),
                    new RealMFC("S1_PARA_TESTING_DRY1_BIT", 0),
                    new RealMFC("S1_PARA_DRY1_USE_BIT", 0),
                    new RealMFC("S1_PARA_DRY1_PAUSE1_BIT", 0),
                    new RealMFC("S1_PARA_DRY1_PAUSE2_BIT", 0),
                    new RealMFC("S1_PARA_DRY1_PAUSE3_BIT", 0),
                    new RealMFC("S1_PARA_HEATING_STATUS", 0),
                    new RealMFC("S1_PARA_DRY2_TIMES", 0),
                    new RealMFC("S1_PARA_DRY2_POSITION_01", 0),
                    new RealMFC("S1_PARA_DRY2_POSITION_12", 0),
                    new RealMFC("S1_PARA_DRY2_POSITION_23", 0),
                    new RealMFC("S1_PARA_DRY2_PAUSE_1", 0),
                    new RealMFC("S1_PARA_DRY2_PAUSE_2", 0),
                    new RealMFC("S1_PARA_DRY2_PAUSE_3", 0),
                    new RealMFC("S1_PARA_TESTING_DRY2_BIT", 0),
                    new RealMFC("S1_PARA_DRY2_USE_BIT", 0),
                    new RealMFC("S1_PARA_DRY2_PAUSE1_BIT", 0),
                    new RealMFC("S1_PARA_DRY2_PAUSE2_BIT", 0),
                    new RealMFC("S1_PARA_DRY2_PAUSE3_BIT", 0),
                    new RealMFC("S1_PARA_HEATING_STATUS", 0),
                    new RealMFC("S1_PARA_ROBOT_Y_POS_1", 0),
                    new RealMFC("S1_PARA_ROBOT_Y_POS_2", 0),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_1", 0),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_2", 0),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_3", 0),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_4", 0),
                    new RealMFC("S1_PARA_ROBOT_Z_POS_5", 0),
                    new RealMFC("S1_PARA_TESTING_ROBOT_BIT", 0),
                    new RealMFC("S1_PARA_FS_MAX", 0),
                    new RealMFC("S1_PARA_FS_MIN", 0),
                    new RealMFC("S1_PARA_TESTING_FS_BIT", 0),
                    new RealMFC("S1_PARA_HEATING_TEMP_SP", 0),
                    new RealMFC("S1_PARA_TESTING_HEATING_BIT", 0),
                    new RealMFC("S1_VISION_ENABLE", 0),
                    new RealMFC("S1_CAP_RUBBER_ENABLE", 0),
                    new RealMFC("S1_CAP_NON_RUBBER_ENABLE", 0)
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
                if (tag.StationId == "IE-F3-BLO06")
                {
                    switch (tag.TagId)
                    {
                        case "S1_PARA_SPRAY_VALVE":
                            RealMFCValues[0] = new RealMFC("S1_PARA_SPRAY_VALVE", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_T_PRE_SPRAY":
                            RealMFCValues[1] = new RealMFC("S1_PARA_T_PRE_SPRAY", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_T_SPRAYING":
                            RealMFCValues[2] = new RealMFC("S1_PARA_T_SPRAYING", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_SPRAY_DELAY":
                            RealMFCValues[3] = new RealMFC("S1_PARA_SPRAY_DELAY", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_NOZZLE_CLEAN":
                            RealMFCValues[4] = new RealMFC("S1_PARA_NOZZLE_CLEAN", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_WAIT_TO_CLEAN_TIME":
                            RealMFCValues[5] = new RealMFC("S1_PARA_WAIT_TO_CLEAN_TIME", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_CLEAN_DURATION_TIME":
                            RealMFCValues[6] = new RealMFC("S1_PARA_CLEAN_DURATION_TIME", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_SPRAY_FILL_WAY_INSERTED":
                            RealMFCValues[7] = new RealMFC("S1_PARA_SPRAY_FILL_WAY_INSERTED", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_TRIG_POS_A":
                            RealMFCValues[8] = new RealMFC("S1_PARA_TRIG_POS_A", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_TRIG_POS_B":
                            RealMFCValues[9] = new RealMFC("S1_PARA_TRIG_POS_B", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_T_MOVING":
                            RealMFCValues[10] = new RealMFC("S1_PARA_T_MOVING", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_TESTING_DOSING_BIT":
                            RealMFCValues[11] = new RealMFC("S1_PARA_TESTING_DOSING_BIT", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_TIMES":
                            RealMFCValues[12] = new RealMFC("S1_PARA_DRY1_TIMES", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_POSITION_01":
                            RealMFCValues[13] = new RealMFC("S1_PARA_DRY1_POSITION_01", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_POSITION_12":
                            RealMFCValues[14] = new RealMFC("S1_PARA_DRY1_POSITION_12", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_POSITION_23":
                            RealMFCValues[15] = new RealMFC("S1_PARA_DRY1_POSITION_23", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_PAUSE_1":
                            RealMFCValues[16] = new RealMFC("S1_PARA_DRY1_PAUSE_1", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_PAUSE_2":
                            RealMFCValues[17] = new RealMFC("S1_PARA_DRY1_PAUSE_2", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_PAUSE_3":
                            RealMFCValues[18] = new RealMFC("S1_PARA_DRY1_PAUSE_3", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_TESTING_DRY1_BIT":
                            RealMFCValues[19] = new RealMFC("S1_PARA_TESTING_DRY1_BIT", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_USE_BIT":  // Assuming index 20
                            RealMFCValues[20] = new RealMFC("S1_PARA_DRY1_USE_BIT", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_PAUSE1_BIT":  // Assuming index 21
                            RealMFCValues[21] = new RealMFC("S1_PARA_DRY1_PAUSE1_BIT", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_PAUSE2_BIT":  // Assuming index 22
                            RealMFCValues[22] = new RealMFC("S1_PARA_DRY1_PAUSE2_BIT", tag.TagValue);
                            ReloadData();
                            break;
                        case "S1_PARA_DRY1_PAUSE3_BIT":  // Assuming index 23
                            RealMFCValues[23] = new RealMFC("S1_PARA_DRY1_PAUSE3_BIT", tag.TagValue);
                            ReloadData();
                            break;

                        case "S1_PARA_DRY2_TIMES":
                            RealMFCValues[25] = new RealMFC("S1_PARA_DRY2_TIMES", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_POSITION_01":
                            RealMFCValues[26] = new RealMFC("S1_PARA_DRY2_POSITION_01", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_POSITION_12":
                            RealMFCValues[27] = new RealMFC("S1_PARA_DRY2_POSITION_12", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_POSITION_23":
                            RealMFCValues[28] = new RealMFC("S1_PARA_DRY2_POSITION_23", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_PAUSE_1":
                            RealMFCValues[29] = new RealMFC("S1_PARA_DRY2_PAUSE_1", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_PAUSE_2":
                            RealMFCValues[30] = new RealMFC("S1_PARA_DRY2_PAUSE_2", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_PAUSE_3":
                            RealMFCValues[31] = new RealMFC("S1_PARA_DRY2_PAUSE_3", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_TESTING_DRY2_BIT":
                            RealMFCValues[32] = new RealMFC("S1_PARA_TESTING_DRY2_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_USE_BIT":
                            RealMFCValues[33] = new RealMFC("S1_PARA_DRY2_USE_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_PAUSE1_BIT":
                            RealMFCValues[34] = new RealMFC("S1_PARA_DRY2_PAUSE1_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_PAUSE2_BIT":
                            RealMFCValues[35] = new RealMFC("S1_PARA_DRY2_PAUSE2_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_DRY2_PAUSE3_BIT":
                            RealMFCValues[36] = new RealMFC("S1_PARA_DRY2_PAUSE3_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_HEATING_STATUS":
                            RealMFCValues[24] = new RealMFC("S1_PARA_HEATING_STATUS", tag.TagValue);
                            RealMFCValues[37] = new RealMFC("S1_PARA_HEATING_STATUS", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_ROBOT_Y_POS_1":
                            RealMFCValues[38] = new RealMFC("S1_PARA_ROBOT_Y_POS_1", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_ROBOT_Y_POS_2":
                            RealMFCValues[39] = new RealMFC("S1_PARA_ROBOT_Y_POS_2", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_ROBOT_Z_POS_1":
                            RealMFCValues[40] = new RealMFC("S1_PARA_ROBOT_Z_POS_1", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_ROBOT_Z_POS_2":
                            RealMFCValues[41] = new RealMFC("S1_PARA_ROBOT_Z_POS_2", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_ROBOT_Z_POS_3":
                            RealMFCValues[42] = new RealMFC("S1_PARA_ROBOT_Z_POS_3", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_ROBOT_Z_POS_4":
                            RealMFCValues[43] = new RealMFC("S1_PARA_ROBOT_Z_POS_4", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_ROBOT_Z_POS_5":
                            RealMFCValues[44] = new RealMFC("S1_PARA_ROBOT_Z_POS_5", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_TESTING_ROBOT_BIT":
                            RealMFCValues[45] = new RealMFC("S1_PARA_TESTING_ROBOT_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_FS_MAX":
                            RealMFCValues[46] = new RealMFC("S1_PARA_FS_MAX", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_FS_MIN":
                            RealMFCValues[47] = new RealMFC("S1_PARA_FS_MIN", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_TESTING_FS_BIT":
                            RealMFCValues[48] = new RealMFC("S1_PARA_TESTING_FS_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_HEATING_TEMP_SP":
                            RealMFCValues[49] = new RealMFC("S1_PARA_HEATING_TEMP_SP", tag.TagValue);
                            ReloadData(); break;
                        case "S1_PARA_TESTING_HEATING_BIT":
                            RealMFCValues[50] = new RealMFC("S1_PARA_TESTING_HEATING_BIT", tag.TagValue);
                            ReloadData(); break;
                        case "S1_VISION_ENABLE":
                            RealMFCValues[51] = new RealMFC("S1_VISION_ENABLE", tag.TagValue);
                            ReloadData(); break;
                        case "S1_CAP_RUBBER_ENABLE":
                            RealMFCValues[52] = new RealMFC("S1_CAP_RUBBER_ENABLE", tag.TagValue);
                            ReloadData(); break;
                        case "S1_CAP_NON_RUBBER_ENABLE":
                            RealMFCValues[53] = new RealMFC("S1_CAP_NON_RUBBER_ENABLE", tag.TagValue);
                            ReloadData(); break;
                        default: break;
                    }
                }
            }
            OnPropertyChanged(nameof(RealMFCValues));
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
            MFCErrorNotification();
        }

        private async void MFCErrorNotification()
        {
            IsMFCAlarm = MFCEntries.Select(i => i.IsAlarmed).ToList();
            var notificationManager = new NotificationManager();
            var notificationContent = new NotificationContent
            {
                Title = "Cảnh báo",
                Message = "Có lỗi MFC!",
                Type = NotificationType.Error
            };

            try
            {
                if (IsMFCAlarm.Contains(true))
                {
                    await notificationManager.ShowAsync(notificationContent, areaName: "WindowArea", expirationTime: TimeSpan.MaxValue);
                }
                else await notificationManager.CloseAllAsync();
            }
            catch { }
        }
    }
}
