using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Employees;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ErrorInformations;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Lines;
using WEMBLEY.DemoApp.Core.Domain.Dtos.MachineStatus;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Products;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Stations;
using WEMBLEY.DemoApp.Core.Domain.Models;

namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface IApiService
    {
        Task<IEnumerable<LineDto>> GetAllineAsync();
        Task<IEnumerable<StationDto>> GetAllStationAsync();
        Task<IEnumerable<ProductDto>> GetProductsByLineAsync(string lineId);
        Task<IEnumerable<ReferenceDto>> GetReferencesByLineAsync(string lineId);
        Task<IEnumerable<ReferenceDto>> GetAllReferenceseAsync();
        Task CreateLotAsync(string refName, CreateLotDto createDto);
        Task UpdateLotAsync(string refName, CreateLotDto createDto);
        Task CompleteRefAsync(string refName);


        Task<IEnumerable<DeviceReferenceDto>> GetStationReferencesMFCAsync(string stationId, string referenceId);
        Task<IEnumerable<StationReferenceInfoDto>> GetStationReferencesInfoAsync();

        Task FixMFCAsync(string referenceId, string deviceId, IEnumerable<MFCDto> fixDto);

        Task<IEnumerable<ParameterDto>> GetAllLotDeviceReferenceAsync();
        Task<IEnumerable<ParameterDto>> GetLotDeviceReferenceByDeviceAsync(string lineId);
        Task<ParameterDto> GetLotDeviceReferenceAsync(string referenceId);

        Task<IEnumerable<EmployeeDto>> GetAllPersonAsync();
        Task CreatePersonAsync(EmployeeWorkingDto createDto);
        Task DeletePersonAsync(string personId);

        Task AddPersonToDeviceAsync(string stationId, AddPersonToDeviceDto createDto);
        Task UpdatePersonToDeviceAsync(string stationId, AddPersonToDeviceDto fixDto);
        Task DeletePersonToDeviceAsync(string stationId, AddPersonToDeviceDto createDto);

        Task<IEnumerable<ErrorStatusDto>> GetErrorsHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<MachineStatusDto>> GetStatusHistoryAsync(string deviceId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<ShiftReportDto>> GetShiftReportHistoryAsync(string stationId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<DataPoint>> GetLastestOEEAsync(string stationId, int interval);
        Task<IEnumerable<ShiftReportWithShotDto>> GetShortenShiftReportWithShotByShiftIdAsync(string shiftReportId, int interval);
        Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByShiftIdAsync(int shiftReportId, int pageIndex, int pageSize);
        Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByDateAsync(DateTime Date, int shiftNumber);
        Task<byte[]> DownloadShiftReportFileAsync(string deviceId, DateTime startDate, DateTime endDate);
    }
}
