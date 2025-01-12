// /Services/ApiService.cs
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.JSInterop;

namespace ParkingSystem.Services
{
	public class ApiService
	{
		private readonly HttpClient _httpClient;
		private readonly IJSRuntime _jsRuntime;

		public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
		{
			_httpClient = httpClient;
			_jsRuntime = jsRuntime;
		}

		private async Task<string> GetAuthTokenAsync()
		{
			var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
			return token;
		}

        public async Task<HttpResponseMessage> MakeRequestAsync(string url, HttpMethod method = null, object? data = null)
        {
            var token = await GetAuthTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Authorization token is missing.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestMessage = new HttpRequestMessage(method ?? HttpMethod.Get, url);

            // If data is provided, serialize it into JSON and set it as content
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return await _httpClient.SendAsync(requestMessage);
        }

    }
}
