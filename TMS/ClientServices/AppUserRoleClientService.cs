using System.Net.Http.Json;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
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

        public async Task<List<AppUsersListDto>> GetUnAssignedUsersList(FilterModel filter)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetUnAssignedUsersList",filter);
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<AppUsersListDto>>();
                    return lst;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<long>> GetAssignedUsersList(FilterModel filter)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetAssignedUsersList", filter);
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<long>>();
                    return lst;
                }
                return new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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
        Task<List<AppUsersListDto>> GetUnAssignedUsersList(FilterModel filter);
        Task<List<long>> GetAssignedUsersList(FilterModel filter);
        Task<PaginationResponse<AppUserRoleDto>> GetAll(FilterModel filters);
        Task<bool> SaveOrUpdate(AppUserRoleDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
