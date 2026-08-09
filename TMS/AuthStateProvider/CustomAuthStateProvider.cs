using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace TMS.AuthStateProvider
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;

        public CustomAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _js.InvokeAsync<string?>(
                    "sessionStorage.getItem",
                    "authToken");

                if (string.IsNullOrWhiteSpace(token))
                {
                    return Anonymous();
                }

                var identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, "AuthenticatedUser")
                    },
                    authenticationType: "JwtAuth");

                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return Anonymous();
            }
        }

        public void NotifyUserLogin(string token)
        {
            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, "AuthenticatedUser")
                },
                authenticationType: "JwtAuth");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(user)));
        }

        public async Task NotifyUserLogout()
        {
            await _js.InvokeVoidAsync("sessionStorage.removeItem","authToken");

            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
        }

        private static AuthenticationState Anonymous()
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()));
        }
    }
}