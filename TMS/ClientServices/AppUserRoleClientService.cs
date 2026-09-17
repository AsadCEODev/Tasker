using System.Net.Http.Json;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Pagination;

namespace TMS.ClientServices
{
    public class AppUserRoleClientService : IAppUserRoleClientService
    {
        private readonly string baseUrl = "api/AppUserRole";
        private readonly HttpClient httpClient;
        public AppUserRoleClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<PaginationResponse<AppUserRoleDto>> GetAll(FilterModel filters)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetAll", filters);
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<PaginationResponse<AppUserRoleDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SaveOrUpdate(AppUserRoleDto dto)
        {
            try
            {
                var respponse = await httpClient.PostAsJsonAsync($"{baseUrl}/SaveOrUpdate", dto);
                if (respponse.IsSuccessStatusCode)
                {
                    var result = await respponse.Content.ReadFromJsonAsync<bool>();
                    return result;
                }
                return false;
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

    public interface IAppUserRoleClientService
    {
        Task<PaginationResponse<AppUserRoleDto>> GetAll(FilterModel filters);
        Task<bool> SaveOrUpdate(AppUserRoleDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
