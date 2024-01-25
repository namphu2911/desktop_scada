using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class DatabaseSynchronizationService : IDatabaseSynchronizationService
    {
        private readonly IApiService _apiService;
        private readonly ReferenceStore _referenceStore;
        private readonly DeviceStore _deviceStore;
        private readonly PersonStore _personStore;
        private readonly HomeDataStore _homeDataStore;

        List<string> Errors { get; set; } = new();

        public DatabaseSynchronizationService(IApiService apiService, ReferenceStore referenceStore, DeviceStore deviceStore, HomeDataStore homeDataStore, PersonStore personStore)
        {
            _apiService = apiService;
            _referenceStore = referenceStore;
            _deviceStore = deviceStore;
            _homeDataStore = homeDataStore;
            _personStore = personStore;
        }

        public async Task SynchronizeReferencesData()
        {
            var referenceDto = await _apiService.GetAllReferenceseAsync();
            _referenceStore.SetReference(referenceDto);
        }

        public async Task SynchronizeDevicesData()
        {
            var deviceDtos = await _apiService.GetAllDeviceTypeAsync();
            _deviceStore.SetDevice(deviceDtos);
        }

        public async Task SynchronizePersonsData()
        {
            var personDtos = await _apiService.GetAllPersonAsync();
            _personStore.SetPerson(personDtos);
        }

        public async Task SynchronizeHomeData()
        {
            var homeRefDtos = await _apiService.GetAllLotDeviceReferenceAsync();
            _homeDataStore.SetHomeRef(homeRefDtos);
        }
    }
}
