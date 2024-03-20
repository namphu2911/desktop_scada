using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using WEMBLEY.DemoApp.Core.Domain.Dtos;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Employees;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ErrorInformations;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Lines;
using WEMBLEY.DemoApp.Core.Domain.Dtos.MachineStatus;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Products;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Stations;
using WEMBLEY.DemoApp.Core.Domain.Exceptions;
using WEMBLEY.DemoApp.Core.Domain.Models;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        private const string serverUrl = "https://wembleyscada.azurewebsites.net";
        //private const string serverUrl2 = "https://wembleyscadacloud.azurewebsites.net";

        //http://10.0.70.45:81
        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<LineDto>> GetAllineAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Line");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<LineDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<IEnumerable<StationDto>> GetAllStationAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Stations");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<StationDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByLineAsync(string lineId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Products?LineId={lineId}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<IEnumerable<ReferenceDto>> GetReferencesByLineAsync(string lineId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References?LineId={lineId}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ReferenceDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<IEnumerable<ReferenceDto>> GetAllReferenceseAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ReferenceDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        //Tao Lo
        public async Task CreateLotAsync(string refName, CreateLotDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/References/{refName}", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        //Update LO
        public async Task UpdateLotAsync(string refName, CreateLotDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/References/{refName}", content);
            response.EnsureSuccessStatusCode();
        }

        //Ket thuc Lo
        public async Task CompleteRefAsync(string refName)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/References/Parameters/Completed/{refName}", null);
            response.EnsureSuccessStatusCode();
        }

        //Get MFC theo station va Ref
        public async Task<IEnumerable<DeviceReferenceDto>> GetStationReferencesMFCAsync(string stationId, string referenceId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/StationReferences?StationId={stationId}&ReferenceId={referenceId}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<DeviceReferenceDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<IEnumerable<StationReferenceInfoDto>> GetStationReferencesInfoAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/StationReferences/Store");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<StationReferenceInfoDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        //UpdateMFC
        public async Task FixMFCAsync(string referenceId, string stationId, IEnumerable<MFCDto> fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/StationReferences/{stationId}/{referenceId}", content);
            response.EnsureSuccessStatusCode();
        }

        //Lay tat ca Paramter hien tai
        public async Task<IEnumerable<ParameterDto>> GetAllLotDeviceReferenceAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References/Parameters");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ParameterDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        //Lay Paramter theo Line
        public async Task<IEnumerable<ParameterDto>> GetLotDeviceReferenceByDeviceAsync(string lineId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References/Parameters?LineId={lineId}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ParameterDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<ParameterDto> GetLotDeviceReferenceAsync(string referenceId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References/Parameters?ReferenceId={referenceId}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ParameterDto>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }
        public async Task<IEnumerable<EmployeeDto>> GetAllPersonAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Employees");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<EmployeeDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task CreatePersonAsync(EmployeeWorkingDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Employees", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        public async Task DeletePersonAsync(string employeeId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}/api/Employees/{employeeId}");

            response.EnsureSuccessStatusCode();
        }
        
        //Trang tao lo
        public async Task AddPersonToDeviceAsync(string stationId, AddPersonToDeviceDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Employees/WorkRecords/{stationId}", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }
        
        //Trang tao lo
        public async Task UpdatePersonToDeviceAsync(string stationId, AddPersonToDeviceDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Employees/WorkRecords/{stationId}", content);
            response.EnsureSuccessStatusCode();
        }

        //Trang tao lo
        public async Task DeletePersonToDeviceAsync(string stationId, AddPersonToDeviceDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage httpRequest = new()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{serverUrl}/api/Employees/WorkRecords/{stationId}"),
                Content = content,
            };
            
            HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
        }

        //Trang lich su loi
        public async Task<IEnumerable<ErrorStatusDto>> GetErrorsHistoryAsync(string stationId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ErrorInformations?StationId={stationId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ErrorStatusDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        // Trang trang thai may
        public async Task<IEnumerable<MachineStatusDto>> GetStatusHistoryAsync(string stationId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/MachineStatuses?StationId={stationId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<MachineStatusDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }       

        //Bao cao tong
        public async Task<IEnumerable<ShiftReportDto>> GetShiftReportHistoryAsync(string stationId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports?StationId={stationId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ShiftReportDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        //No using
        public async Task<IEnumerable<DataPoint>> GetLastestOEEAsync(string stationId, int interval)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports/Latest?StationId={stationId}&Interval={interval}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<DataPoint>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }        

        //Bao cao theo ca
        public async Task<IEnumerable<ShiftReportWithShotDto>> GetShortenShiftReportWithShotByShiftIdAsync(string shiftReportId, int interval)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports/ShortenDetails?ShiftReportId={shiftReportId}&Interval={interval}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ShiftReportWithShotDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        //No using
        public async Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByShiftIdAsync(int shiftReportId, int pageIndex, int pageSize)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports/Details?ShiftReportId={shiftReportId}&PageIndex={pageIndex}&PageSize={pageSize}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ShiftReportWithShotDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        //No using
        public async Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByDateAsync(DateTime Date, int shiftNumber)
        {
            string date = Date.ToString("yyyy-MM-dd");
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports/Details?Date={date}&StationId={shiftNumber}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ShiftReportWithShotDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        //Download Report
        public async Task<byte[]> DownloadShiftReportFileAsync(string stationId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports/DownloadReport?StationId={stationId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsByteArrayAsync();
            return responseBody;
        }
    }
}
