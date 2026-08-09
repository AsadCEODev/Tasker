using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace TSM.API.AuthDeligator
{
    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;

        public JwtAuthorizationHandler(IJSRuntime js)
        {
            _js = js;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await _js.InvokeAsync<string?>(
                "sessionStorage.getItem",
                "authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", token);
            }

            return await base.SendAsync(
                request,
                cancellationToken);
        }
    }
}