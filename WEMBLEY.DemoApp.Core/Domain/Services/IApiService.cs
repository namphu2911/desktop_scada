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

        Task<IEnumerable<DeviceReferenceDto>> GetDeviceReferenceMFCAsync(int refId, string deviceId);
        Task FixMFCAsync(int refId, string deviceId, IEnumerable<MFCDto> fixDto);

        Task<IEnumerable<LotDeviceReferenceDto>> GetAllLotDeviceReferenceAsync();
        Task<IEnumerable<LotDeviceReferenceDto>> GetLotDeviceReferenceByDeviceTypeAsync(string deviceType);
        Task<LotDeviceReferenceDto> GetLotDeviceReferenceAsync(int refId);


        Task CreateLot(string refName, CreateLotDto createDto);

        Task<IEnumerable<ErrorStatusDto>> GetErrorsHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<MachineStatusDto>> GetStatusHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ShiftReportDto>> GetShiftReportHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByShiftIdAsync(int ShiftReportId);
        Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByDateAsync(DateTime Date, int shiftNumber);
        Task<byte[]> DownloadShiftReportFileAsync(string deviceId, DateTime startDate, DateTime endDate);
    }
}
