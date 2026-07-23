using System.Net.Http.Json;
using TMS.Shared.Model.Setup;

namespace TMS.ClientServices
{
    public class TagsClientService : ITagsClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "Api/Tags";

        public TagsClientService(HttpClient _httpClient)
        {
            try
            {
                httpClient = _httpClient;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SetupTagDto>> GetAll()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var lst = await response.Content.ReadFromJsonAsync<List<SetupTagDto>>();
                    return lst ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<SetupTagDto> GetById(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var obj = await response.Content.ReadFromJsonAsync<SetupTagDto>();
                    return obj ?? new();
                }
                return new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public interface ITagsClientService
    {
        Task<List<SetupTagDto>> GetAll();
        Task<SetupTagDto> GetById(int id);
    }
}
