using System.Net.Http.Json;
using TMS.Shared.Model;

namespace TMS.ClientServices
{
    public class AppScreenClientService : IAppScreenClientService
    {
        private readonly string baseUrl = "api/AppScreen";
        private readonly HttpClient httpClient;
        public AppScreenClientService( HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<AppScreenDto>> GetAllScreensListAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<AppScreenDto>>();
                    return lst;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<AppScreenDto> GetScreensByIdAsync(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var found = await response.Content.ReadFromJsonAsync<AppScreenDto>();
                    return found;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }

    public interface IAppScreenClientService
    {
        Task<List<AppScreenDto>> GetAllScreensListAsync();
        Task<AppScreenDto> GetScreensByIdAsync(int id);
    }
}
