using System.Net.Http.Json;
using System.Text.Json;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;


namespace TMS.ClientServices
{
    public class TaskClientService : ITaskClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "api/Task";
        public TaskClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<PaginationResponse<SetupTaskDto>> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetAll?pageNumber={pageIndex}&pageSize={pageSize}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<PaginationResponse<SetupTaskDto>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result;
                }
                return new PaginationResponse<SetupTaskDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                 
        }

        public async Task<SetupTaskDto> GetById(long Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?Id={Id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<SetupTaskDto>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result;
                }
                return new SetupTaskDto();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<int> Save(SetupTaskDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/Save", dto);
                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return -1; // Already exists
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Update(SetupTaskDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/Update", dto);
                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return -1; // Already exists
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(long id)
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
    }

    public interface ITaskClientService
    {
        Task<PaginationResponse<SetupTaskDto>> GetAll(int pageIndex, int pageSize);
        Task<SetupTaskDto> GetById(long Id);
        Task<int> Save(SetupTaskDto dto);
        Task<int> Update(SetupTaskDto dto);
        Task<bool> Delete(long id);
    }
}
