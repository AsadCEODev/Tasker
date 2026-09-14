using System.Net.Http.Json;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.ClientServices
{
    public class DesignationsClientService : IDesignationsClientService
    {
        private readonly string baseUrl = "Api/Designations";
        private readonly HttpClient httpClient;
        public DesignationsClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<PaginationResponse<SetupDesignationDto>> GetAllDesignationsAsync(FilterModel filter)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetAll", filter);
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<PaginationResponse<SetupDesignationDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SetupDesignationDto>> GetDesignationsList()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetDesignationsList");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<SetupDesignationDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<SetupDesignationDto> GetDesignationByIdAsync(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var found = await response.Content.ReadFromJsonAsync<SetupDesignationDto>();
                    return found ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<int> SaveDesignationAsync(SetupDesignationDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/Save",dto);
                int result = await response.Content.ReadFromJsonAsync<int>();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<int> UpdateDesignationAsync(SetupDesignationDto dto)
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

        public async Task<bool> DeleteDesignationAsync(int id)
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

        public async Task<vwDesignationSummaryDataDto> GetDesignationSummaryDataAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetDesignationSummary");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<vwDesignationSummaryDataDto>();
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

    public interface IDesignationsClientService
    {
        Task<PaginationResponse<SetupDesignationDto>> GetAllDesignationsAsync(FilterModel filter);
        Task<List<SetupDesignationDto>> GetDesignationsList();
        Task<SetupDesignationDto> GetDesignationByIdAsync(int id);
        Task<int> SaveDesignationAsync(SetupDesignationDto dto);
        Task<int> UpdateDesignationAsync(SetupDesignationDto dto);
        Task<bool> DeleteDesignationAsync(int id);
        Task<vwDesignationSummaryDataDto> GetDesignationSummaryDataAsync();
    }
}
