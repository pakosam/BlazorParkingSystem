using Blazored.LocalStorage;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using static ParkingSystem.Pages.Login;

public class AuthStateService
{
    private readonly IJSRuntime _jsRuntime;

    public event Action? OnChange;

    public AuthStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task LogInAsync(string token)
    {
        Console.WriteLine($"Token: {token}");
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        NotifyStateChanged();
    }

    public async Task LogOutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        NotifyStateChanged();  
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();  
    }
}
