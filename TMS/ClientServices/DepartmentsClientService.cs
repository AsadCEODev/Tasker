using System.Net.Http.Json;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.ClientServices
{
    public class DepartmentsClientService : IDepartmentsClientService
    {
        private readonly string baseUrl = "Api/Departments";
        private readonly HttpClient httpClient;
        public DepartmentsClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<PaginationResponse<SetupDepartmentDto>> GetAllDepartmentsAsync(FilterModel filter)
        {
            try
            {

                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetAll", filter);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginationResponse<SetupDepartmentDto>>();
                    return result ?? new();
                }

                return new PaginationResponse<SetupDepartmentDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<SetupDepartmentDto>> GetDepartmentsList()
        {
            try
            {

                var response = await httpClient.GetAsync($"{baseUrl}/GetDepartmentsList");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<SetupDepartmentDto>>();
                    return result ?? new();
                }

                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<SetupDepartmentDto> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var found = await response.Content.ReadFromJsonAsync<SetupDepartmentDto>();
                    return found ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<int> SaveDepartmentAsync(SetupDepartmentDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/Save", dto);
                int result = await response.Content.ReadFromJsonAsync<int>();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<int> UpdateDepartmentAsync(SetupDepartmentDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/Update", dto);
                int result = await response.Content.ReadFromJsonAsync<int>();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{baseUrl}/Delete?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<vwDepartmentSummaryDataDto> GetDepartmentSummaryDataAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetDepartmentSummary");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<vwDepartmentSummaryDataDto>();
                    return data ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }

    public interface IDepartmentsClientService
    {
        Task<PaginationResponse<SetupDepartmentDto>> GetAllDepartmentsAsync(FilterModel filter);
        Task<List<SetupDepartmentDto>> GetDepartmentsList();
        Task<SetupDepartmentDto> GetDepartmentByIdAsync(int id);
        Task<int> SaveDepartmentAsync(SetupDepartmentDto dto);
        Task<int> UpdateDepartmentAsync(SetupDepartmentDto dto);
        Task<bool> DeleteDepartmentAsync(int id);
        Task<vwDepartmentSummaryDataDto> GetDepartmentSummaryDataAsync();
    }
}
