using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Devices;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ErrorInformations;
using WEMBLEY.DemoApp.Core.Domain.Dtos.MachineStatus;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Persons;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Products;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports;
using WEMBLEY.DemoApp.Core.Domain.Models;

namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface IApiService
    {
        Task<IEnumerable<DeviceDto>> GetAllDeviceTypeAsync();
        Task<IEnumerable<ProductDto>> GetProductsByDeviceTypeAsync(string deviceType);
        Task<IEnumerable<ReferenceDto>> GetReferencesByDeviceTypeAsync(string deviceType);
        Task<IEnumerable<ReferenceDto>> GetAllReferenceseAsync();
        Task CreateLotAsync(string refName, CreateLotDto createDto);
        Task UpdateLotAsync(string refName, CreateLotDto createDto);
        Task CompleteRefAsync(string refName);


        Task<IEnumerable<DeviceReferenceDto>> GetDeviceReferenceMFCAsync(int refId, string deviceId);
        Task FixMFCAsync(int refId, string deviceId, IEnumerable<MFCDto> fixDto);

        Task<IEnumerable<ParameterDto>> GetAllLotDeviceReferenceAsync();
        Task<IEnumerable<ParameterDto>> GetLotDeviceReferenceByDeviceTypeAsync(string deviceType);
        Task<ParameterDto> GetLotDeviceReferenceAsync(int refId);

        Task<IEnumerable<PersonDto>> GetAllPersonAsync();
        Task CreatePersonAsync(PersonWorkingDto createDto);
        Task DeletePersonAsync(string personId);

        Task AddPersonToDeviceAsync(string deviceId, AddPersonToDeviceDto createDto);
        Task UpdatePersonToDeviceAsync(string deviceId, AddPersonToDeviceDto fixDto);
        Task DeletePersonToDeviceAsync(string deviceId, AddPersonToDeviceDto createDto);

        Task<IEnumerable<ErrorStatusDto>> GetErrorsHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<MachineStatusDto>> GetStatusHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ShiftReportDto>> GetShiftReportHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<DataPoint>> GetLastestOEEAsync(string deviceId);
        Task<IEnumerable<ShiftReportWithShotDto>> GetShortenShiftReportWithShotByShiftIdAsync(int shiftReportId, int interval);
        Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByShiftIdAsync(int shiftReportId, int pageIndex, int pageSize);
        Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByDateAsync(DateTime Date, int shiftNumber);
        Task<byte[]> DownloadShiftReportFileAsync(string deviceId, DateTime startDate, DateTime endDate);
    }
}
