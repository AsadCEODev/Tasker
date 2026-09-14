using System.Net.Http.Json;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.ClientServices
{
    public class AssignProjectClientService : IAssignProjectClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "api/AssignUserProjects";
        public AssignProjectClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public async Task<PaginationResponse<UserProjectDto>> GetAll(int pageIndex, int pageSize, string? queryString)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetAll?pageIndex={pageIndex}&pageSize={pageSize}&queryString={queryString}");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<PaginationResponse<UserProjectDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<UserProjectDto>> GetByUserId(long userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetByUserId?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<UserProjectDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<UserProjectDto>> GetById(long Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetByUserId?Id={Id}");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<UserProjectDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Save(UserAssignProjectsDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/Save", dto);
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
        public async Task<bool> Detele(long id)
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

        public async Task<vwvwAssignedProjectSummryDto> GetAssignedProjectSummaryAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetAssignedProjectSummary");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<vwvwAssignedProjectSummryDto>();
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

    public interface IAssignProjectClientService
    {
        Task<PaginationResponse<UserProjectDto>> GetAll(int pageIndex, int pageSize, string queryString);
        Task<List<UserProjectDto>> GetByUserId(long userId);
        Task<bool> Save(UserAssignProjectsDto dto);
        Task<bool> Detele(long id);
        Task<vwvwAssignedProjectSummryDto> GetAssignedProjectSummaryAsync();
    }
}
