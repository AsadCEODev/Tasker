using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TMS.Shared.Model;

namespace TMS.ClientServices
{
    public class AppRoleScreensClientService : IAppRoleScreensClientService
    {
        private readonly string baseUrl = "Api/AppRoleScreens";
        private readonly HttpClient httpClient;
        public AppRoleScreensClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<AppRolesScreenDto>> GetUserPermissions()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetUserPermissions");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<AppRolesScreenDto>>();
                    return data;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }

    public interface IAppRoleScreensClientService
    {
        Task<List<AppRolesScreenDto>> GetUserPermissions();
    }
}
