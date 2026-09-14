using System.Net.Http.Json;
using System.Runtime.InteropServices;
using TMS.Shared.Model;

namespace TMS.ClientServices
{
    public class AppRolesClientService : IAppRolesClientService
    {

        private readonly string baseUrl = "api/AppRoles";
        private readonly HttpClient httpClient;

        public AppRolesClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<int> SaveOrUpdate(AppRoleDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/SaveOrUpdate", dto);
                var result =await response.Content.ReadFromJsonAsync<int>();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }

    public interface IAppRolesClientService
    {
        Task<int> SaveOrUpdate(AppRoleDto dto);
    }
}
