using System.Net.Http.Json;
using System.Runtime.InteropServices;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;

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
        public async Task<List<AppRoleDto>> GetRolesList()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetRolesList");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<AppRoleDto>>();
                    return lst;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AppRoleDto>> GetAll(FilterModel filters)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetAll",filters);
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<AppRoleDto>>();
                    return lst;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<AppRoleDto> GetById(long id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<AppRoleDto>();
                    return lst;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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


        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{baseUrl}/Delete?id={id}");
                if(response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<bool>();
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }

    public interface IAppRolesClientService
    {
        Task<List<AppRoleDto>> GetRolesList();
        Task<List<AppRoleDto>> GetAll(FilterModel filters);
        Task<AppRoleDto> GetById(long id);
        Task<int> SaveOrUpdate(AppRoleDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
