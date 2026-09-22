using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using TMS.Shared.Model.Filters;
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
        public async Task<PaginationResponse<SetupUserDto>> GetAll(FilterModel filter)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/GetAll", filter);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginationResponse<SetupUserDto>>();
                    return result ?? new PaginationResponse<SetupUserDto>();
                }

                return new PaginationResponse<SetupUserDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SetupUserDto>> GetUsersList()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetUsersList");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<SetupUserDto>>();
                    return lst ?? new();
                }
                return new();
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

        public async Task<int> UpdateAsync(SetupUserDto model, IBrowserFile? imageFile)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                content.Add(new StringContent(model.Id.ToString()), "Id");
                content.Add(new StringContent(model.UserName ?? ""), "UserName");
                content.Add(new StringContent(model.FullName ?? ""), "FullName");
                content.Add(new StringContent(model.FatherName ?? ""), "FatherName");
                content.Add(new StringContent(model.PhoneNo ?? ""), "PhoneNo");
                content.Add(new StringContent(model.CNIC ?? ""), "CNIC");
                content.Add(new StringContent(model.Email ?? ""), "Email");

                content.Add(new StringContent(model.DepartmentId.ToString()), "DepartmentId");

                content.Add(new StringContent(model.DesignationId.ToString()), "DesignationId" );

                content.Add(new StringContent(model.IsActive.ToString()),"IsActive" );

                content.Add(new StringContent(model.ProfileImagePath ?? ""),"ProfileImagePath");

                content.Add( new StringContent(model.HashPassword ?? ""), "HashPassword");

                content.Add(new StringContent(model.ConfirmPassword ?? ""),"ConfirmPassword");

                if (imageFile != null)
                {
                    var memoryStream = new MemoryStream();
                    await imageFile.OpenReadStream(1024 * 1024 * 10).CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset stream position back to start

                    content.Add(new StreamContent(memoryStream), "imageFile", imageFile.Name);
                }

                var response = await httpClient.PostAsync($"{baseUrl}/UpdateProfile", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<int>();
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Client Service Error: {ex.Message}",
                    ex
                );
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
        Task<PaginationResponse<SetupUserDto>> GetAll(FilterModel filters);
        Task<List<SetupUserDto>> GetUsersList();
        Task<SetupUserDto> GetById(long id);
        Task<int> Save(SetupUserDto setupUserDto);
        Task<int> Update(SetupUserDto setupUserDto);
        Task<bool> Delete(long id);
        Task<UserSummary?> GetUsersSummary();
        Task<int> UpdateAsync(SetupUserDto model, IBrowserFile imageFile);
    }
}