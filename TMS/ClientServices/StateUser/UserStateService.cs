
using System.Net.Http.Json;
using TMS.Shared.Model.Setup; 

namespace TMS.ClientServices.StateUser
{
    public class UserStateService
    {
        private readonly HttpClient _http;
        private SetupUser? _cachedUser;
        private bool _isLoaded = false;

        public UserStateService(HttpClient http)
        {
            _http = http;
        }

        public async Task<SetupUser?> GetCurrentUserAsync()
        {
            if (_isLoaded && _cachedUser != null)
            {
                return _cachedUser;
            }

            try
            {
                var response = await _http.GetAsync("api/auth/GetCurrentUserInfo");
                if (response.IsSuccessStatusCode)
                {
                    _cachedUser = await response.Content.ReadFromJsonAsync<SetupUser>();
                    _isLoaded = true;
                }
            }
            catch
            {
                _cachedUser = null;
            }

            return _cachedUser;
        }

        public void ClearState()
        {
            _cachedUser = null;
            _isLoaded = false;
        }
    }
}
