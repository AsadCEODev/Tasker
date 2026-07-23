using System.Net.Http.Json;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.ClientServices
{
    public class UserClientService : IUserClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "api/Users";
        public UserClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<PaginationResponse<SetupUserDto>> GetAll(int pageIndex, int pageSize, string? queryString)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<PaginationResponse<SetupUserDto>>($"{baseUrl}/GetAll?pageIndex={pageIndex}&pageSize={pageSize}&queryString={queryString}");

                return result ?? new PaginationResponse<SetupUserDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<SetupUserDto> GetById(long id)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<SetupUserDto>($"{baseUrl}/GetById?id={id}");
                if (result == null)
                {
                    return new SetupUserDto();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> Save(SetupUserDto setupUserDto)
        {
            var response = await httpClient.PostAsJsonAsync($"{baseUrl}/Save", setupUserDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }

            // Yahan aap status code check kar sakte hain
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return -1; // Already exists
            }

            return 0; // General error
        }
        public async Task<int> Update(SetupUserDto setupUserDto)
        {
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{baseUrl}/Update", setupUserDto);
                if (result.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    throw new Exception($"Failed to update user. Status code: {result.StatusCode}");
                }
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
                var result = await httpClient.DeleteAsync($"{baseUrl}/Delete/{id}");
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Failed to delete user. Status code: {result.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserSummary?> GetUsersSummary()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetUsersSummary");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<UserSummary>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

    public interface IUserClientService
    {
        public Task<PaginationResponse<SetupUserDto>> GetAll(int pageIndex,int pageSize, string queryString);
        public Task<SetupUserDto> GetById(long id);
        public Task<int> Save(SetupUserDto setupUserDto);
        public Task<int> Update(SetupUserDto setupUserDto);
        public Task<bool> Delete(long id);
        public Task<UserSummary?> GetUsersSummary();
    }
}