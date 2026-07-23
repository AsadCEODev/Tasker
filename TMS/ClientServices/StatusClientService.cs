using System.Net.Http.Json;
using TMS.Shared.Model.Setup;

namespace TMS.ClientServices
{
    public class StatusClientService: IStatusClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "Api/Status";

        public StatusClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
           
        }

        public async Task<List<SetupStatusDto>> GetAll()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<SetupStatusDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<SetupStatusDto> GetById(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<SetupStatusDto>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

    public interface IStatusClientService
    {
        Task<List<SetupStatusDto>> GetAll();
        Task<SetupStatusDto> GetById(int id);
    }
}
