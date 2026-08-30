using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.ClientServices
{
    public class UserAssignedTaskClientService : IUserAssignedTaskClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "api/UserAssignedTasks";
        public UserAssignedTaskClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<PaginationResponse<UserTaskDto>> GetTasksByUser(FilterDto filter)
        {
            try
            {
              
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetTasksByUser",filter);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginationResponse<UserTaskDto>>();

                    return result ?? new PaginationResponse<UserTaskDto>();
                }

                return new PaginationResponse<UserTaskDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateUserTask(UserTaskDto dto, IBrowserFile? file)
        {
            try
            {
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(dto.TaskId.ToString()), nameof(dto.TaskId));
                content.Add(new StringContent(dto.UserProgress.ToString()), nameof(dto.UserProgress));
                content.Add(new StringContent(dto.Remarks ?? string.Empty), nameof(dto.Remarks));

                if (file != null)
                {
                    var memoryStream = new MemoryStream();
                    await file.OpenReadStream(1024 * 1024 * 10).CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    content.Add(new StreamContent(memoryStream), "file", file.Name);
                }

                var response = await httpClient.PostAsync($"{baseUrl}/UpdateUserTask", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ChangeStatus(SetupTaskDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/ChangeStatus", dto);
                if(response.IsSuccessStatusCode)
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

    public interface IUserAssignedTaskClientService
    {
        Task<PaginationResponse<UserTaskDto>> GetTasksByUser(FilterDto filter);
        Task<bool> UpdateUserTask(UserTaskDto dto, IBrowserFile? file);
        Task<bool> ChangeStatus(SetupTaskDto dto);
    }
}
