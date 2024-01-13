using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using WEMBLEY.DemoApp.Core.Domain.Dtos;
using WEMBLEY.DemoApp.Core.Domain.Dtos.DeviceReferences;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Devices;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ErrorInformations;
using WEMBLEY.DemoApp.Core.Domain.Dtos.MachineStatus;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Persons;
using WEMBLEY.DemoApp.Core.Domain.Dtos.Products;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;
using WEMBLEY.DemoApp.Core.Domain.Dtos.ShiftReports;
using WEMBLEY.DemoApp.Core.Domain.Exceptions;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        private const string serverUrl = "https://wembleyscadaapi20240113161149.azurewebsites.net/";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<DeviceDto>> GetAllDeviceTypeAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Devices");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<DeviceDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByDeviceTypeAsync(string deviceType)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Products?DeviceType={deviceType}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<IEnumerable<ReferenceDto>> GetReferencesByDeviceTypeAsync(string deviceType)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References?DeviceType={deviceType}");

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

        public async Task UpdateLotAsync(string refName, CreateLotDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/References/{refName}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task CompleteRefAsync(string refName)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/References/Parameters/Completed/{refName}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<DeviceReferenceDto>> GetDeviceReferenceMFCAsync(int referenceId, string deviceId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/DeviceReferences?ReferenceId={referenceId}&DeviceId={deviceId}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<DeviceReferenceDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task FixMFCAsync(int refId, string deviceId, IEnumerable<MFCDto> fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/DeviceReferences/{deviceId}/{refId}", content);
            response.EnsureSuccessStatusCode();
        }

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

        public async Task<IEnumerable<ParameterDto>> GetLotDeviceReferenceByDeviceTypeAsync(string deviceType)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References/Parameters?DeviceType={deviceType}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ParameterDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task<ParameterDto> GetLotDeviceReferenceAsync(int refId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/References/Parameters?ReferenceId={refId}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ParameterDto>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }
        public async Task<IEnumerable<PersonDto>> GetAllPersonAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Persons");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<PersonDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }
            return result;
        }

        public async Task CreatePersonAsync(PersonWorkingDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Persons", content);

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

        public async Task DeletePersonAsync(string personId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}/api/Persons/{personId}");

            response.EnsureSuccessStatusCode();
        }

        public async Task AddPersonToDeviceAsync(string deviceId, AddPersonToDeviceDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Persons/PersonWorkRecords/{deviceId}", content);

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

        public async Task UpdatePersonToDeviceAsync(string deviceId, AddPersonToDeviceDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Persons/PersonWorkRecords/{deviceId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePersonToDeviceAsync(string deviceId, AddPersonToDeviceDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage httpRequest = new()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{serverUrl}/api/Persons/PersonWorkRecords/{deviceId}"),
                Content = content,
            };
            
            HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
        }


        public async Task<IEnumerable<ErrorStatusDto>> GetErrorsHistoryAsync(string deviceId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ErrorInformations?DeviceId={deviceId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ErrorStatusDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        public async Task<IEnumerable<MachineStatusDto>> GetStatusHistoryAsync(string deviceId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/MachineStatus?DeviceId={deviceId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<MachineStatusDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        public async Task<IEnumerable<ShiftReportDto>> GetShiftReportHistoryAsync(string deviceId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports?DeviceId={deviceId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ShiftReportDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

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

        public async Task<IEnumerable<ShiftReportWithShotDto>> GetShiftReportWithShotByDateAsync(DateTime Date, int shiftNumber)
        {
            string date = Date.ToString("yyyy-MM-dd");
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports/Details?Date={date}&ShiftNumber={shiftNumber}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ShiftReportWithShotDto>>(responseBody);

            if (result is null)
            {
                throw new Exception();
            }

            return result;
        }

        public async Task<byte[]> DownloadShiftReportFileAsync(string deviceId, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/ShiftReports/downloadReport?DeviceId={deviceId}&StartTime={startDateString}&EndTime={endDateString}");

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsByteArrayAsync();
            return responseBody;
        }
    }
}
