using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;


namespace TMS.ClientServices
{
    public class TaskClientService : ITaskClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "api/Tasks";
        public TaskClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<PaginationResponse<SetupTaskDto>> GetAll(FilterDto filter)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetAll", filter);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<PaginationResponse<SetupTaskDto>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result;
                }
                return new PaginationResponse<SetupTaskDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                 
        }

        public async Task<SetupTaskDto> GetById(long Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?Id={Id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<SetupTaskDto>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result;
                }
                return new SetupTaskDto();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<int> Save(SetupTaskDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = await BuildMultipartContent(dto, file);
                var response = await httpClient.PostAsync($"{baseUrl}/Save", content);

                if (response.IsSuccessStatusCode) return 1;
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict) return -1;
                return 0;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Update(SetupTaskDto dto, IBrowserFile? file)
        {
            try
            {
                using var content = await BuildMultipartContent(dto, file);
                var response = await httpClient.PostAsync($"{baseUrl}/Update", content);

                if (response.IsSuccessStatusCode) return 1;
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict) return -1;
                return 0;
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

        private  async Task<MultipartFormDataContent> BuildMultipartContent(SetupTaskDto dto, IBrowserFile? file)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(dto.Id.ToString()), nameof(dto.Id));
            content.Add(new StringContent(dto.TaskTitle ?? string.Empty), nameof(dto.TaskTitle));
            content.Add(new StringContent(dto.TaskDesc ?? string.Empty), nameof(dto.TaskDesc));
            content.Add(new StringContent(dto.TagId.ToString()), nameof(dto.TagId));
            content.Add(new StringContent(dto.ProjectId.ToString()), nameof(dto.ProjectId));
            //content.Add(new StringContent(dto.UserId.ToString()), nameof(dto.UserId));
       
            // ISO format for DateTime
            content.Add(new StringContent(dto.DueDate.ToString("o")), nameof(dto.DueDate));

            if (dto.StatusId.HasValue)
            {
                content.Add(new StringContent(dto.StatusId.Value.ToString()), nameof(dto.StatusId));
            }

            if (dto.Progress.HasValue)
            {
                content.Add(new StringContent(dto.Progress.Value.ToString()), nameof(dto.Progress));
            }

            if (!string.IsNullOrEmpty(dto.FileName))
            {
                content.Add(new StringContent(dto.FileName), nameof(dto.FileName));
            }
            content.Add(new StringContent(dto.IsStart.ToString()),"IsStart");
            content.Add(new StringContent(dto.TaskTime.ToString()), "TaskTime");

            if (dto.UserTasks != null && dto.UserTasks.Any())
            {
                int index = 0;
                foreach (var userTask in dto.UserTasks)
                {
                    content.Add(new StringContent(userTask.UserId.ToString()), $"UserTasks[{index}].UserId");
                    index++;
                }
            }
            if (file != null)
            {
                var memoryStream = new MemoryStream();
                await file.OpenReadStream(1024 * 1024 * 10).CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset stream position back to start

                content.Add(new StreamContent(memoryStream), "file", file.Name);
            }

            return content;
        }


        public async Task<TaskSummaryDto> GetTasksSummary(FilterDto filter)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetTasksSummary", filter);

                if (response.IsSuccessStatusCode)
                {
                    // ReadAsFromJsonAsync automatically JSON ko deserialize kar deta hai
                    var result = await response.Content.ReadFromJsonAsync<TaskSummaryDto>(new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result ?? new TaskSummaryDto();
                }

                return new TaskSummaryDto();
            }
            catch (Exception ex)
            {
                
                throw new Exception($"An error occurred while fetching task summary: {ex.Message}", ex);
            }
        }
    }

    public interface ITaskClientService
    {
        Task<PaginationResponse<SetupTaskDto>> GetAll(FilterDto filter);
        Task<SetupTaskDto> GetById(long id);
        Task<int> Save(SetupTaskDto dto, IBrowserFile? file);
        Task<int> Update(SetupTaskDto dto, IBrowserFile? file);
        Task<bool> Delete(long id);
        Task<TaskSummaryDto> GetTasksSummary(FilterDto filter);
    }
}
