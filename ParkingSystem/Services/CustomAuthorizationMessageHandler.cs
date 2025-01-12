using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

public class CustomAuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthorizationMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Retrieve the token from localStorage
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); // Attach the token
        }

        return await base.SendAsync(request, cancellationToken);
    }

}
