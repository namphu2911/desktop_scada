using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Domain.Dtos;

namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface IApiService
    {
        Task<IEnumerable<DeviceDto>> GetAllDeviceTypeAsync();
        Task<IEnumerable<ProductDto>> GetProductsByDeviceTypeAsync(string deviceType);
        Task<IEnumerable<ReferenceDto>> GetReferencesByDeviceTypeAsync(string deviceType);
        Task<IEnumerable<ReferenceDto>> GetAllReferenceseAsync();

        Task<IEnumerable<LotDeviceReferenceDto>> GetAllLotDeviceReferenceAsync();
        Task<IEnumerable<LotDeviceReferenceDto>> GetLotDeviceReferenceByDeviceTypeAsync(string deviceType);
        Task<LotDeviceReferenceDto> GetLotDeviceReferenceAsync(int refId);
        Task CreateLot(string refName, CreateLotDto createDto);


        Task<IEnumerable<ErrorStatusDto>> GetErrorsHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<MachineStatusDto>> GetStatusHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);

    }
}
