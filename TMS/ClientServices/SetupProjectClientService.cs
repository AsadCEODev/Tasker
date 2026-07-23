using System.Net.Http.Json;
using TMS.Shared.Model.Setup;

namespace TMS.ClientServices
{
    public class SetupProjectClientService : ISetupProjectClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "Api/Projects";
        public SetupProjectClientService(HttpClient _httpClient)
        {
            
            httpClient = _httpClient;
        }

        public async Task<List<SetupProjectDto>> GetAll()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<SetupProjectDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<SetupProjectDto> GetById(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var obj = await response.Content.ReadFromJsonAsync<SetupProjectDto>();
                    return obj ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public interface ISetupProjectClientService
    {
        Task<List<SetupProjectDto>> GetAll();
        Task<SetupProjectDto> GetById(int id);
    }
}
