using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class DatabaseSynchronizationService : IDatabaseSynchronizationService
    {
        private readonly IApiService _apiService;
        private readonly ReferenceStore _referenceStore;
        private readonly StationStore _deviceStore;
        private readonly EmployeeStore _employeeStore;
        private readonly HomeDataStore _homeDataStore;

        List<string> Errors { get; set; } = new();

        public DatabaseSynchronizationService(IApiService apiService, ReferenceStore referenceStore, StationStore deviceStore, HomeDataStore homeDataStore, EmployeeStore employeeStore)
        {
            _apiService = apiService;
            _referenceStore = referenceStore;
            _deviceStore = deviceStore;
            _homeDataStore = homeDataStore;
            _employeeStore = employeeStore;
        }

        public async Task SynchronizeReferencesData()
        {
            var referenceDto = await _apiService.GetAllReferenceseAsync();
            _referenceStore.SetReference(referenceDto);
        }

        public async Task SynchronizeStationsData()
        {
            var deviceDtos = await _apiService.GetStationReferencesInfoAsync();
            _deviceStore.SetStation(deviceDtos);
        }

        public async Task SynchronizeEmployeesData()
        {
            var employeeDtos = await _apiService.GetAllPersonAsync();
            _employeeStore.SetEmployee(employeeDtos);
        }

        public async Task SynchronizeHomeData()
        {
            var homeRefDtos = await _apiService.GetAllLotDeviceReferenceAsync();
            _homeDataStore.SetHomeRef(homeRefDtos);
        }
    }
}
