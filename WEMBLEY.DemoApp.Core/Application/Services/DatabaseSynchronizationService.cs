using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.Store;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class DatabaseSynchronizationService : IDatabaseSynchronizationService
    {
        private readonly IApiService _apiService;
        private readonly ReferenceStore _referenceStore;
        private readonly DeviceStore _deviceStore;

        public DatabaseSynchronizationService(IApiService apiService, ReferenceStore referenceStore, DeviceStore deviceStore)
        {
            _apiService = apiService;
            _referenceStore = referenceStore;
            _deviceStore = deviceStore;
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
    }
}
